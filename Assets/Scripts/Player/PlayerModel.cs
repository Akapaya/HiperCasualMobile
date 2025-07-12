using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerModel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerDataSO _playerSettings;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _model;

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

    public void MoveCharacter(Vector3 moveDirection)
    {
        float inputMagnitude = moveDirection.magnitude;

        float normalizedSpeed = Mathf.Clamp01(inputMagnitude);
        _animator.SetFloat(_animatorSpeedParamter, normalizedSpeed);

        if (inputMagnitude > 0.01f)
        {
            Vector3 move = moveDirection.normalized * _playerSettings.MoveSpeed * Time.deltaTime;
            RotateCharacter(move);
            transform.position += move;
        }
    }

    public void RotateCharacter(Vector3 moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        _model.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _playerSettings.RotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isPush) return;

        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            if (damagable.IsAlive)
            {
                _animator.SetLayerWeight(_animator.GetLayerIndex(_animatorUpperLayer), 1);
                _animator.Play(_animatorPushAnimation, _animator.GetLayerIndex(_animatorUpperLayer), 0f);
                StartCoroutine(PunchRoutine(damagable, other));
            }
        }
    }

    private IEnumerator PunchRoutine(IDamagable damagable, Collider body)
    {
        _isPush = true;

        damagable.TakeDamage(_playerSettings.Strenght);

        yield return new WaitForSeconds(_pushDuration);

        if(!damagable.IsAlive)
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
}
