using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BlackHoleProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SFXEmitter emitter;
    [SerializeField] private float speed = 25f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float lifeTime = 5f;
    private float lifeTimeTimer;

    [Header("Black Hole Settings")]
    [SerializeField] private LayerMask blackHoleLayerMask;
    [SerializeField] private float blackHoleRadius;
    [SerializeField] private float blackHolePullForce;
    [SerializeField] private float blackHoleDuration;
    [SerializeField] private float blackHoleTickDamage;
    [SerializeField] private float blackHoleTickInterval;
    private float blackHoleTickTimer;
    private float blackHoleDurationTimer;
    private bool hasActivated;

    [HideInInspector] public event Action<BlackHoleProjectile> OnReturn;
    [HideInInspector] public event Action OnBlackHoleTick;

    private BlackHoleProjectile OriginPrefab;
    public BlackHoleProjectile OriginProjectile => OriginPrefab;

    //프로젝타일들도 추상 클래스를 만들어서 관리해야 할 필요가 있음.
    public void Initialized(BlackHoleProjectile projectile)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;
        transform.rotation = Quaternion.identity;

        lifeTimeTimer = lifeTime;
        blackHoleDurationTimer = 0f;
        blackHoleTickTimer = 0f;
        OriginPrefab = projectile;
        hasActivated = false;
    }

    private void Update()
    {
        lifeTimeTimer -= Time.deltaTime;
        if (lifeTimeTimer <= 0)
        {
            hasActivated = true;
        }

        if (hasActivated)
        {
            blackHoleDurationTimer += Time.deltaTime;
            blackHoleTickTimer += Time.deltaTime;
            ActivateBlackHole();

            if (blackHoleDurationTimer >= blackHoleDuration)
            {
                OnReturn?.Invoke(this);
                rb.isKinematic = false;
                return;
            }
        }
        else
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void OnLaunched(Vector3 direction)
    {
        rb.linearVelocity = direction * speed;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void ActivateBlackHole()
    {
        Collider[] affectedObjects = Physics.OverlapSphere(transform.position, blackHoleRadius, blackHoleLayerMask);

        for (int i = 0; i < affectedObjects.Length; i++)
        {
            if (affectedObjects[i].gameObject.TryGetComponent(out IKnockable knockable))
            {
                Vector3 directionToCenter = (transform.position - affectedObjects[i].transform.position).normalized;
                knockable.TakeKnockback(directionToCenter * blackHolePullForce * Time.deltaTime, Time.deltaTime);
            }

            if (blackHoleTickTimer >= blackHoleTickInterval)
            {
                if (affectedObjects[i].TryGetComponent(out IHittable hittable))
                {
                    if (hittable is PlayerManager)
                        continue;

                    hittable.TakeDamage(blackHoleTickDamage);
                    OnBlackHoleTick?.Invoke();
                }

                if (i == affectedObjects.Length - 1)
                {
                    blackHoleTickTimer = 0f;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasActivated)
            return;

        int layer = collision.gameObject.layer;

        if (((1 << layer) & blackHoleLayerMask) != 0)
        {
            hasActivated = true;

            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;

            if (collision.gameObject.TryGetComponent(out IHittable hittable))
            {
                if (hittable is PlayerManager)
                    return;

                hittable.TakeDamage(damage);
            }

            //if (collision.gameObject.TryGetComponent(out EnemyBase enemy))
            //{
            //    enemy.TakeDamage(damage);
            //    OnBlackHoleTick?.Invoke();
            //}
        }
    }
}
