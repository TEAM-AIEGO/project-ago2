using System.Collections.Generic;
using UnityEngine;

public interface IORAORAORAStrategy
{
    IEnumerable<EnemySpawnRequest> CreateSpawnRequests(NegromancyEnemy enemy);
}
