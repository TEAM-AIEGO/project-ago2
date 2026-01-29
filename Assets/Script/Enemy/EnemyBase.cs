using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBase : Unit
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

    protected IAttackStrategy attackStrategy;
    protected IMuKatteKuruNoKaStrategy muKatteKuruNoKaStrategy;

    [SerializeField] protected float moveSpeed;
    public float MoveSpeed => moveSpeed;
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

    void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null )
        {
            Debug.LogError("player is Null");
        }

        rb = GetComponent<Rigidbody>();

        state = EnemyState.idle;
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

    public abstract void TakeDamage(float damageAmount);
}
