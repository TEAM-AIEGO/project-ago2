using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class TurretBase : EnemyBase
{
    [HideInInspector] public event Action<Projectile, Vector3, Quaternion, float, float> OnShootProjectile;

    [Header("Turret Settings")]
    [SerializeField] protected Transform ShootPoint;
    [SerializeField] protected float projectileSpeed = 40f;
    [SerializeField] protected float maxLeadTime = 2f;
    [SerializeField] protected HitFlash hitFlash;
    [SerializeField] protected Projectile projectilePrefab;
    public Projectile ProjectilePrefab => projectilePrefab;

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        muDAMUDAMUDAStrategy = new GermanysTechnologyStrategy(projectileSpeed, attackDamage, maxLeadTime, ShootPoint, projectilePrefab, OnShootProjectile);
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

    protected override void AttackCheck()
    {
        if (!EnsurePlayer())
            return;

        if (Vector3.Distance(player.transform.position, transform.position) < AttackDictance)
        {
            if (!canAttack)
                return;

            state = EnemyState.attacking;
        }
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

    protected bool EnsurePlayer()
    {
        if (player != null)
            return true;

        player = GameObject.FindGameObjectWithTag("Player");
        return player != null;
    }
}