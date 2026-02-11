using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyGrenadeProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SFXEmitter emitter;
    [SerializeField] private float lifeTime = 5f;
    private float lifeTimeTimer;

    [Header("Explosion Settings")]
    [SerializeField] private LayerMask explosionLayerMask;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionDamageReductionDistance;
    [SerializeField] private float explosionDamage;
    private bool hasExploded;

    [HideInInspector] public event Action<EnemyGrenadeProjectile> OnReturn;

    private EnemyGrenadeProjectile OriginPrefab;
    public EnemyGrenadeProjectile OriginProjectile => OriginPrefab;

    //프로젝타일들도 추상 클래스를 만들어서 관리해야 할 필요가 있음.
    public void Initialized(EnemyGrenadeProjectile projectile, float damage)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;

        lifeTimeTimer = lifeTime;
        OriginPrefab = projectile;
        explosionDamage = damage;
        hasExploded = false;
    }

    private void Update()
    {
        lifeTimeTimer -= Time.deltaTime;
        if (lifeTimeTimer <= 0)
        {
            OnExplosion();
        }
    }

    public void OnLaunched(Vector3 direction)
    {
        rb.linearVelocity = direction;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void OnExplosion()
    {
        if (hasExploded)
        {
            return;
        }

        hasExploded = true;
        emitter.Play(AudioIds.RobotBombRobotCrashAttack);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayerMask);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
            float damageMultiplier = 1f;

            if (distance > explosionDamageReductionDistance)
            {
                damageMultiplier = Mathf.Clamp01(1f - (distance - explosionDamageReductionDistance) / (explosionRadius - explosionDamageReductionDistance));
            }

            if (hitColliders[i].TryGetComponent(out IKnockable knockable))
            {
                knockable.TakeExplosionKnockback(15f, transform.position, explosionRadius, 0.5f);
            }

            if (hitColliders[i].TryGetComponent(out PlayerManager player))
            {
                player.TakeDamage(explosionDamage * damageMultiplier);
            }
        }

        OnReturn?.Invoke(this);
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
