using UnityEngine;

enum RangedState
{
    idle,
    moving,
    attacking
}

[RequireComponent(typeof(Rigidbody))]
public class RangedEnemy : EnemyBase
{
    [SerializeField] private Vector3 attackVectorRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private HitFlash hitFlash;
    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        //나중에 Ranged Attack Strategy 넣기
        muKatteKuruNoKaStrategy = new DontMuKatteKuruNoKaStrategy();
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
        Debug.Log($"Ranged Enemy took {damage} damage. Remaining Health: {health}");

        hitFlash.Flash();
    }
}
