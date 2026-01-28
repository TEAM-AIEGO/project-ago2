using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<Projectile> projectilePrefabs;

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
    }

    public Projectile SpawnProjectile(Projectile prefab, Vector3 spawnPos, Quaternion spawnRot)
    {
        if (!projectilePool.ContainsKey(prefab))
            return null;

        Projectile projectile;

        if (projectilePool[prefab].Count == 0)
        {
            projectile = Instantiate(prefab, spawnPos, spawnRot, projectileParent);
            projectile.OriginPrefab = prefab;
        }
        else
        {
            projectile = projectilePool[prefab].Dequeue();
        }

        projectile.transform.position = spawnPos;
        projectile.transform.rotation = spawnRot;
        projectile.transform.SetParent(projectileParent);
        projectile.gameObject.SetActive(true);

        return projectile;
    }

    public void DisableProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectilePool[projectile.OriginPrefab].Enqueue(projectile);
    }
}