using UnityEngine;

public class RagdollTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;

    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;

    private void Awake()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>(true);
        ragdollColliders = GetComponentsInChildren<Collider>(true);

        SetRagdoll(false);
    }

    public void SetRagdoll(bool enabled)
    {
        animator.enabled = !enabled;

        rb.isKinematic = enabled;

        for (int i = 0; i < ragdollBodies.Length; i++)
        {
            ragdollBodies[i].isKinematic = !enabled;
            rb.detectCollisions = enabled;
        }

        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = enabled;
        }
    }

    public void RagdollKnockback(Vector3 force)
    {
        ragdollBodies[0].AddForce(force * 10, ForceMode.VelocityChange);
        //for (int i = 0;i < ragdollBodies.Length; i++)
        //{
        //    ragdollBodies[i].AddForce(force, ForceMode.VelocityChange);
        //}
    }

    public void RagdollExplosionKnockback(float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        ragdollBodies[0].AddExplosionForce(explosionForce * 20, explosionPosition, explosionRadius, 2f, ForceMode.VelocityChange);
        //for(int i = 0; i < ragdollBodies.Length; i++)
        //{
        //    ragdollBodies[i].AddExplosionForce(explosionForce, explosionPosition, explosionRadius, 2f, ForceMode.VelocityChange);
        //}
    }
}
