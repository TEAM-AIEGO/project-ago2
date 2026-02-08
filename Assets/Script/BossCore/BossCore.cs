using UnityEngine;

public class BossCore : Unit, IWarpObserver
{
    [SerializeField] private HitFlash hitFlash;

    private GameObject player;
    [SerializeField] private WarpSystemManager warpSystemManager;
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private TurretBase[] turrets;
    private void Awake()
    {
        Initialize();
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
            }

            turret.SetBossCore(this);
            turret.Initialize(turret, warpStage);
            warpSystemManager?.RegisterWarpObserver(turret);
        }
    }

    private void TurretActivate()
    {

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
