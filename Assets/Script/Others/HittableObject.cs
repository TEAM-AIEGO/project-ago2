using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HittableObject : MonoBehaviour, IKnockable
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void TakeKnockback(Vector3 force, float duration)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    public void TakeExplosionKnockback(float explosionForce, Vector3 explosionPosition, float explosionRadius, float duration)
    {
        rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, 2f, ForceMode.VelocityChange);
    }
}
