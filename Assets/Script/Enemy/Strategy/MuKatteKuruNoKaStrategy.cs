using UnityEngine;

public class MuKatteKuruNoKaStrategy : IMuKatteKuruNoKaStrategy
{
    public void KonoDIOniMuKatteKuruNoKa(EnemyBase enemy, Transform target)
    {
        Vector3 toTarget = target.position - enemy.Rb.position;
        toTarget.y = 0f;

        Vector3 dir = toTarget.normalized;

        Quaternion desired = Quaternion.LookRotation(dir, Vector3.up);
        enemy.Rb.MoveRotation(Quaternion.Slerp(enemy.Rb.rotation, desired, enemy.TurnSpeed * Time.deltaTime));

        Vector3 newVelocity = dir * enemy.MoveSpeed;
        newVelocity.y = enemy.Rb.linearVelocity.y;
        enemy.Rb.linearVelocity = newVelocity;
    }
}

public class DontMuKatteKuruNoKaStrategy : IMuKatteKuruNoKaStrategy
{
    public void KonoDIOniMuKatteKuruNoKa(EnemyBase enemy, Transform target)
    {
        enemy.Rb.linearVelocity = Vector2.zero;
    }
}