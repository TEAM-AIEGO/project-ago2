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
        base.Idle();
    }

    protected override void Moving()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
        {
            emitter?.Play(AudioIds.VicinityRobotMoving, false, 2.3f);
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
        emitter.Play(AudioIds.RobotVicinityRobotScratch1, false, 0.7f);
        muDAMUDAMUDAStrategy.Attacking(this, player.transform);
    }

    public override void TakeDamage(float damage)
    {
        hitFlash.Flash();

        base.TakeDamage(damage);

        if (health <= maxHealth / 2)
        {
            if (spawnOnHalf)
            {
                emitter.Play(AudioIds.RobotVicinityRobotScreech, false);
                RequestSpawn();
            }
        }
    }

    protected override void Dead()
    {
        base.Dead();
    }

    public override void OnWarpStageChanged(int newStage)
    {
        base.OnWarpStageChanged(newStage);

        if (textures.Length == 0) return;
        switch (newStage)
        {
            case 0:
                up.material = textures[0];
                down.material = textures[1];
                break;
            case 1:
                up.material = textures[2];
                down.material = textures[3];
                break;
        }
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