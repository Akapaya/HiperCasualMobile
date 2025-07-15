using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The sales area has its own interface of areas and methods to handle the stack items that are dumped into it.
/// </summary>
public class SellArea : MonoBehaviour, IArea
{
    [Header("References")]
    [SerializeField] private PlayerStack _playerStack;

    [Header("Settings")]
    [SerializeField] private AreaEnum.AreasTypes _areaType;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private float _throwDuration = 0.5f;
    [SerializeField] private float _arcHeight = 2f;
    [SerializeField] private float _timeUntilCleanStack = 3f;

    [Header("Temp Data")]
    [SerializeField] private List<StackItemStruct> _stackItems = new(10);

    [Header("Events")]
    [SerializeField] private Action<int> OnSellItem;

    #region Start Methods
    private void OnEnable()
    {
        AreaManager.Instance.RegisterInDictionary(_areaType, this);
    }
    #endregion

    #region IArea Methods
    public void ActivateArea()
    {
        StartCoroutine(SellRoutine());
    }

    public void DeactivateArea()
    {
        StartCoroutine(CleanStack());
    }

    public void RegisterObserver(Action<int> action)
    {
        OnSellItem += action;
    }

    public void UnregisterObserver(Action<int> action)
    {
        OnSellItem -= action;
    }
    #endregion

    #region Sell Methods
    /// <summary>
    /// Routine to start sell all stack items dropped
    /// </summary>
    private IEnumerator SellRoutine()
    {
        while (_playerStack.Stack.Count > 0)
        {
            var obj = _playerStack.Stack[_playerStack.Stack.Count - 1];
            _playerStack.Stack.RemoveAt(_playerStack.Stack.Count - 1);
            _stackItems.Add(obj);
            obj.Item.transform.parent = null;

            StartCoroutine(MoveInArc(obj));

            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Animation to move in Arc the stack item dropped
    /// </summary>
    /// <param name="obj">Transform of stack item</param>
    private IEnumerator MoveInArc(StackItemStruct obj)
    {
        Vector3 start = obj.Item.transform.position;

        Vector3 peak = Vector3.Lerp(start, _targetPoint.position, 0.5f);
        peak.y += _arcHeight;

        float halfDuration = _throwDuration / 2f;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            obj.Item.transform.position = Vector3.Lerp(start, peak, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.Item.transform.position = peak;

        elapsed = 0f;

        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            obj.Item.transform.position = Vector3.Lerp(peak, _targetPoint.position, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.Item.transform.position = _targetPoint.position;
        OnSellItem?.Invoke(obj.SellItem.ValueItem);
    }

    /// <summary>
    /// Routine to clean Stack after a period, deactive items and return to pool
    /// </summary>
    private IEnumerator CleanStack()
    {
        yield return new WaitForSeconds(_timeUntilCleanStack);

        while (_stackItems.Count > 0)
        {
            StackItemStruct obj = _stackItems[_stackItems.Count - 1];
            _stackItems.RemoveAt(_stackItems.Count - 1);

            string family = obj.Stackable.StackFamily;

            ObjectPoolManager.Instance.Return(family, obj.Item.gameObject);

            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion
}