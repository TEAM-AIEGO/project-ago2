using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NegromancyEnemy : EnemyBase
{
    [Header("Negromancy Spawn")]
    [SerializeField] private EnemyBase spawnPrefab;
    [SerializeField] private int spawnCount = 2;
    [SerializeField] private float spawnRadius = 2f;
    [SerializeField] private bool spawnOnHalf = true;

    [SerializeField] private Vector3 attackVectorRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private HitFlash hitFlash;

    public event Action<EnemySpawnRequest> SpawnRequested;

    private IORAORAORAStrategy oRAORAORAStrategy;

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        muDAMUDAMUDAStrategy = new MeleeAttackStrategy(attackVectorRange, attackPoint);
        muKatteKuruNoKaStrategy = new MuKatteKuruNoKaStrategy();
        oRAORAORAStrategy = new NegromancySpawnStrategy(spawnPrefab, spawnCount, spawnRadius);

        spawnOnHalf = true;
    }

    protected override void Idle()
    {
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
        muDAMUDAMUDAStrategy.Attacking(this, player.transform);
        attackTime = 0f;
    }

    public override void TakeDamage(float damage)
    {
        hitFlash.Flash();

        base.TakeDamage(damage);

        if (health <= maxHealth / 2)
        {
            if (spawnOnHalf)
            {
                RequestSpawn();
            }
        }
    }

    protected override void Dead()
    {
        base.Dead();
    }

    private void RequestSpawn()
    {
        spawnOnHalf = false;

        if (oRAORAORAStrategy == null)
        {
            return;
        }

        foreach (EnemySpawnRequest request in oRAORAORAStrategy.CreateSpawnRequests(this))
        {
            SpawnRequested?.Invoke(request);
        }
    }
}