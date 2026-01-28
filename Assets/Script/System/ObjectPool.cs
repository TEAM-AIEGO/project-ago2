using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPool : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<Projectile> projectilePrefabs;

    [Header("Events")]
    public UnityEvent<Projectile, Vector3, Quaternion> ProjectileRequest;
    public UnityEvent<Projectile> ProjectileDisableRequest;

    [Header("Parent Transforms")]
    [SerializeField] private Transform projectileParent;

    private Dictionary<Projectile, Queue<Projectile>> projectilePool = new();

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        foreach (Projectile projectilePrefab in projectilePrefabs)
        {
            projectilePool[projectilePrefab] = new Queue<Projectile>();
        }

        ProjectileRequest.AddListener(SpawnProjectile);
        ProjectileDisableRequest.AddListener(DisableProjectile);
    }

    public void SpawnProjectile(Projectile prefab, Vector3 spawnPos, Quaternion spawnRot)
    {
        if (!projectilePool.ContainsKey(prefab))
            return;

        Projectile projectile;

        if (projectilePool[prefab].Count == 0)
        {
            projectile = Instantiate(prefab, spawnPos, spawnRot, projectileParent);
            //projectile.OriginPrefab = prefab;
        }
        else
        {
            projectile = projectilePool[prefab].Dequeue();
        }

        projectile.Initialized(prefab, this);
        projectile.transform.position = spawnPos;
        projectile.transform.rotation = spawnRot;
        projectile.transform.SetParent(projectileParent);
        projectile.gameObject.SetActive(true);
    }

    public void DisableProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectilePool[projectile.OriginProjectile].Enqueue(projectile);
    }
}