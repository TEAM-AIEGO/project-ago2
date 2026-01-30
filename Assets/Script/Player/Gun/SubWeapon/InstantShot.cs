using UnityEngine;

public class InstantShot : SubWeapon
{
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform launchPoint;

    [SerializeField] private float launchDelayTime = 0.1f;

    private float delayTimer;
    private bool isLaunching;

    protected override void Initialize()
    {
        delayTimer = 0f;
        isLaunching = false;
    }

    protected override void Update()
    {
        base.Update();

        if (!isLaunching)
            return;

        delayTimer += Time.deltaTime;

        if (delayTimer >= launchDelayTime)
        {
            Fire();
            isLaunching = false;
            delayTimer = 0f;
        }
    }

    public override void Use()
    {
        if (!isReady || isLaunching)
            return;

        isLaunching = true;
        isReady = false;
        cooldownTimer = cooldownTime;
    }

    private void Fire()
    {
        objectPool.ProjectileRequest.Invoke(projectilePrefab, launchPoint.position, launchPoint.rotation);
    }
}
