using System.Collections;
using Unity.Collections;
using UnityEngine;

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

        if (other.CompareTag(_targetTags))
        {
            _animator.SetLayerWeight(_animator.GetLayerIndex(_animatorUpperLayer), 1);
            StartCoroutine(PunchRoutine(other.gameObject));
        }
    }

    private IEnumerator PunchRoutine(GameObject enemy)
    {
        _isPush = true;

        enemy.GetComponent<RagdollActivator>().ActivateRagdoll();

        yield return new WaitForSeconds(_pushDuration);

        StopPush();
    }

    public void StopPush()
    {
        _isPush = false;
        _animator.Play(_animatorPushAnimation, _animator.GetLayerIndex(_animatorUpperLayer));
        _animator.SetLayerWeight(_animator.GetLayerIndex(_animatorUpperLayer), 0);
    }
}
