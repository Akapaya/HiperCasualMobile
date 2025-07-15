using UnityEngine;

/// <summary>
/// Ragdoll script to active or deactive rigidbodies and colliders.
/// </summary>
public class RagdollActivator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody[] _ragdollBodies;
    [SerializeField] private Collider[] _ragdollColliders;
    [SerializeField] private Collider _mainCollider;
    [SerializeField] private Animator _animator;

    #region Start Methods
    void OnEnable()
    {
        SetRagdollActive(false);
    }
    #endregion

    #region Ragdoll Methods
    [ContextMenu("ActivateRagdoll")]
    public void ActivateRagdoll()
    {
        SetRagdollActive(true);
    }

    [ContextMenu("DeactivateRagdoll")]
    public void DeactivateRagdoll()
    {
        SetRagdollActive(false);
        _animator.Play("Stack");
    }

    void SetRagdollActive(bool active)
    {
        _animator.enabled = !active;

        foreach (var col in _ragdollColliders)
            col.enabled = active;

        foreach (var rb in _ragdollBodies)
            rb.isKinematic = !active;
    }
    #endregion
}
