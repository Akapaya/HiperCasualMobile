using System;
using UnityEngine;

/// <summary>
/// Enemy logic and stats part, controls damage, stack and ragdoll logic.
/// </summary>
public class EnemyModel : MonoBehaviour, IDamagable, IStackable, ISellItem
{
    [Header("References")]
    [SerializeField] private EnemyDataSO _enemyDataSO;
    [SerializeField] private RagdollActivator _ragdollActivator;

    [Header("Temp Data")]
    [SerializeField] private int _currentHealth;
    [SerializeField] private bool _isAlive;
    [SerializeField] private bool _onStack;

    [Header("Events")]
    public Action OnDeath;

    public bool IsAlive { get => _isAlive;}
    public bool OnStack { get => _onStack;}

    public int ValueItem => _enemyDataSO.AmountDropCoins;

    public string StackFamily => _enemyDataSO.EnemyID;

    #region Start Methods
    private void OnEnable()
    {
        _isAlive = true;
        _onStack = false;
        _currentHealth = _enemyDataSO.MaxHealth;
    }
    #endregion

    #region IDamagable Methods
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
    #endregion

    #region IStackable Methods
    public void ActiveOnStackState()
    {
        _onStack = true;
        _ragdollActivator.DeactivateRagdoll();
    }
    #endregion
}
