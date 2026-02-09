using UnityEngine;

public class BossCore : Unit, IWarpObserver
{
    [SerializeField] private HitFlash hitFlash;

    private GameObject player;
    [SerializeField] private WarpSystemManager warpSystemManager;
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private TurretBase[] turrets;

    private float turretCount = 0f;

    private void Awake()
    {
        Initialize();
    }

    protected override void Update()
    {

    }

    public void Initialize()
    {
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

        turretCount = 0;

        int warpStage = warpSystemManager != null ? warpSystemManager.GetWarpStage() : 0;
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
            turret.Initialize(turret, warpStage);
            turret.Died.AddListener(TurretDestroyedCount);
            warpSystemManager?.RegisterWarpObserver(turret);
        }
    }

    private void TurretDestroyedCount()
    {
        turretCount--;
        Debug.Log("Turret Count : " + turretCount);

        if (turretCount == 0)
        {
            Debug.Log("second phase");
        }
    }
    
    private void TurrentWarpChange(int stage)
    {

    }

    public override void TakeDamage(float damage)
    {
        if (health <= 0)
            return;

        base.TakeDamage(damage);

        hitFlash.Flash();
    }

    public void OnWarpStageChanged(int newStage)
    {
        TurrentWarpChange(newStage);
    }
}
