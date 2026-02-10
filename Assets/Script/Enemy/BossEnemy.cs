using System;
using UnityEngine;

enum BossState
{
    idle,
    moving,
    attacking
}

[RequireComponent(typeof(Rigidbody))]
public class BossEnemy : EnemyBase
{
    [HideInInspector] public event Action<Projectile, Vector3, Quaternion, float, float> OnShootSansWall;
    
    [SerializeField] private Transform ShootPoint;
    [SerializeField] private float projectileSpeed = 40f;
    [SerializeField] private float maxLeadTime = 2f;
    [SerializeField] private HitFlash hitFlash;
    [SerializeField] private Projectile projectilePrefab;
    public Projectile ProjectilePrefab => projectilePrefab;

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        muDAMUDAMUDAStrategy = new SansBoneWallAttackStrategy(projectileSpeed, attackDamage, maxLeadTime, ShootPoint, projectilePrefab, OnShootSansWall);
        muKatteKuruNoKaStrategy = new MuKatteKuruNoKaStrategy();
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
        //Debug.Log("Melee Enemy is Moving");
        //Debug.Log(player.transform.position);
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
        Debug.Log("Boss Enemy is Attacking");
        muDAMUDAMUDAStrategy.Attacking(this, player.transform);

        attackTime = 0f;
    }

    public override void TakeDamage(float damage)
    {
        hitFlash.Flash();

        base.TakeDamage(damage);
    }
}