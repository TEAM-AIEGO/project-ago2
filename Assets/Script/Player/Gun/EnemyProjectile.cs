using System;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [HideInInspector] public event Action<EnemyProjectile> OnReturn;
    [SerializeField] private float speed = 25f;

    private EnemyProjectile originProjectile;
    public EnemyProjectile OriginProjectile => originProjectile;

    [SerializeField] private float lifeTime = 3f;
    private float lifeTimeTimer;

    [SerializeField] private LayerMask contactableLayerMask;

    private float damage;

    public void Initialize(EnemyProjectile projectile, float damage)
    {
        lifeTimeTimer = lifeTime;
        originProjectile = projectile;
        this.damage = damage;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);

        lifeTimeTimer -= Time.deltaTime;
        if (lifeTimeTimer <= 0)
        {
            OnReturn.Invoke(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        int layer = collision.gameObject.layer;

        if (((1 << layer) & contactableLayerMask) != 0)
        {
            if (collision.gameObject.TryGetComponent(out PlayerManager player))
            {
                player.TakeDamage(damage);
                OnReturn.Invoke(this);
            }
            else
            {
                OnReturn.Invoke(this);
            }
        }
    }
}
