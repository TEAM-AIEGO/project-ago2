using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [HideInInspector] public event Action<GrenadeProjectile> OnReturn;
    [HideInInspector] public event Action OnExplosionHit;

    [SerializeField] private float speed = 25f;

    private GrenadeProjectile OriginPrefab;
    public GrenadeProjectile OriginProjectile => OriginPrefab;

    [SerializeField] private float lifeTime = 5f;
    private float lifeTimeTimer;

    [Header("Explosion Settings")]
    [SerializeField] private LayerMask explosionLayerMask;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionDamageReductionDistance;
    [SerializeField] private float explosionDamage;

    public void Initialized(GrenadeProjectile projectile)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;

        lifeTimeTimer = lifeTime;
        OriginPrefab = projectile;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);

        lifeTimeTimer -= Time.deltaTime;
        if (lifeTimeTimer <= 0)
        {
            OnExplosion();
        }
    }

    public void OnLaunched (Vector3 direction)
    {
        rb.linearVelocity = direction * speed;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnExplosion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayerMask);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
            float damageMultiplier = 1f;

            if (distance > explosionDamageReductionDistance)
            {
                damageMultiplier = Mathf.Clamp01(1 - (distance - explosionDamageReductionDistance) / (explosionRadius - explosionDamageReductionDistance));
            }

            EnemyBase enemy = hitColliders[i].GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage * damageMultiplier);
                OnExplosionHit?.Invoke();
            }

            //IDamageable damageable = hitColliders[i].GetComponent<IDamageable>();

            //if (damageable != null)
            //{
            //    damageable.TakeDamage(explosionDamage * damageMultiplier);
            //}
        }

        OnReturn.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        int layer = collision.gameObject.layer;

        if (((1 << layer) & explosionLayerMask) != 0)
        {
            OnExplosion();
        }
    }
}
