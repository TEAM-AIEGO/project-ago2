using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class BossCore : EnemyBase, IWarpObserver
{
    [HideInInspector] public event Action<Projectile, Vector3, Quaternion, float, float> OnShootSansWall;
    [HideInInspector] public event Action<Projectile, Vector3, Quaternion, float, float> OnShootLaserProjectile;

    [SerializeField] private HitFlash hitFlash;

    [Header("Sans Wall Setting")]
    [SerializeField] private Projectile sansWallProjectilePrefab;
    [SerializeField] private Transform sansWallShootPoint;
    [SerializeField] private float sansWallProjectileSpeed = 40f;
    [SerializeField] private float sansWallMaxLeadTime = 2f;

    [Header("Laserrrr Setting")]
    [SerializeField] private LineRenderer laserLineRenderer;
    [SerializeField] private LayerMask laserMask;
    [SerializeField] private float ziiiiingDistance;
    [SerializeField] private float ziiiiingDuration;
    [SerializeField] private float ziiiiingFire;
    [SerializeField] private float ziiiiingTimer;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform laserShootPoint;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform targetTransform;
    private Vector3 targetVector;
    private bool isZiiiiingStart;
    private bool isZiiiiingEnd;

    [SerializeField] private WarpSystemManager warpSystemManager;
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private TurretBase[] turrets;

    private float turretCount = 0f;
    
    private bool isSecondPhase = false;

    public void Initialize(int warpStage)
    {
        isPlayerDetected = false;
        health = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("player is Null");
        }

        state = EnemyState.idle;
        NextStrategy();

        if (warpSystemManager == null)
        {
            warpSystemManager = FindFirstObjectByType<WarpSystemManager>();
        }

        if (turrets == null || turrets.Length == 0)
        {
            turrets = GetComponentsInChildren<TurretBase>(true);
        }

        if (objectPool == null)
        {
            objectPool = FindFirstObjectByType<ObjectPool>();
        }

        laserLineRenderer.positionCount = 2;
        laserLineRenderer.enabled = false;
        isSecondPhase = false;

        turretCount = 0;

        //warpStage = warpSystemManager != null ? warpSystemManager.GetWarpStage() : 0;
        foreach (var turret in turrets)
        {
            if (turret == null)
                continue;

            if (objectPool != null)
            {
                if (turret is BurstTurret burstTurret)
                {
                    burstTurret.OnShootProjectile -= objectPool.SpawnProjectile;
                    burstTurret.OnShootProjectile += objectPool.SpawnProjectile;
                }

                if (turret is GrenadeTurret grenadeTurret)
                {
                    grenadeTurret.OnLaunchProjectile -= objectPool.SpawnEnemyGrenadeProjectile;
                    grenadeTurret.OnLaunchProjectile += objectPool.SpawnEnemyGrenadeProjectile;
                }

                if (turret is LaserTurret laserTurret)
                {
                    laserTurret.OnShootProjectile += objectPool.SpawnProjectile;
                }
            }

            turretCount++;
            Debug.Log("Turret Count : " + turretCount);
            turret.SetBossCore(this);
            turret.gameObject.SetActive(true);
            turret.Initialize(turret, warpStage);
            turret.Died.AddListener(TurretDestroyedCount);
            warpSystemManager?.RegisterWarpObserver(turret);

        }

        Died.RemoveAllListeners();
        Died.AddListener(Dead);
    }

    protected override void Update()
    {
        if (!isSecondPhase)
            return;

        if (!isDead && health <= 0)
        {
            isDead = true;
            Died?.Invoke();
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
                if (muDAMUDAMUDAStrategy is GermanysTechnologyStrategy)
                {
                    if (!canAttack)
                    {
                        state = EnemyState.moving;
                        break;
                    }

                    LookTarget();

                    if (isZiiiiingEnd)
                    {
                        laserLineRenderer.enabled = false;
                        Attacking();
                    }
                }
                else
                {
                    if (!canAttack)
                    {
                        state = EnemyState.moving;
                        break;
                    }

                    Attacking();
                    canAttack = false;
                }
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

        if (isZiiiiingStart)
        {
            if (ziiiiingTimer >= ziiiiingDuration)
            {
                if (ziiiiingTimer >= ziiiiingFire)
                {
                    isZiiiiingStart = false;
                    isZiiiiingEnd = true;
                    ziiiiingTimer = 0;
                }
            }
            else
            {
                targetVector = player.transform.position;
            }

            ziiiiingTimer += Time.deltaTime;
            Ziiiiing();
        }
    }

    private void Ziiiiing()
    {
        Vector3 start = laserShootPoint.position;
        Vector3 direction = (targetVector - laserShootPoint.position).normalized;
        Vector3 end = start + direction * ziiiiingDistance;

        targetTransform.position = end;

        if (Physics.Raycast(start, direction, out RaycastHit hitInfo, ziiiiingDistance, laserMask, QueryTriggerInteraction.Ignore))
        {
            end = hitInfo.point;
            targetTransform.position = hitInfo.point;
        }

        laserLineRenderer.startColor = Color.red;
        laserLineRenderer.endColor = Color.red;
        laserLineRenderer.SetPosition(0, start);
        laserLineRenderer.SetPosition(1, end);
    }

    private void TurretDestroyedCount()
    {
        turretCount--;
        Debug.Log("Turret Count : " + turretCount);

        if (turretCount == 0)
        {
            isSecondPhase = true;
            Debug.Log("second phase");
        }
    }
    
    private void TurrentWarpChange(int stage)
    {

    }

    public override void OnWarpStageChanged(int newStage)
    {
        if (isSecondPhase) 
            return;

        TurrentWarpChange(newStage);
    }

    protected override void Moving()
    {
        return;
    }

    protected override void AttackCheck()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < AttackDictance)
        {
            if (!canAttack)
                return;

            if (muDAMUDAMUDAStrategy is GermanysTechnologyStrategy)
            {
                isZiiiiingStart = true;
                laserLineRenderer.enabled = true;
            }

            state = EnemyState.attacking;
        }
    }

    protected override void Attacking()
    {
        muDAMUDAMUDAStrategy.Attacking(this, targetTransform);
        attackTime = 0f;
        NextStrategy();
        isZiiiiingEnd = false;
    }

    public override void TakeDamage(float damage)
    {
        if (health <= 0)
            return;

        base.TakeDamage(damage);

        hitFlash.Flash();
    }

    public override void TakeKnockback(Vector3 force, float duration)
    {
        return;
    }

    public override void TakeExplosionKnockback(float explosionForce, Vector3 explosionPosition, float explosionRadius, float duration)
    {
        return;
    }

    protected override void Dead()
    {
        laserLineRenderer.enabled = false;
        //base.Dead();
    }

    private void NextStrategy()
    {
        int randomStrategy = UnityEngine.Random.Range(0, 10);

        if (randomStrategy < 10 / 2)
        {
            muDAMUDAMUDAStrategy = new SansBoneWallAttackStrategy(sansWallProjectileSpeed, attackDamage, sansWallMaxLeadTime, sansWallShootPoint, sansWallProjectilePrefab, OnShootSansWall);
        }
        else
        {
            muDAMUDAMUDAStrategy = new GermanysTechnologyStrategy(projectileSpeed, attackDamage, 0f, laserShootPoint, projectilePrefab, OnShootLaserProjectile);
        }
    }
}
