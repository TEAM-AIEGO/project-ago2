using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    private Projectile OriginPrefab;
    public Projectile OriginProjectile => OriginPrefab;
    private ObjectPool objectPool;

    void Start()
    {
        
    }

    public void Initialized(Projectile projectile, ObjectPool objectPool)
    {
        OriginPrefab = projectile;
        this.objectPool = objectPool;
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    public void DisableProjectile()
    {
        objectPool.ProjectileDisableRequest.Invoke(this);
    }
}