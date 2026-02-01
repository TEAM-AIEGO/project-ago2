using UnityEngine;
using UnityEngine.Events;

public class RailCannon : SubWeapon
{
    [SerializeField] private SFXEmitter emitter;
    [SerializeField] private Projectile railCannonProjectilePrefab;
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

        RaycastHit[] hitInfo = Physics.RaycastAll(aimRay, 1000, LayerMask.GetMask("Enemy", "Hittable", "Ground"));

        for (int i = 0; i < hitInfo.Length; i++)
        {
            var currentObject = hitInfo[i].collider.gameObject;
            if (currentObject.TryGetComponent(out EnemyBase enemyBase))
            {
                enemyBase.TakeDamage(99f);
                uiManager.ShowHitMarker();
            }
            if (currentObject.TryGetComponent(out IKnockable knockable))
            {
                knockable.TakeKnockback(aimRay.direction * 250, 0.5f);
            }
            if (i == hitInfo.Length - 1)
            {
                aimPoint = hitInfo[i].point;
            }
        }

        if (aimPoint != Vector3.zero)
        {
            aimPoint = Camera.main.transform.position + Camera.main.transform.forward * 1000f;
        }

        Vector3 aimDirection = (aimPoint - projectileLaunchPoint.position).normalized;

        if (Physics.Raycast(projectileLaunchPoint.position, aimDirection, out RaycastHit hit, 1000f))
        {
            direction = (hit.point - projectileLaunchPoint.position).normalized;
        }
        else
        {
            direction = aimDirection;
        }

        objectPool.SpawnProjectile(railCannonProjectilePrefab, projectileLaunchPoint.position, Quaternion.LookRotation(direction));
        emitter.PlayFollow("Rail_Cannon_Fire", playerTransform);
        cameraShake?.AddRecoil(new Vector2(Random.Range(-300f, 300f), 1000f));
        isInAftereffect = true;
    }
}
