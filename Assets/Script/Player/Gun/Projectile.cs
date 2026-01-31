using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public event Action<Projectile> OnReturn;

    [SerializeField] private float speed = 50f;
    private Projectile OriginPrefab;
    public Projectile OriginProjectile => OriginPrefab;
    [SerializeField] private float lifeTime = 2;
    private float lifeTimeTimer;

    public void Initialized(Projectile projectile)
    {
        lifeTimeTimer = lifeTime;
        OriginPrefab = projectile;
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
        lifeTimeTimer -= Time.deltaTime;
        if (lifeTimeTimer <= 0)
        {
            OnReturn.Invoke(this);
        }
    }
}