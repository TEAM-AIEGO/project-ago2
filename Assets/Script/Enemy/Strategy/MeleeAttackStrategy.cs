using UnityEngine;

public class MeleeAttackStrategy : IMUDAMUDAMUDAStrategy
{
    private Vector2 meleeAttackRange;
    private Transform meleeAttackPoint;

    public MeleeAttackStrategy(Vector3 meleeAttackRange, Transform meleeAttackPoint)
    {
        this.meleeAttackRange = meleeAttackRange;
        this.meleeAttackPoint = meleeAttackPoint;
    }

    public void Attacking(EnemyBase enemy, Transform target)
    {
        Collider[] hitPlayers = Physics.OverlapBox(meleeAttackPoint.position, meleeAttackRange / 2, Quaternion.identity, LayerMask.GetMask("Player"));

        for (int i = 0; i < hitPlayers.Length; i++)
        {
            if (hitPlayers[i].TryGetComponent(out PlayerManager player))
            {
                player?.TakeDamage(enemy.AttackDamage);
            }
        }
    }
}