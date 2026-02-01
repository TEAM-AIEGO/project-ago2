using System.Collections.Generic;
using UnityEngine;

public class NegromancySpawnStrategy : INegromancySpawnStrategy
{
    private readonly EnemyBase spawnPrefab;
    private readonly int spawnCount;
    private readonly float spawnRadius;

    public NegromancySpawnStrategy(EnemyBase spawnPrefab, int spawnCount, float spawnRadius)
    {
        this.spawnPrefab = spawnPrefab;
        this.spawnCount = Mathf.Max(0, spawnCount);
        this.spawnRadius = Mathf.Max(0f, spawnRadius);
    }

    public IEnumerable<EnemySpawnRequest> CreateSpawnRequests(NegromancyEnemy enemy)
    {
        if (spawnPrefab == null || enemy == null)
        {
            yield break;
        }

        Vector3 basePosition = enemy.transform.position;
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = basePosition + new Vector3(offset.x, 0f, offset.y);
            yield return new EnemySpawnRequest(spawnPrefab, spawnPosition);
        }
    }
}
