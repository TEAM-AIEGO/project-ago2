using System;
using UnityEngine;

public class GrenadeTurret : TurretBase
{
    [HideInInspector] public event Action<Vector3, Vector3, float> OnLaunchProjectile;

    [SerializeField] private float filghtTime = 1.5f;

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        muDAMUDAMUDAStrategy = new KillerQueenDaiIchiNoBakudanStrategy(filghtTime, attackDamage, ShootPoint, OnLaunchProjectile);
        muKatteKuruNoKaStrategy = new DontMuKatteKuruNoKaStrategy();
    }

    protected override void Idle()
    {
        if (!EnsurePlayer())
            return;

        if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance)
        {
            state = EnemyState.moving;
        }
    }

    protected override void Moving()
    {
        return;
    }

    protected override void Attacking()
    {
        muDAMUDAMUDAStrategy.Attacking(this, player.transform);
        attackTime = 0f;
    }

    public override void TakeDamage(float damage)
    {
        if (health <= 0)
            return;

        if (isPlayerDetected == false && EnsurePlayer())
        {
            isPlayerDetected = true;
            state = EnemyState.moving;
        }

        health -= damage;
        Debug.Log($"{name} took {damage} damage. Remaining Health: {health}");

        hitFlash.Flash();
    }
}