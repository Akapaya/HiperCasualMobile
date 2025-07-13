using System;
using System.Collections;
using UnityEngine;
using static AreaEnum;

public class PlayerModel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerDataSO _playerSettings;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _model;
    [SerializeField] private Rigidbody _rigidbody;

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
        OnCoinsChanged += UIManager.Instance.ChangeTMPCoinText;
    }
    #endregion

    #region Movement Feature
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

    public void RotateCharacter(Vector3 moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        _model.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _playerSettings.RotationSpeed * Time.deltaTime);
    }
    #endregion

    #region Push Feature
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
}
