using System.Collections.Generic;
using UnityEngine;

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

    private void OnEnable()
    {
        _playerModel.OnEnemyEliminated += AddEnemyToStack;
    }

    private void Start()
    {
        UpdaterManager.Instance.AddIUpdaterInList(this);
    }

    private void OnDisable()
    {
        _playerModel.OnEnemyEliminated -= AddEnemyToStack;
    }

    public void AddEnemyToStack(Transform target)
    {
        if(target.TryGetComponent(out IStackable stackable))
        {
            stackable.ActiveOnStackState();
            Vector3 stackOffset = new Vector3(0, _stackHeight, 0);
            Transform last = _stack.Count > 0 ? _stack[_stack.Count - 1].transform : _stackPoint;
            target.position = last.position + stackOffset;
            _stack.Add(target);
        }
    }

    public void UpdateSection()
    {
        Vector3 current = _stackPoint.position;
        for (int i = 0; i < _stack.Count; i++)
        {
            Vector3 desired = i == 0 ? current + Vector3.up * 1.2f : _stack[i - 1].position + Vector3.up * 1.2f;
            _stack[i].position = Vector3.Lerp(_stack[i].position, desired, Time.deltaTime * _inertiaSpeed);

            Vector3 baseEuler = transform.rotation.eulerAngles;

            Vector3 targetEuler = baseEuler + _rotationOffset;

            Vector3 currentEuler = _stack[i].rotation.eulerAngles;

            if ((_lockAxes & RotationAxis.X) != 0) targetEuler.x = currentEuler.x;
            if ((_lockAxes & RotationAxis.Y) != 0) targetEuler.y = currentEuler.y;
            if ((_lockAxes & RotationAxis.Z) != 0) targetEuler.z = currentEuler.z;

            Quaternion filteredRotation = Quaternion.Euler(targetEuler);
            _stack[i].rotation = Quaternion.Lerp(_stack[i].rotation, filteredRotation, Time.deltaTime * _inertiaSpeed);

        }
    }
}