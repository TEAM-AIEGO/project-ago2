using UnityEngine;

enum BombState
{
    idle,
    moving,
    attacking
}

[RequireComponent(typeof(Rigidbody))]
public class BombEnemy : EnemyBase
{
    [SerializeField] private Vector3 attackVectorRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private HitFlash hitFlash;
    public override void Initialize(EnemyBase origin)
    {
        base.Initialize(origin);

        //나중에 Bomb Attack Strategy 넣기
        dontMuKatteKuruNoKaStrategy = new DontMuKatteKuruNoKaStrategy();
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
        dontMuKatteKuruNoKaStrategy.KonoDIOniMuKatteKuruNoKa(this, player.transform);
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
        Debug.Log($"Bomb Enemy took {damage} damage. Remaining Health: {health}");

        hitFlash.Flash();
    }
}
