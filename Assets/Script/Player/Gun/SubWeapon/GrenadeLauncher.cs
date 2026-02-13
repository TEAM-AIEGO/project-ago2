using UnityEngine;
using UnityEngine.Events;

public class GrenadeLauncher : SubWeapon
{
    [SerializeField] private SFXEmitter emitter;
    [SerializeField] private GrenadeProjectile grenadeProjectilePrefab;
    [SerializeField] private Transform projectileLaunchPoint;

    protected override void Update()
    {
        base.Update();
    }

    public override void UnLock()
    {
        base.UnLock();
    }

    public override void Initialize(UnityEvent completeEvent) 
    {
        uiManager = FindFirstObjectByType<UIManager>();
        if (!uiManager)
        {
            Debug.LogWarning("UIManager not found! Hitmarker will not show.");
        }

        onSubWeaponUseComplete = completeEvent;

        delayTimer = 0f;
        isLaunching = false;
        isInAftereffect = false;
    }

    public override void Use()
    {
        base.Use();
    }

    protected override void Fire()
    {
        Vector3 direction;

        Ray aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint = Vector3.zero;

        aimPoint = Camera.main.transform.position + Camera.main.transform.forward * 1000f;

        direction = (aimPoint - projectileLaunchPoint.position).normalized;

        GrenadeProjectile grenade = objectPool.SpawnGrenadeProjectile(projectileLaunchPoint.position);
        emitter.PlayFollow(AudioIds.GrenadeLauncherFire, playerTransform);
        grenade.OnLaunched(direction);
        
        grenade.OnExplosionHit += PlayHitMarker;

        cameraShake?.AddRecoil(new Vector2(Random.Range(-100f, 100f), 300f));
    }

    public void PlayHitMarker()
    {
        uiManager.ShowHitMarker();
    }
}