using System;
using UnityEngine;

enum RangedState
{
    idle,
    moving,
    attacking
}

[RequireComponent(typeof(Rigidbody))]
public class RangedEnemy : EnemyBase
{
    [HideInInspector] public event Action<Projectile, Vector3, Quaternion, float, float> OnShootProjectile;

    [SerializeField] private Transform ShootPoint;
    [SerializeField] private float projectileSpeed = 40f;
    [SerializeField] private float maxLeadTime = 2f;
    [SerializeField] private HitFlash hitFlash;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        muDAMUDAMUDAStrategy = new GermanysTechnologyStrategy(projectileSpeed, attackDamage, maxLeadTime, ShootPoint, projectilePrefab, OnShootProjectile);
        muKatteKuruNoKaStrategy = new DontMuKatteKuruNoKaStrategy();
    }

    protected override void Update()
    {
        base.Update();
        if (!isDead && health <= 0)
        {
            state = EnemyState.Dead;
            isDead = true;
            Died?.Invoke();
        }
    }
    protected override void Idle()
    {
        if (state == EnemyState.Dead)
            return;

        base.Idle();
    }

    protected override void Moving()
    {
        if (state == EnemyState.Dead)
            return;

        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
        {
            
            emitter?.Play(AudioIds.RobotTurretRobotMoving, false, 0.2f, 0.6f);
            footstepTimer = interval;
        }
        muKatteKuruNoKaStrategy.KonoDIOniMuKatteKuruNoKa(this, player.transform);
    }

    protected override void AttackCheck()
    {
        if (state == EnemyState.Dead)
            return;

        if (Vector3.Distance(player.transform.position, transform.position) < AttackDictance)
        {
            if (!canAttack)
                return;

            state = EnemyState.attacking;
        }
    }

    protected override void LookTarget()
    {
        if (state == EnemyState.Dead)
            return;

        base.LookTarget();
        //Vector3 hDirection = (player.transform.position - head.transform.position).normalized;
        //Quaternion hLookRotation = Quaternion.LookRotation(new Vector3(hDirection.x, hDirection.y, hDirection.z));
        //head.transform.rotation = Quaternion.Slerp(head.transform.rotation, hLookRotation, Time.deltaTime * turnSpeed);

        //if (muKatteKuruNoKaStrategy is MuKatteKuruNoKaStrategy)
        //{
        //    Vector3 direction = (player.transform.position - body.transform.position).normalized;
        //    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //    body.transform.rotation = Quaternion.Slerp(head.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        //}
    }

    protected override void Attacking()
    {
        if (state == EnemyState.Dead)
            return;

        emitter.PlayFollow(AudioIds.RobotTurretRobotShot, transform, false, 0.4f, 0.7f);
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
                if (enemyAnimator.GetBool("Move"))
                    enemyAnimator.SetBool("Move", false);
                break;
            case 1:
                muKatteKuruNoKaStrategy = new MuKatteKuruNoKaStrategy();
                if (!enemyAnimator.GetBool("Move") && state == EnemyState.moving)
                    enemyAnimator.SetBool("Move", true);
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
