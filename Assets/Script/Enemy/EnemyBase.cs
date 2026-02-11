using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))] 
public abstract class EnemyBase : Unit, IWarpObserver, IKnockable
{
    protected enum EnemyState
    {
        idle,
        moving,
        attacking
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

    [SerializeField] protected Avatar firstAvatar;
    [SerializeField] protected Avatar secondAvatar;
    [SerializeField] protected GameObject firstModels;
    [SerializeField] protected GameObject secondModels;
    private int fL;
    private int sL;

    protected float footstepTimer;
    [SerializeField] protected float interval;

    protected SFXEmitter emitter;
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

    protected bool isPlayerDetected = false;
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
        emitter = GetComponent<SFXEmitter>();
        fL = enemyAnimator.GetLayerIndex("First_Layer");
        sL = enemyAnimator.GetLayerIndex("Second_Layer");

        enemyAnimator.avatar = firstAvatar;
        enemyAnimator.SetLayerWeight(fL, 1);
        enemyAnimator.SetLayerWeight(sL, 0);
        enemyAnimator.SetBool("Move", false);

        state = EnemyState.idle;

        OnWarpStageChanged(warpStage);

        Died.RemoveAllListeners();
        Died.AddListener(Dead);
    }

    protected override void Update()
    {
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
                    Moving();
                }
                else
                {
                    //Debug.Log(knockbackStun);
                    knockbackStun = Mathf.Max(0, knockbackStun - Time.deltaTime);
                }
                AttackCheck();
                break;
            case EnemyState.attacking:
                if (!canAttack)
                {
                    state = EnemyState.moving;
                    break;
                }
                //enemyAnimator.SetTrigger("Attack");
                Attacking();
                canAttack = false; 
                break;
        }

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

    public virtual void TakeKnockback(Vector3 force, float duration)
    {
        if (muKatteKuruNoKaStrategy is DontMuKatteKuruNoKaStrategy && force.magnitude < 50f)
            return;

        knockbackStun += duration;
        rb.AddForce(force, ForceMode.Impulse);
    }

    public virtual void TakeExplosionKnockback(float explosionForce, Vector3 explosionPosition, float explosionRadius, float duration)
    {
        knockbackStun += duration;
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
        if (Vector3.Distance(player.transform.position, transform.position) < detectionDistance)
        {
            if (enemyAnimator != null)
                enemyAnimator.SetBool("Move", true);

            emitter.PlayFollow(AudioIds.RobotTargetAcquiredBleep, transform, false, 0f, 0.1f);
            state = EnemyState.moving;
        }
    }

    protected abstract void Moving();

    protected abstract void AttackCheck();

    public void OnAttack() => Attacking();

    protected abstract void Attacking();

    public override void TakeDamage(float damage)
    {
        emitter.Play(AudioIds.RobotRobotHeat, false);
        base.TakeDamage(damage);
        if (isPlayerDetected == false)
        {
            isPlayerDetected = true;

            if (enemyAnimator != null)
                enemyAnimator.SetBool("Move", true);
            state = EnemyState.moving;
        }
    }

    protected virtual void Dead()
    {
        List<string> audioIdsList = new List<string>
        { 
            AudioIds.RobotRobotCrunch1,
            AudioIds.RobotRobotCrunch2,
            AudioIds.RobotRobotCrunch3 
        };
        int randomAudioIdsIdx = UnityEngine.Random.Range(0, audioIdsList.Count);
        emitter.PlayFollow(audioIdsList[randomAudioIdsIdx], transform);
        OnReturn?.Invoke(this);
    }

    public virtual void OnWarpStageChanged(int newStage)
    {
        if (firstModels != null && secondModels != null)
            switch (newStage)
            {
                case 0:
                    firstModels.SetActive(true);
                    secondModels.SetActive(false);
                    enemyAnimator.avatar = firstAvatar;
                    enemyAnimator.SetLayerWeight(fL, 1);
                    enemyAnimator.SetLayerWeight(sL, 0);
                    break;
                case 1:
                    secondModels.SetActive(true);
                    firstModels.SetActive(false);
                    enemyAnimator.avatar = secondAvatar;
                    enemyAnimator.SetLayerWeight(fL, 0);
                    enemyAnimator.SetLayerWeight(sL, 1);
                    break;
            }
        
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
