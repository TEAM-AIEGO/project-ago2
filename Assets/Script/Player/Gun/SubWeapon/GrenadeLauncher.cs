using UnityEngine;
using UnityEngine.Events;

public class GrenadeLauncher : SubWeapon
{
    [SerializeField] private SFXEmitter emitter;
    [SerializeField] private GrenadeProjectile grenadeProjectilePrefab;
    [SerializeField] private Transform projectileLaunchPoint;

    private UIManager uiManager;

    protected override void Update()
    {
        base.Update();
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
        isLaunching = true;
        isReady = false;
        subWeaponObj.SetActive(true);
    }

    protected override void Fire()
    {
        Vector3 direction;

        Ray aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint = Vector3.zero;

        aimPoint = Camera.main.transform.position + Camera.main.transform.forward * 1000f;

        direction = (aimPoint - projectileLaunchPoint.position).normalized;

        GrenadeProjectile grenade = objectPool.SpawnGrenadeProjectile(projectileLaunchPoint.position);
        grenade.OnLaunched(direction);
        
        grenade.OnExplosionHit += PlayHitMarker;

        cameraShake?.AddRecoil(new Vector2(Random.Range(-300f, 300f), 500f));
    }

    public void PlayHitMarker()
    {
        uiManager.ShowHitMarker();
    }
}