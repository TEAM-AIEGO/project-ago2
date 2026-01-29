using UnityEngine;

public interface IAttackStrategy
{
    void Attacking(EnemyBase enemy, Transform target);
}