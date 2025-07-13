using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IPoolable, IUpdater
{
    [Header("References")]
    [SerializeField] private EnemyDataSO _enemyDataSO;
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyModel _enemyModel;

    [Header("Temp Data")]
    [SerializeField] private Queue<Waypoint> _targetPath;
    [SerializeField] private Waypoint _currentPoint;
    [SerializeField] private float _stopTimer = 0f;
    [SerializeField] private bool _isWaiting = false;

    [Header("Settings")]
    [SerializeField] private string _animatorSpeedParamter = "Velocity";

    #region Start Methods
    private void OnEnable()
    {
        UpdaterManager.Instance.AddIUpdaterInList(this);
    }

    public void Start()
    {
        PickNewTargetPath();
        _stopTimer = _enemyDataSO.StopTime;
        _isWaiting = true;
    }

    private void OnDisable()
    {
        UpdaterManager.Instance.RemoveIUpdaterInList(this);
    }
    #endregion

    #region IUpdater
    public void UpdateSection()
    {
        if(!_enemyModel.IsAlive || _enemyModel.OnStack)
        {
            return;
        }

        if (_isWaiting)
        {
            _stopTimer -= Time.deltaTime;
            if (_stopTimer <= 0f)
            {
                _isWaiting = false;
            }
            return;
        }

        if(_currentPoint != null)
        {
            float distance = Vector3.Distance(transform.position, _currentPoint.transform.position);

            if (distance > _enemyDataSO.PointReachDistance)
            {
                Vector3 direction = (_currentPoint.transform.position - transform.position).normalized;
                float inputMagnitude = direction.magnitude;

                float normalizedSpeed = Mathf.Clamp01(inputMagnitude);
                _animator.SetFloat(_animatorSpeedParamter, normalizedSpeed);
                transform.position += direction * _enemyDataSO.MoveSpeed * Time.deltaTime;
                transform.forward = direction;
            }
            else
            {
                _animator.SetFloat(_animatorSpeedParamter, 0);
                _stopTimer = _enemyDataSO.StopTime;
                _isWaiting = true;

                if (_targetPath.Count > 0)
                {
                    _currentPoint = _targetPath.Dequeue();
                }
                else
                {
                    PickNewTargetPath();
                }
            }
        }
        else
        {
            PickNewTargetPath();
        }
    }
    #endregion

    #region Enemy Methods
    private void PickNewTargetPath()
    {
        _targetPath = WaypointManager.Instance.GenerateRandomPath(transform.position);
        _currentPoint = _targetPath.Dequeue();
    }
    #endregion

    #region Ipoolable
    public void OnDespawned()
    {
        
    }

    public void Destroy()
    {
        
    }

    public void OnPooled()
    {
        
    }
    #endregion
}
