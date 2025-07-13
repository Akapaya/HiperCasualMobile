using System;
using UnityEngine;

public class EnemyModel : MonoBehaviour, IDamagable, IStackable, ISellItem
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

    public int ValueItem => _enemyDataSO.AmountDropCoins;

    public string StackFamily => _enemyDataSO.EnemyID;

    private void OnEnable()
    {
        _isAlive = true;
        _onStack = false;
        _currentHealth = _enemyDataSO.MaxHealth;
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
    }

    public void ActiveOnStackState()
    {
        _onStack = true;
        _ragdollActivator.DeactivateRagdoll();
    }
}
