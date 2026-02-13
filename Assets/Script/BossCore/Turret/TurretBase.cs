using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class TurretBase : EnemyBase
{
    [Header("Turret Settings")]
    [SerializeField] protected Transform ShootPoint;
    [SerializeField] protected HitFlash hitFlash;
    [SerializeField] private BossCore bossCore;

    [SerializeField] private Transform turretHead;
    [SerializeField] private Transform poSin;

    protected bool isDestroyed = false;

    public BossCore BossCore => bossCore;

    public void SetBossCore(BossCore core) => bossCore = core;

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        isDestroyed = false;
    }

    protected override void Update()
    {
        if (isDestroyed)
            return;

        if (state == EnemyState.Dead)
        {
            bodyDisableTimer += Time.deltaTime;

            if (bodyDisableTimer >= bodyDisableTime)
            {
                Return();
            }
            return;
        }

        switch (state)
        {
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.moving:
                //Debug.Log("Enemy is moving");
                if (knockbackStun <= 0)
                {
                    if (muKatteKuruNoKaStrategy is DontMuKatteKuruNoKaStrategy)
                        LookTarget();

                    if (Vector3.Distance(player.transform.position, transform.position) < AttackDictance && !canAttack)
                    {
                        state = EnemyState.idle;
                        rb.linearVelocity = Vector3.zero;
                        enemyAnimator.SetBool("Move", false);
                        return;
                    }

                    Moving();
                    AttackCheck();
                }
                else
                {
                    //Debug.Log(knockbackStun);
                    knockbackStun = Mathf.Max(0, knockbackStun - Time.deltaTime);
                }
                break;
            case EnemyState.attacking:
                if (enemyAnimator != null)
                {
                    if (isAttacking != true)
                    {
                        //enemyAnimator.applyRootMotion = true;
                        isAttacking = true;
                        BeginAttack();
                    }

                    LookTarget();
                }
                else
                {
                    Attacking();
                    canAttack = false;
                }
                //Attacking();
                //canAttack = false;
                break;
        }

        if (!isAttacking)
        {
            if (attackTime < attackCooldown)
            {
                attackTime += Time.deltaTime;
                canAttack = false;
            }
            else
            {
                canAttack = true;
            }
        }
        else
        {
            canAttack = false;
        }
    }

    protected override void LookTarget()
    {
        Vector3 direction = (player.transform.position - turretHead.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, lookRotation, Time.deltaTime * turnSpeed);

        Vector3 direction2 = (player.transform.position - poSin.position).normalized;
        Quaternion lookRotation2 = Quaternion.LookRotation(new Vector3(direction2.x, direction2.y, direction2.z));
        poSin.rotation = Quaternion.Slerp(poSin.rotation, lookRotation2, Time.deltaTime * turnSpeed);
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
        base.TakeDamage(damage);

        //if (isPlayerDetected == false && EnsurePlayer())
        //{
        //    isPlayerDetected = true;
        //    state = EnemyState.moving;
        //}

        //Debug.Log($"{name} took {damage} damage. Remaining Health: {health}");
    }


    public override void TakeKnockback(Vector3 force, float duration)
    {
        return;
    }

    public override void TakeExplosionKnockback(float explosionForce, Vector3 explosionPosition, float explosionRadius, float duration)
    {
        return;
    }

    protected override void Dead()
    {
        state = EnemyState.Dead;
        hitFlash.enabled = false;
    }

    protected bool EnsurePlayer()
    {
        if (player != null)
            return true;

        player = GameObject.FindGameObjectWithTag("Player");
        return player != null;
    }
}