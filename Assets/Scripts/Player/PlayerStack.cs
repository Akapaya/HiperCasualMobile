using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script handles stack of objects, position, movements and rotation.
/// </summary>
public class PlayerStack : MonoBehaviour, IUpdater
{
    [Header("References")]
    [SerializeField] private PlayerDataSO _playerSettings;
    [SerializeField] private PlayerModel _playerModel;
    [SerializeField] private Transform _stackPoint;

    [Header("Temp Data")]
    [SerializeField] private List<Transform> _stack;

    [Header("Settings")]
    [SerializeField] private float _stackHeight = 0.5f;
    [SerializeField] private float _multiplyStackHeight = 1.2f;
    [SerializeField] private float _inertiaSpeed = 2f;
    [SerializeField] private Vector3 _rotationOffset;
    [SerializeField] private RotationAxis _lockAxes = RotationAxis.Y;

    public List<Transform> Stack { get => _stack;}

    [System.Flags]
    public enum RotationAxis
    {
        None = 0,
        X = 1 << 0,
        Y = 1 << 1,
        Z = 1 << 2,
        All = X | Y | Z
    }

    #region Start Methods
    private void OnEnable()
    {
        _playerModel.OnEnemyEliminated += AddObjectToStack;
    }

    private void Start()
    {
        UpdaterManager.Instance.AddIUpdaterInList(this);
    }

    private void OnDisable()
    {
        _playerModel.OnEnemyEliminated -= AddObjectToStack;
    }
    #endregion

    #region Stack Methods
    /// <summary>
    /// Add Stackable item in stack list and back of player
    /// </summary>
    /// <param name="target">transform of stackable item</param>
    public void AddObjectToStack(Transform target)
    {
        if(target.TryGetComponent(out IStackable stackable) && _stack.Count < _playerSettings.MaxStack)
        {
            stackable.ActiveOnStackState();
            Vector3 stackOffset = new Vector3(0, _stackHeight, 0);
            Transform last = _stack.Count > 0 ? _stack[_stack.Count - 1].transform : _stackPoint;
            target.position = last.position + stackOffset;
            _stack.Add(target);
        }
    }

    /// <summary>
    /// Update Stack item position
    /// </summary>
    /// <param name="index">index of stack item</param>
    /// <param name="current">current position</param>
    private void UpdateStackPosition(int index, ref Vector3 current)
    {
        Vector3 basePosition = current;
        
        if(index != 0)
        {
            basePosition = _stack[index - 1].position;
        }

        Vector3 desired = basePosition + Vector3.up * _multiplyStackHeight;

        _stack[index].position = Vector3.Lerp(_stack[index].position,desired,Time.deltaTime * _inertiaSpeed);
    }

    /// <summary>
    /// Rotation stack items using offSet Rotation, player rotation or lock each Axys
    /// </summary>
    /// <param name="index">index of stack item</param>
    private void UpdateStackRotation(int index)
    {
        Vector3 baseEuler = transform.rotation.eulerAngles;
        Vector3 targetEuler = baseEuler + _rotationOffset;
        Vector3 currentEuler = _stack[index].rotation.eulerAngles;

        if ((_lockAxes & RotationAxis.X) != 0) targetEuler.x = currentEuler.x;
        if ((_lockAxes & RotationAxis.Y) != 0) targetEuler.y = currentEuler.y;
        if ((_lockAxes & RotationAxis.Z) != 0) targetEuler.z = currentEuler.z;

        Quaternion filteredRotation = Quaternion.Euler(targetEuler);

        _stack[index].rotation = Quaternion.Lerp(_stack[index].rotation,filteredRotation,Time.deltaTime * _inertiaSpeed);
    }
    #endregion

    #region IUpdater Methods
    public void UpdateSection()
    {
        Vector3 current = _stackPoint.position;
        for (int i = 0; i < _stack.Count; i++)
        {
            UpdateStackPosition(i, ref current);
            UpdateStackRotation(i);
        }
    }
    #endregion
}