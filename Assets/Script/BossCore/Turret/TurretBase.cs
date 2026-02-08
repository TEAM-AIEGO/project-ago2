using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class TurretBase : EnemyBase
{
    [Header("Turret Settings")]
    [SerializeField] protected Transform ShootPoint;
    [SerializeField] protected HitFlash hitFlash;
    [SerializeField] private BossCore bossCore;

    public BossCore BossCore => bossCore;

    public void SetBossCore(BossCore core)
    {
        bossCore = core;
    }
    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);
    }
    protected override void Update()
    {
        if (!isDead && health <= 0)
        {
            isDead = true;
            Destroy(gameObject);
            return;
        }

        base.Update();
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