using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBase : Unit, IWarpObserver
{
    protected enum EnemyState
    {
        idle,
        moving,
        attacking
    }

    protected EnemyState state;
    protected GameObject player;
    protected Rigidbody rb;
    public Rigidbody Rb => rb;

    protected EnemyBase originEnemy;
    public EnemyBase OriginEnemy => originEnemy;

    protected IAttackStrategy attackStrategy;
    protected IMuKatteKuruNoKaStrategy muKatteKuruNoKaStrategy;

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

    public event Action<EnemyBase> OnReturn;

    void Start()
    {

    }

    public virtual void Initialize(EnemyBase oringin)
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null )
        {
            Debug.LogError("player is Null");
        }

        rb = GetComponent<Rigidbody>();

        state = EnemyState.idle;

        Died.AddListener(Dead);
    }

    protected override void Update()
    {
        base.Update();

        switch (state)
        {
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.moving:
                //Debug.Log("Enemy is moving");
                Moving();
                AttackCheck();
                break;
            case EnemyState.attacking:
                if (!canAttack)
                {
                    state = EnemyState.moving;
                    break;
                }
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

    protected abstract void Idle();

    protected abstract void Moving();

    protected abstract void AttackCheck();

    protected abstract void Attacking();

    protected virtual void Dead()
    {
        Destroy(gameObject);
        //OnReturn?.Invoke(this);
    }

    public void OnWarpStageChanged(int newStage)
    {
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
