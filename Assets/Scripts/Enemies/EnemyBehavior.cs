using System;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IPoolable, IUpdater
{
    [Header("References")]
    [SerializeField] private EnemyDataSO _enemyDataSO;
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyModel _enemyModel;

    [Header("Temp Data")]
    [SerializeField] private Vector3 targetPoint;
    [SerializeField] private float stopTimer = 0f;
    [SerializeField] private bool isWaiting = false;

    [Header("Settings")]
    [SerializeField] private string _animatorSpeedParamter = "Velocity";

    #region Start Methods
    private void OnEnable()
    {
        UpdaterManager.Instance.AddIUpdaterInList(this);
    }

    public void Start()
    {
        PickNewTargetPoint();
        stopTimer = _enemyDataSO.StopTime;
        isWaiting = true;
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

        if (isWaiting)
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0f)
            {
                isWaiting = false;
            }
            return;
        }

        float distance = Vector3.Distance(transform.position, targetPoint);

        if (distance > _enemyDataSO.PointReachDistance)
        {
            Vector3 direction = (targetPoint - transform.position).normalized;
            float inputMagnitude = direction.magnitude;

            float normalizedSpeed = Mathf.Clamp01(inputMagnitude);
            _animator.SetFloat(_animatorSpeedParamter, normalizedSpeed);
            transform.position += direction * _enemyDataSO.MoveSpeed * Time.deltaTime;
            transform.forward = direction;
        }
        else
        {
            _animator.SetFloat(_animatorSpeedParamter, 0);
            stopTimer = _enemyDataSO.StopTime;
            isWaiting = true;
            PickNewTargetPoint();
        }
    }
    #endregion

    #region Enemy Methods
    private void PickNewTargetPoint()
    {
        float x = UnityEngine.Random.Range(_enemyDataSO.PatrolAreaMin.x, _enemyDataSO.PatrolAreaMax.x);
        float z = UnityEngine.Random.Range(_enemyDataSO.PatrolAreaMin.y, _enemyDataSO.PatrolAreaMax.y);
        targetPoint = new Vector3(x, transform.position.y, z);
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
