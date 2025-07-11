using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    [Header("References")]
    public Rigidbody[] ragdollBodies;
    public Collider[] ragdollColliders;
    public Collider mainCollider;
    public Animator animator;

    #region Start Methods
    void Start()
    {
        SetRagdollActive(false);
    }
    #endregion

    [ContextMenu("ActivateRagdoll")]
    public void ActivateRagdoll()
    {
        SetRagdollActive(true);
    }

    void SetRagdollActive(bool active)
    {
        animator.enabled = !active;
        mainCollider.enabled = !active;

        foreach (var col in ragdollColliders)
            col.enabled = active;

        foreach (var rb in ragdollBodies)
            rb.isKinematic = !active;
    }
}
