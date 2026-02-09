using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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
    private bool hasExploded;

    //프로젝타일들도 추상 클래스를 만들어서 관리해야 할 필요가 있음.
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

    public void OnLaunched (Vector3 direction)
    {
        rb.linearVelocity = direction * speed;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void OnExplosion()
    {
        if (hasExploded)
        {
            return;
        }

        List<GameObject> duplicationObjs = new();

        hasExploded = true;
        emitter.Play("Grenade_Explosion");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayerMask);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            

            float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
            float damageMultiplier = 1f;

            if (distance > explosionDamageReductionDistance)
            {
                damageMultiplier = Mathf.Clamp01(1 - (distance - explosionDamageReductionDistance) / (explosionRadius - explosionDamageReductionDistance));
            }

            if (hitColliders[i].TryGetComponent(out IKnockable knockable))
            {
                knockable.TakeExplosionKnockback(15f, transform.position, explosionRadius, 0.5f);
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

                    float finalDamage = (hittable is PlayerManager) ? (explosionDamage / 4) * damageMultiplier : explosionDamage * damageMultiplier;

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
