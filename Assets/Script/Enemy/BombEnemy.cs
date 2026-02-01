using System;
using UnityEngine;

enum BombState
{
    idle,
    moving,
    attacking
}

[RequireComponent(typeof(Rigidbody))]
public class BombEnemy : EnemyBase
{
    [HideInInspector] public event Action<Vector3, Vector3, float> OnLaunchProjectile;

    [SerializeField] private float filghtTime = 1.5f;
    [SerializeField] private Transform LaunchPoint;
    [SerializeField] private HitFlash hitFlash;

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        muDAMUDAMUDAStrategy = new KillerQueenDaiIchiNoBakudanStrategy(filghtTime, attackDamage, LaunchPoint, OnLaunchProjectile);
        muKatteKuruNoKaStrategy = new DontMuKatteKuruNoKaStrategy();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void Idle()
    {
        // Detect player in idle state
        if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance)
        {
            state = EnemyState.moving;
        }
    }

    protected override void Moving()
    {
        muKatteKuruNoKaStrategy.KonoDIOniMuKatteKuruNoKa(this, player.transform);
    }

    protected override void AttackCheck()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < AttackDictance)
        {
            if (!canAttack)
                return;

            state = EnemyState.attacking;
        }
    }

    protected override void Attacking()
    {
        muDAMUDAMUDAStrategy.Attacking(this, player.transform);
        attackTime = 0f;
    }

    public override void OnWarpStageChanged(int newStage)
    {
        base.OnWarpStageChanged(newStage);

        switch (newStage)
        {
            case 0:
                muKatteKuruNoKaStrategy = new DontMuKatteKuruNoKaStrategy();
                break;
            case 1:
                muKatteKuruNoKaStrategy = new MuKatteKuruNoKaStrategy();
                break;
        }
    }

    public override void TakeDamage(float damage)
    {
        if (health <= 0)
            return;

        // Detect player on taking damage
        if (isPlayerDetected == false)
        {
            isPlayerDetected = true;
            state = EnemyState.moving;
        }

        health -= damage;
        Debug.Log($"Bomb Enemy took {damage} damage. Remaining Health: {health}");

        hitFlash.Flash();
    }
}
