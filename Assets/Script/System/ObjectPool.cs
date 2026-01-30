using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPool : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<Projectile> projectilePrefabs;
    [SerializeField] private AudioPlayer audioPlayerPrefab;

    [Header("Parent Transforms")]
    [SerializeField] private Transform projectileParent;
    [SerializeField] private Transform audioPlayerParent;

    private Dictionary<Projectile, Queue<Projectile>> projectilePool = new();
    private Queue<AudioPlayer> audioPlayerPool = new();

    private void Awake()
    {
        Debug.Log($"[ObjectPool Awake] {name} id={GetInstanceID()} scene={gameObject.scene.name}");
        Initialize();
    }

    private void OnDestroy()
    {
        Debug.Log($"[ObjectPool Destroy] {name} id={GetInstanceID()} scene={gameObject.scene.name}");
    }

    public void Initialize()
    {
        foreach (Projectile projectilePrefab in projectilePrefabs)
        {
            projectilePool[projectilePrefab] = new Queue<Projectile>();
        }
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