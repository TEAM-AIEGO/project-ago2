using UnityEngine;

public class BossCore : Unit, IWarpObserver
{
    [SerializeField] private HitFlash hitFlash;

    private GameObject player;

    public void Initialize()
    {

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
