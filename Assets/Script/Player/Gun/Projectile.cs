using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    private Projectile OriginPrefab;
    public Projectile OriginProjectile => OriginPrefab;
    private ObjectPool objectPool;
    [SerializeField] private float lifeTime = 2;
    private float lifeTimeTimer;

    public void Initialized(Projectile projectile, ObjectPool objectPool)
    {
        OriginPrefab = projectile;
        this.objectPool = objectPool;
        lifeTimeTimer = lifeTime;
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
        lifeTimeTimer -= Time.deltaTime;
        if (lifeTimeTimer <= 0)
        {
            DisableProjectile();
        }
    }

    public void DisableProjectile()
    {
        objectPool.ProjectileDisableRequest.Invoke(this);
    }
}