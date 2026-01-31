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
    [SerializeField] private AudioPlayer audioPlayerPrefab;
    [SerializeField] private List<EnemyBase> enemyBasePrefabs;

    [Header("Parent Transforms")]
    [SerializeField] private Transform projectileParent;
    [SerializeField] private Transform audioPlayerParent;
    [SerializeField] private Transform enemyParent;

    private readonly Dictionary<Projectile, Queue<Projectile>> projectilePool = new();
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

        enemy.Initialize(prefab);
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

        projectile.Initialized(prefab);
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

    public AudioPlayer SpawnAudioPlayer()
    {
        AudioPlayer audioPlayer;

        if (audioPlayerPool.Count == 0)
        {
            audioPlayer = Instantiate(audioPlayerPrefab, transform);
        }
        else
        {
            audioPlayer = audioPlayerPool.Dequeue();
        }

        audioPlayer.gameObject.SetActive(true);
        audioPlayer.Finished += DisableAudioPlayer;
        return audioPlayer;
    }

    public void DisableAudioPlayer(AudioPlayer audioPlayer)
    {
        audioPlayer.gameObject.SetActive(false);
        audioPlayer.transform.SetParent(audioPlayerParent);
        audioPlayerPool.Enqueue(audioPlayer);
    }
}