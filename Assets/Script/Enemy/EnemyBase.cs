using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))] 
public abstract class EnemyBase : Unit, IWarpObserver, IKnockable
{
    protected enum EnemyState
    {
        idle,
        moving,
        attacking,
        Dead
    }

    protected EnemyState state;
    protected GameObject player;
    protected Animator enemyAnimator;
    protected Rigidbody rb;
    public Rigidbody Rb => rb;
    protected float knockbackStun;

    protected EnemyBase originEnemy;
    public EnemyBase OriginEnemy => originEnemy;

    protected IMUDAMUDAMUDAStrategy muDAMUDAMUDAStrategy;
    protected IMuKatteKuruNoKaStrategy muKatteKuruNoKaStrategy;
    protected IORAORAORAStrategy NegromancyStrategy;

    [SerializeField] protected RagdollTrigger rdTrigger;

    [SerializeField] protected SkinnedMeshRenderer up;
    [SerializeField] protected SkinnedMeshRenderer down;

    [SerializeField] protected Material[] textures;

    #region Enemy Stats
    [Header("Enemy Stats")]
    [SerializeField] protected float stage1MoveSpeed;
    [SerializeField] protected float stage2MoveSpeed;
    [SerializeField] protected float currentMoveSpeed;
    public float MoveSpeed => currentMoveSpeed;
    [SerializeField] protected float turnSpeed;
    public float TurnSpeed => turnSpeed;

    [SerializeField] protected float detectionDistance;
    [SerializeField] protected float attackDistance;
    public float AttackDictance => attackDistance;

    [SerializeField] protected float attackDamage;
    public float AttackDamage => attackDamage;

    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackTime;
    [SerializeField] protected bool canAttack = true;

    [SerializeField] protected float bodyDisableTime = 5f;
    [SerializeField] protected float bodyDisableTimer;

    protected bool isPlayerDetected = false;
    [SerializeField] protected bool isAttacking = false;
    #endregion

    [HideInInspector] public event Action<EnemyBase> OnReturn;

    public virtual void Initialize(EnemyBase oringin, int warpStage)
    {
        isPlayerDetected = false;
        health = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null )
        {
            Debug.LogError("player is Null");
        }

        originEnemy = oringin;
        rb = GetComponent<Rigidbody>();
        enemyAnimator = GetComponent<Animator>();

        enemyAnimator.applyRootMotion = false;
        enemyAnimator.SetBool("Move", false);

        state = EnemyState.idle;

        OnWarpStageChanged(warpStage);

        Died.RemoveAllListeners();
        Died.AddListener(Dead);
    }

    protected override void Update()
    {
        if (state == EnemyState.Dead)
        {
            bodyDisableTimer += Time.deltaTime;

            if (bodyDisableTimer >= bodyDisableTime)
            {
                OnReturn.Invoke(this);
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

    public virtual void TakeKnockback(Vector3 force, float duration)
    {
        if (muKatteKuruNoKaStrategy is DontMuKatteKuruNoKaStrategy && force.magnitude < 50f)
            return;

        knockbackStun += duration;
        if (state == EnemyState.Dead && rdTrigger)
        {
            rdTrigger.RagdollKnockback(force);
            return;
        }

        rb.AddForce(force, ForceMode.Impulse);

    }

    public virtual void TakeExplosionKnockback(float explosionForce, Vector3 explosionPosition, float explosionRadius, float duration)
    {
        knockbackStun += duration;

        if (state == EnemyState.Dead && rdTrigger)
        {
            rdTrigger.RagdollExplosionKnockback(explosionForce, explosionPosition, explosionRadius);
            return;
        }

        rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, 2f, ForceMode.VelocityChange);
    }

    protected virtual void LookTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    protected virtual void Idle()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > AttackDictance)
        {
            PlayerDetectCheck();
        }
        else
        {
            LookTarget();
            AttackCheck();
        }
    }
    
    protected virtual void PlayerDetectCheck()
    {
        if (!isPlayerDetected)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance)
            {
                if (enemyAnimator != null)
                    if (muKatteKuruNoKaStrategy is not DontMuKatteKuruNoKaStrategy)
                        enemyAnimator.SetBool("Move", true);

                isPlayerDetected = true;
                state = EnemyState.moving;
            }

        }
        else
        {
            if (enemyAnimator != null)
                if (muKatteKuruNoKaStrategy is not DontMuKatteKuruNoKaStrategy)
                    enemyAnimator.SetBool("Move", true);

            state = EnemyState.moving;
        }
    }

    protected abstract void Moving();

    protected abstract void AttackCheck();

    protected virtual void BeginAttack()
    {
        if (!isAttacking)
            return;

        Debug.Log("Begin Attack");

        enemyAnimator.SetTrigger("Attack");
    }

    public virtual void OnAttack()
    {
        Debug.Log("Attack");
        Attacking();
    }

    protected abstract void Attacking();

    public virtual void OnAttackEnd()
    {
        Debug.Log("is Attack End");
        //enemyAnimator.applyRootMotion = false;
        isAttacking = false;
        canAttack = false;
        attackTime = 0f;

        enemyAnimator.SetBool("Move", false);
        state = EnemyState.idle;
    }

    public override void TakeDamage(float damage)
    {
        if (state == EnemyState.Dead)
            return;

        base.TakeDamage(damage);

        if (isPlayerDetected == false)
        {
            isPlayerDetected = true;

            if (enemyAnimator != null)
                if (muKatteKuruNoKaStrategy is not DontMuKatteKuruNoKaStrategy)
                    enemyAnimator.SetBool("Move", true);

            state = EnemyState.moving;
        }
    }

    protected virtual void Dead()
    {
        state = EnemyState.Dead;
        rb.constraints = RigidbodyConstraints.None;
        if (!rdTrigger) return;
        rdTrigger.SetRagdoll(true);

        //OnReturn?.Invoke(this);
    }

    public virtual void OnWarpStageChanged(int newStage)
    {
        if (state == EnemyState.Dead)
            return;

        currentMoveSpeed = GetSpeed(newStage);
    }

    protected int GetSpeed(int warpStage)
    {
        return warpStage switch
        {
            0 => (int)stage1MoveSpeed,
            1 => (int)stage2MoveSpeed,
            _ => (int)stage1MoveSpeed,
        };
    }
}