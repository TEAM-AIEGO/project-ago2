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
        base.Idle();
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
    }

    public override void OnWarpStageChanged(int newStage)
    {
        base.OnWarpStageChanged(newStage);

        switch (newStage)
        {
            case 0:
                muKatteKuruNoKaStrategy = new DontMuKatteKuruNoKaStrategy();
                if (enemyAnimator.GetBool("Move"))
                    enemyAnimator.SetBool("Move", false);
                break;
            case 1:
                muKatteKuruNoKaStrategy = new MuKatteKuruNoKaStrategy();
                if (!enemyAnimator.GetBool("Move") && state == EnemyState.moving)
                    enemyAnimator.SetBool("Move", true);
                break;
        }

        if (textures.Length == 0) return;
        switch (newStage)
        {
            case 0:
                up.material = textures[0];
                break;
            case 1:
                up.material = textures[1];
                break;
        }
    }

    public override void TakeDamage(float damage)
    {
        if (hitFlash)
        {
            hitFlash.Flash();
        }

        base.TakeDamage(damage);
    }
}