using UnityEngine;

public struct EnemySpawnRequest
{
    public EnemySpawnRequest(EnemyBase prefab, Vector3 position)
    {
        Prefab = prefab;
        Position = position;
    }

    public EnemyBase Prefab { get; }
    public Vector3 Position { get; }
}
