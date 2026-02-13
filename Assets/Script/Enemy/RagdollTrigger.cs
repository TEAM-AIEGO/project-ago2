using UnityEngine;

public class RagdollTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider col;

    [SerializeField] private Transform ragdollRoot;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;

    private Transform[] boneTrs;
    private Vector3[] defaultLocalPos;
    private Quaternion[] defaultLocalRot;

    private void Awake()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>(true);
        ragdollColliders = GetComponentsInChildren<Collider>(true);

        if (ragdollRoot != null)
            SetDefaultPose();

        SetRagdoll(false);
    }
    
    private void SetDefaultPose()
    {
        boneTrs = ragdollRoot.GetComponentsInChildren<Transform>(true);
        defaultLocalPos = new Vector3[boneTrs.Length];
        defaultLocalRot = new Quaternion[boneTrs.Length];

        for (int i = 0; i < boneTrs.Length; i++)
        {
            defaultLocalPos[i] = boneTrs[i].localPosition;
            defaultLocalRot[i] = boneTrs[i].localRotation;
        }
    }

    public void SetRagdoll(bool enabled)
    {
        animator.enabled = !enabled;
        col.enabled = !enabled;
        rb.isKinematic = enabled;
        rb.freezeRotation = !enabled;

        for (int i = 0; i < ragdollBodies.Length; i++)
        {
            ragdollBodies[i].isKinematic = !enabled;
            //rb.detectCollisions = enabled;
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

    public void OnDespawn()
    {
        SetRagdoll(false);
        RestoreDefaultPose();
    }

    public void OnSpawn()
    {
        RestoreDefaultPose();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void RestoreDefaultPose()
    {
        for (int i = 0; i < boneTrs.Length; i++)
        {
            boneTrs[i].localPosition = defaultLocalPos[i];
            boneTrs[i].localRotation = defaultLocalRot[i];
        }
    }
}
