using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private List<Transform> _stackItems = new(10);

    [Header("Events")]
    [SerializeField] private Action<int> OnSellItem;

    private void OnEnable()
    {
        AreaManager.Instance.RegisterInDictionary(_areaType, this);
    }

    public void ActivateArea()
    {
        StartCoroutine(SellRoutine());
    }

    private IEnumerator SellRoutine()
    {
        while (_playerStack.Stack.Count > 0)
        {
            var obj = _playerStack.Stack[_playerStack.Stack.Count - 1];
            _playerStack.Stack.RemoveAt(_playerStack.Stack.Count - 1);
            _stackItems.Add(obj);
            obj.transform.parent = null;

            StartCoroutine(MoveInArc(obj.transform));

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator MoveInArc(Transform obj)
    {
        Vector3 start = obj.position;

        Vector3 peak = Vector3.Lerp(start, _targetPoint.position, 0.5f);
        peak.y += _arcHeight;

        float halfDuration = _throwDuration / 2f;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            obj.position = Vector3.Lerp(start, peak, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.position = peak;

        elapsed = 0f;

        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            obj.position = Vector3.Lerp(peak, _targetPoint.position, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.position = _targetPoint.position;
        OnSellItem?.Invoke(obj.GetComponent<ISellItem>().ValueItem);
    }

    public void DeactivateArea()
    {
        StartCoroutine(CleanStack());
    }

    private IEnumerator CleanStack()
    {
        yield return new WaitForSeconds(_timeUntilCleanStack);

        while (_stackItems.Count > 0)
        {
            Transform obj = _stackItems[_stackItems.Count - 1];
            _stackItems.RemoveAt(_stackItems.Count - 1);

            string family = obj.gameObject.GetComponent<IStackable>().StackFamily;

            ObjectPoolManager.Instance.Return(family, obj.gameObject);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RegisterObserver(Action<int> action)
    {
        OnSellItem += action;
    }

    public void UnregisterObserver(Action<int> action)
    {
        OnSellItem -= action;
    }
}
