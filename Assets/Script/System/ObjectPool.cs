using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private WarpSystemManager warpSystemManager;

    [Header("Prefabs")]
    [SerializeField] private List<Projectile> projectilePrefabs;
    [SerializeField] private GrenadeProjectile grenadeProjectilePrefab;
    [SerializeField] private EnemyGrenadeProjectile enemyGrenadeProjectilePrefab;
    [SerializeField] private AudioPlayer audioPlayerPrefab;
    [SerializeField] private List<EnemyBase> enemyBasePrefabs;

    [Header("Parent Transforms")]
    [SerializeField] private Transform projectileParent;
    [SerializeField] private Transform grenadeProjectileParent;
    [SerializeField] private Transform audioPlayerParent;
    [SerializeField] private Transform enemyParent;

    private readonly Dictionary<Projectile, Queue<Projectile>> projectilePool = new();
    private readonly Queue<GrenadeProjectile> grenadeProjectilePool = new();
    private readonly Queue<EnemyGrenadeProjectile> enemyGrenadeProjectilePool = new();
    private readonly Queue<AudioPlayer> audioPlayerPool = new();
    private readonly Dictionary<EnemyBase, Queue<EnemyBase>> enemyBasePool = new();

    private void Awake()
    {
        //Debug.Log($"[ObjectPool Awake] {name} id={GetInstanceID()} scene={gameObject.scene.name}");
        Initialize();
    }

    private void OnDestroy()
    {
        //Debug.Log($"[ObjectPool Destroy] {name} id={GetInstanceID()} scene={gameObject.scene.name}");
    }

    public void Initialize()
    {
        for (int i = 0; i < projectilePrefabs.Count; i++)
        {
            projectilePool[projectilePrefabs[i]] = new Queue<Projectile>();
        }

        for (int i = 0; i < enemyBasePrefabs.Count; i++)
        {
            enemyBasePool[enemyBasePrefabs[i]] = new Queue<EnemyBase>();
        }
    }

    public EnemyBase SpawnEnemy(EnemyBase prefab, Vector3 spawnPos)
    {
        if (!enemyBasePool.ContainsKey(prefab))
            return null;

        EnemyBase enemy;

        if (enemyBasePool[prefab].Count == 0)
        {
            enemy = Instantiate(prefab, enemyParent);
        }
        else
        {
            enemy = enemyBasePool[prefab].Dequeue();
        }

        if (enemy is RangedEnemy rangedEnemy)
            rangedEnemy.OnShootProjectile += SpawnProjectile;

        if (enemy is BombEnemy bombEnemy)
            bombEnemy.OnLaunchProjectile += SpawnEnemyGrenadeProjectile;

        //Debug.Log($"Spawning Enemy: {enemy.name} at {spawnPos}");
        enemy.Initialize(prefab, warpSystemManager.GetWarpStage());
        enemy.transform.position = spawnPos;
        enemy.transform.rotation = Quaternion.identity;
        enemy.transform.SetParent(enemyParent);
        enemy.gameObject.SetActive(true);
        warpSystemManager.RegisterWarpObserver(enemy);

        return enemy;
    }

    public void DisableEnemy(EnemyBase enemy)
    {
        warpSystemManager.UnregisterWarpObserver(enemy);
        enemy.gameObject.SetActive(false);
        enemyBasePool[enemy.OriginEnemy].Enqueue(enemy);
    }

    public void SpawnProjectile(Projectile prefab, Vector3 spawnPos, Quaternion spawnRot, float speed = 0f ,float damage = 0f)
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

        projectile.Initialized(prefab, damage, speed);
        projectile.transform.position = spawnPos;
        projectile.transform.rotation = spawnRot;
        projectile.transform.SetParent(projectileParent);
        projectile.OnReturn += DisableProjectile;
        projectile.gameObject.SetActive(true);
    }

    public void DisableProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectilePool[projectile.OriginProjectile].Enqueue(projectile);
    }

    public GrenadeProjectile SpawnGrenadeProjectile(Vector3 spawnPos)
    {
        GrenadeProjectile grenadeProjectile;

        if (grenadeProjectilePool.Count == 0)
        {
            grenadeProjectile = Instantiate(grenadeProjectilePrefab, spawnPos, Quaternion.identity, grenadeProjectileParent);
        }
        else
        {
            grenadeProjectile = grenadeProjectilePool.Dequeue();
        }

        grenadeProjectile.Initialized(grenadeProjectilePrefab);
        grenadeProjectile.transform.position = spawnPos;
        grenadeProjectile.transform.rotation = Quaternion.identity;
        //grenadeProjectile.transform.SetParent(grenadeProjectileParent);
        grenadeProjectile.OnReturn += DisableGrenadeProjectile;
        grenadeProjectile.gameObject.SetActive(true);
        return grenadeProjectile;
    }

    public void DisableGrenadeProjectile(GrenadeProjectile grenadeProjectile)
    {
        grenadeProjectile.gameObject.SetActive(false);
        grenadeProjectilePool.Enqueue(grenadeProjectile);
    }

    public void SpawnEnemyGrenadeProjectile(Vector3 spawnPos, Vector3 direction, float damage)
    {
        EnemyGrenadeProjectile enemyGrenadeProjectile;

        if (enemyGrenadeProjectilePool.Count == 0)
        {
            enemyGrenadeProjectile = Instantiate(enemyGrenadeProjectilePrefab, spawnPos, Quaternion.identity, grenadeProjectileParent);
        }
        else
        {
            enemyGrenadeProjectile = enemyGrenadeProjectilePool.Dequeue();
        }

        enemyGrenadeProjectile.Initialized(enemyGrenadeProjectilePrefab, damage);
        enemyGrenadeProjectile.transform.position = spawnPos;
        enemyGrenadeProjectile.transform.rotation = Quaternion.identity;
        //enemyGrenadeProjectile.transform.SetParent(grenadeProjectileParent);
        enemyGrenadeProjectile.OnReturn += DisableEnemyGrenadeProjectile;
        enemyGrenadeProjectile.gameObject.SetActive(true);
        enemyGrenadeProjectile.OnLaunched(direction);
    }

    public void DisableEnemyGrenadeProjectile(EnemyGrenadeProjectile enemyGrenadeProjectile)
    {
        enemyGrenadeProjectile.gameObject.SetActive(false);
        enemyGrenadeProjectilePool.Enqueue(enemyGrenadeProjectile.OriginProjectile);
    }

    public AudioPlayer SpawnAudioPlayer()
    {
        AudioPlayer audioPlayer;

        if (audioPlayerPool.Count == 0)
        {
            //Debug.Log("Instantiate AudioPlayer : " + audioPlayerPool.Count);
            audioPlayer = Instantiate(audioPlayerPrefab);
        }
        else
        {
            //Debug.Log("Reuse AudioPlayer : " + audioPlayerPool.Count);
            audioPlayer = audioPlayerPool.Dequeue();
        }

        audioPlayer.Initialize(audioPlayer);
        audioPlayer.Finished += DisableAudioPlayer;
        audioPlayer.gameObject.SetActive(true);
        return audioPlayer;
    }

    public void DisableAudioPlayer(AudioPlayer audioPlayer)
    {
        audioPlayer.gameObject.SetActive(false);
        audioPlayer.transform.SetParent(audioPlayerParent);

        if (!audioPlayerPool.Contains(audioPlayer.OriginPlayerPrefab))
            audioPlayerPool.Enqueue(audioPlayer.OriginPlayerPrefab);
    }
}