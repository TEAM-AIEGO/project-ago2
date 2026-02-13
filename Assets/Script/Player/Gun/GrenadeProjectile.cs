using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SFXEmitter emitter;

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

    [Header("Rail Explosion Settings")]
    [SerializeField] private float railExplosionRadius;
    [SerializeField] private float railExplosionDamageReductionDistance;
    [SerializeField] private float railExplosionDamage;
    private bool hasExploded;

    public void Initialized(GrenadeProjectile projectile)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;

        lifeTimeTimer = lifeTime;
        OriginPrefab = projectile;
        hasExploded = false;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);

        //rb.linearVelocity = transform.forward * speed;

        lifeTimeTimer -= Time.deltaTime;
        if (lifeTimeTimer <= 0)
        {
            OnExplosion();
        }
    }

    public void OnLaunched(Vector3 direction)
    {
        rb.linearVelocity = direction * speed;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void OnExplosion(bool railgun = false)
    {
        if (hasExploded)
        {
            return;
        }

        float finalExplosionDamage = railgun ? railExplosionDamage : explosionDamage;
        float finalExplosionRadius = railgun ? railExplosionRadius : explosionRadius ;
        float finalExplosionDamageReductionDistance = railgun ? railExplosionDamageReductionDistance : explosionDamageReductionDistance;

        List<GameObject> duplicationObjs = new();

        hasExploded = true;
        emitter.Play(AudioIds.GrenadeExplosion);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayerMask);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            

            float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
            float damageMultiplier = 1f;

            if (distance > explosionDamageReductionDistance)
            {
                damageMultiplier = Mathf.Clamp01(1 - (distance - finalExplosionDamageReductionDistance) / (finalExplosionRadius - finalExplosionDamageReductionDistance));
            }

            if (hitColliders[i].TryGetComponent(out IKnockable knockable))
            {
                knockable.TakeExplosionKnockback(railgun ? 25f : 15f, transform.position, finalExplosionRadius, 0.5f);
            }

            //if (hitColliders[i].TryGetComponent(out IHittable hittable))
            //{
            //    if (hittable is PlayerManager)
            //    {
            //        hittable.TakeDamage((explosionDamage / 4) * damageMultiplier);
            //        OnExplosionHit?.Invoke();

            //        continue;
            //    }

            //    hittable.TakeDamage(explosionDamage * damageMultiplier);
            //    OnExplosionHit?.Invoke();
            //}

            Transform t = hitColliders[i].transform;

            for (int j = 0; j <= 2 && t != null; j++)
            {
                if (t.TryGetComponent(out IHittable hittable))
                {
                    if (duplicationObjs.Contains(t.gameObject))
                        break;

                    float finalDamage = (hittable is PlayerManager) ? (explosionDamage / 4) * damageMultiplier : finalExplosionDamage * damageMultiplier;

                    hittable.TakeDamage(finalDamage);
                    OnExplosionHit?.Invoke();
                    duplicationObjs.Add(t.gameObject);
                    break;
                }

                t = t.parent;
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
