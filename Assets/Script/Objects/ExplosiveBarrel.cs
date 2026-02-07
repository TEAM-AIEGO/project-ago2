using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExplosiveBarrel : BreakableObject
{
    [Header("Explosion")]
    [SerializeField] private LayerMask explosionLayerMask;
    [SerializeField] private float explosionRadius = 20f;
    [SerializeField] private float explosionDamage = 70f;
    [SerializeField] private float explosionForce = 15f;
    [SerializeField] private float knockbackDuration = 0.5f;

    private bool hasExploded;

    public override void TakeDamage(float damage)
    {
        if (hasExploded)
        {
            return;
        }

        base.TakeDamage(damage);

        if (health <= 0f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded)
        {
            return;
        }

        hasExploded = true;
        Vector3 explosionPosition = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(explosionPosition, explosionRadius, explosionLayerMask);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].TryGetComponent(out IHittable hittable))
            {
                hittable.TakeDamage(explosionDamage);
            }

            if (hitColliders[i].TryGetComponent(out IKnockable knockable))
            {
                knockable.TakeExplosionKnockback(explosionForce, explosionPosition, explosionRadius, knockbackDuration);
            }
        }
    }
}
