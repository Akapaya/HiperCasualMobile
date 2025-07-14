using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy behaviour is a Pool Object and use IUpdater
/// Controls the path and manages movement between waypoints and their states.
/// </summary>
public class EnemyBehavior : MonoBehaviour, IUpdater
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
        PickNewPath();
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
        if (ShouldAbortUpdate())
            return;

        if (IsWaiting())
            return;

        if (_currentPoint != null)
        {
            HandleMovementOrPathUpdate();
        }
        else
        {
            PickNewPath();
        }
    }
    #endregion

    #region Enemy Methods
    /// <summary>
    /// Check if Enemy is Alive or on player stack
    /// </summary>
    /// <returns>True if is alive and NOT on stack</returns>
    private bool ShouldAbortUpdate()
    {
        return !_enemyModel.IsAlive || _enemyModel.OnStack;
    }

    /// <summary>
    /// Handle with Waiting state until go to next way point
    /// </summary>
    /// <returns>True if is waiting</returns>
    private bool IsWaiting()
    {
        if (_isWaiting)
        {
            _stopTimer -= Time.deltaTime;
            if (_stopTimer <= 0f)
            {
                _isWaiting = false;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if close of current point
    /// </summary>
    private void HandleMovementOrPathUpdate()
    {
        float distance = Vector3.Distance(transform.position, _currentPoint.transform.position);

        if (distance > _enemyDataSO.PointReachDistance)
        {
            MoveTowardsCurrentPoint();
        }
        else
        {
            StopAtPoint();
            GetNextPointOrPath();
        }
    }

    /// <summary>
    /// Move enemy to current point using transform to avoid high cost of process
    /// </summary>
    private void MoveTowardsCurrentPoint()
    {
        Vector3 direction = (_currentPoint.transform.position - transform.position).normalized;
        float inputMagnitude = direction.magnitude;
        float normalizedSpeed = Mathf.Clamp01(inputMagnitude);

        _animator.SetFloat(_animatorSpeedParamter, normalizedSpeed);
        transform.position += direction * _enemyDataSO.MoveSpeed * Time.deltaTime;
        transform.forward = direction;
    }

    /// <summary>
    /// Handle to achieve the current point
    /// </summary>
    private void StopAtPoint()
    {
        _animator.SetFloat(_animatorSpeedParamter, 0);
        _stopTimer = _enemyDataSO.StopTime;
        _isWaiting = true;
    }

    /// <summary>
    /// Check if has other way point in current path, or get new path
    /// </summary>
    private void GetNextPointOrPath()
    {
        if (_targetPath.Count > 0)
        {
            _currentPoint = _targetPath.Dequeue();
        }
        else
        {
            PickNewPath();
        }
    }

    /// <summary>
    /// Get a new Random Path from way point Manager
    /// </summary>
    private void PickNewPath()
    {
        _targetPath = WaypointManager.Instance.GenerateRandomPath(transform.position);
        _currentPoint = _targetPath.Dequeue();
    }
    #endregion
}
