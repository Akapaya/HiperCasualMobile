using System;
using UnityEngine;

public class EnemyModel : MonoBehaviour, IDamagable, IStackable
{
    [Header("References")]
    [SerializeField] private EnemyDataSO _enemyDataSO;
    [SerializeField] private RagdollActivator _ragdollActivator;

    [Header("Temp Data")]
    [SerializeField] private int _currentHealth;
    [SerializeField] bool _isAlive;
    [SerializeField] bool _onStack;

    [Header("Events")]
    public Action OnDeath;

    public bool IsAlive { get => _isAlive;}
    public bool OnStack { get => _onStack;}

    private void OnEnable()
    {
        OnDeath += ReturnToPool;
        _isAlive = true;
        _onStack = false;
        _currentHealth = _enemyDataSO.MaxHealth;
    }

    private void OnDisable()
    {
        OnDeath -= ReturnToPool;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        _isAlive = false;
        _ragdollActivator.ActivateRagdoll();
        //OnDeath?.Invoke();
    }

    public void ActiveOnStackState()
    {
        _onStack = true;
        _ragdollActivator.DeactivateRagdoll();
    }

    private void ReturnToPool()
    {
        HordeGameManager.Instance.DespawnEnemy(this.gameObject);
    }
}
