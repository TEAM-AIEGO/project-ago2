using UnityEngine;

enum MeleeState
{
    idle,
    moving,
    attacking
}

[RequireComponent(typeof(Rigidbody))]
public class MeleeEnemy : EnemyBase
{
    [SerializeField] private Vector3 attackVectorRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private HitFlash hitFlash;
    private SFXEmitter emitter;
    private float footstepTimer;
    [SerializeField] private float interval;

    private void Awake()
    {
        emitter = GetComponent<SFXEmitter>();
    }

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        muDAMUDAMUDAStrategy = new MeleeAttackStrategy(attackVectorRange, attackPoint);
        muKatteKuruNoKaStrategy = new MuKatteKuruNoKaStrategy();
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
        //Debug.Log("Melee Enemy is Moving");
        //Debug.Log(player.transform.position);
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
        {
            emitter?.Play("vicinity robot moving", false, 2.3f);
            footstepTimer = interval;
        }
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
        Debug.Log("Melee Enemy is Attacking");
        muDAMUDAMUDAStrategy.Attacking(this, player.transform);

        attackTime = 0f;
    }

    public override void TakeDamage(float damage)
    {
        hitFlash.Flash();

        base.TakeDamage(damage);
    }
}