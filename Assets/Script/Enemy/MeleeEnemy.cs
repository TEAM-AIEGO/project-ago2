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
        Debug.Log("Melee Enemy is Attacking");
        muDAMUDAMUDAStrategy.Attacking(this, player.transform);

        attackTime = 0f;
    }

    public override void TakeDamage(float damage)
    {
        if (health <= 0) 
            return;

        // Detect player on taking damage
        if (isPlayerDetected == false)
        {
            isPlayerDetected = true;
            state = EnemyState.moving;
        }

        health -= damage;
        Debug.Log($"Melee Enemy took {damage} damage. Remaining Health: {health}");
        
        hitFlash.Flash();
    }
}