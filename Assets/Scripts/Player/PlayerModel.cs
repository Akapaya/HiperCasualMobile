using System;
using System.Collections;
using UnityEngine;
using static AreaEnum;
/// <summary>
/// Enemy logic and stats part, controls stats, movement, animation...
/// </summary>
public class PlayerModel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerDataSO _playerSettings;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _model;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Material _material;

    [Header("TempData")]
    [SerializeField] private float _moveDisplacement = 0;
    [SerializeField] private bool _isPush = false;

    [Header("Settings")]
    [SerializeField] private string _animatorSpeedParamter = "Velocity";
    [SerializeField] private string _animatorPushAnimation = "Push";
    [SerializeField] private string _animatorUpperLayer = "UpperBody";
    [SerializeField] private string _targetTags = "Enemy";
    [SerializeField] private float _pushDuration = 2f;

    [Header("Events")]
    public Action<Transform> OnEnemyEliminated;
    public Action<int> OnCoinsChanged;

    #region Start Methods
    private void Start()
    {
        AreaManager.Instance.RegisterObserver(AreasTypes.SellArea, (int coins) => IncreaseCoins(coins));
        UpgradeManager.Instance.OnPurchasedUpgrade += HandledUpgrade;
        OnCoinsChanged += UIManager.Instance.ChangeTMPCoinText;
    }

    private void OnDisable()
    {
        UpgradeManager.Instance.OnPurchasedUpgrade -= HandledUpgrade;
    }
    #endregion

    #region Movement Feature
    /// <summary>
    /// Moves Player using rigid body to avoid pass though walls
    /// </summary>
    /// <param name="moveDirection">Vector with direction move</param>
    public void MoveCharacter(Vector3 moveDirection)
    {
        float inputMagnitude = moveDirection.magnitude;

        float normalizedSpeed = Mathf.Clamp01(inputMagnitude);
        _animator.SetFloat(_animatorSpeedParamter, normalizedSpeed);

        if (inputMagnitude > 0.01f)
        {
            Vector3 move = moveDirection.normalized * _playerSettings.MoveSpeed * Time.deltaTime;
            RotateCharacter(move);
            _rigidbody.MovePosition(_rigidbody.position + move);
        }
    }

    /// <summary>
    /// Rotate player to direction of movement
    /// </summary>
    /// <param name="moveDirection">Vector with direction move</param>
    public void RotateCharacter(Vector3 moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        _model.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _playerSettings.RotationSpeed * Time.deltaTime);
    }
    #endregion

    #region Push Feature
    /// <summary>
    /// Routine to push enemies, dealing damage and handle if enemy eliminated
    /// </summary>
    /// <param name="damagable">Script of IDamagable</param>
    /// <param name="body">Collider of objects punched</param>
    /// <returns></returns>
    private IEnumerator PunchRoutine(IDamagable damagable, Collider body)
    {
        _isPush = true;

        damagable.TakeDamage(_playerSettings.Strenght);

        yield return new WaitForSeconds(_pushDuration);

        if (!damagable.IsAlive)
        {
            OnEnemyEliminated?.Invoke(body.gameObject.transform);
        }

        StopPush();
    }

    /// <summary>
    /// Stop animation and state of Push
    /// </summary>
    public void StopPush()
    {
        _isPush = false;
        _animator.SetLayerWeight(_animator.GetLayerIndex(_animatorUpperLayer), 0);
    }
    #endregion

    #region Sell Feature
    public void IncreaseCoins(int coins)
    {
        _playerSettings.Coins += coins;
        OnCoinsChanged?.Invoke(_playerSettings.Coins);
    }

    public void DecreaseCoins(int coins)
    {
        _playerSettings.Coins -= coins;
        OnCoinsChanged?.Invoke(_playerSettings.Coins);
    }
    #endregion

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            if (_isPush) return;
            if (damagable.IsAlive)
            {
                _animator.SetLayerWeight(_animator.GetLayerIndex(_animatorUpperLayer), 1);
                _animator.Play(_animatorPushAnimation, _animator.GetLayerIndex(_animatorUpperLayer), 0f);
                StartCoroutine(PunchRoutine(damagable, other));
            }
            else
            {
                OnEnemyEliminated?.Invoke(other.gameObject.transform);
            }
            return;
        }

        if(other.gameObject.TryGetComponent(out IArea area))
        {
            area.ActivateArea();
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IArea area))
        {
            area.DeactivateArea();
            return;
        }
    }
    #endregion

    #region Upgrades
    /// <summary>
    /// Handle to increase stats of player depends of UpgradeSO
    /// </summary>
    /// <param name="upgrade">Upgrade SO Purchased</param>
    public void HandledUpgrade(UpgradeSO upgrade)
    {
        if( _playerSettings.Coins > upgrade.Cost )
        {
            _playerSettings.Coins -= upgrade.Cost;
            OnCoinsChanged?.Invoke(_playerSettings.Coins);
            _playerSettings.MaxStack += upgrade.Stack;
            _playerSettings.Strenght += upgrade.Strenght;
            _playerSettings.MoveSpeed += upgrade.Speed;
            _material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        }
        else
        {
            UIManager.Instance.AdviceNotEnoughCoins();
        }
    }
    #endregion
}
