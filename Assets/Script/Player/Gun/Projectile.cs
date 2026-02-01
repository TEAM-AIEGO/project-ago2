using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public event Action<Projectile> OnReturn;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 50f;

    private Projectile OriginPrefab;
    public Projectile OriginProjectile => OriginPrefab;

    [SerializeField] private float lifeTime = 2;
    private float lifeTimeTimer;

    [SerializeField] private LayerMask contactableLayerMask;

    private float damage;

    public void Initialized(Projectile projectile, float damage, float speed)
    {
        lifeTimeTimer = lifeTime;
        OriginPrefab = projectile;
        this.damage = damage;
        this.speed = speed;
    }

    void Update()
    {
        rb.linearVelocity = transform.forward * speed;

        lifeTimeTimer -= Time.deltaTime;
        if (lifeTimeTimer <= 0)
        {
            OnReturn.Invoke(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;

        if (((1 << layer) & contactableLayerMask) != 0)
        {
            //Debug.Log("Projectile hit: " + other.gameObject.name);

            if (damage != 0f)
            {
                if (other.gameObject.TryGetComponent(out PlayerManager player))
                {
                    player.TakeDamage(damage);
                    OnReturn.Invoke(this);
                }
            }
            else
            {
                //Debug.Log("Projectile returned without dealing damage.");
                OnReturn.Invoke(this);
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    int layer = collision.gameObject.layer;

    //    if (((1 << layer) & contactableLayerMask) != 0)
    //    {
    //        Debug.Log("Projectile hit: " + collision.gameObject.name);

    //        if (damage != 0f)
    //        {
    //            if (collision.gameObject.TryGetComponent(out PlayerManager player))
    //            {
    //                player.TakeDamage(damage);
    //                OnReturn.Invoke(this);
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("Projectile returned without dealing damage.");
    //            OnReturn.Invoke(this);
    //        }
    //    }
    //}
}