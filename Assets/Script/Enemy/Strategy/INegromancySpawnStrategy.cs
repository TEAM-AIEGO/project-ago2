using System.Collections.Generic;

public interface INegromancySpawnStrategy
{
    IEnumerable<EnemySpawnRequest> CreateSpawnRequests(NegromancyEnemy enemy);
}
