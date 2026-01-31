using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Events;

public class RailCannon : SubWeapon
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SFXEmitter emitter;
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private Projectile railCannonProjectilePrefab;
    [SerializeField] private Transform projectileLunchPoint;

    [SerializeField] private CameraShake cameraShake;

    [SerializeField] private float lunchDelayTime;
    [SerializeField] private float lunchAftereffectTime;
    [SerializeField] private float delayTimer;

    private bool isLunching;
    private bool isInAftereffect;

    private UIManager uiManager;

    protected override void Update()
    {
        base.Update();

        if (isLunching)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= lunchDelayTime)
            {
                Fire();
                isLunching = false;
                isInAftereffect = true;
                delayTimer = 0f;
            }
            return;
        }

        if (isInAftereffect)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= lunchAftereffectTime)
            {
                isInAftereffect = false;
                delayTimer = 0f;
                cooldownTimer = cooldownTime;

                subWeaponObj.SetActive(false);
                onSubWeaponUseComplete.Invoke();
            }
        }
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
        isLunching = false;
        isInAftereffect = false;
    }

    public override void Use()
    {
        isLunching = true;
        isReady = false;
        subWeaponObj.SetActive(true);
    }

    private void Fire()
    {
        Vector3 direction;

        Ray aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint = Vector3.zero;

        RaycastHit[] hitInfo = Physics.RaycastAll(aimRay, 1000, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < hitInfo.Length; i++)
        {
            var currentEnemyObject = hitInfo[i].collider.gameObject;
            if (currentEnemyObject.TryGetComponent(out Unit unit))
            {
                unit.TakeDamage(9999f);
                uiManager.ShowHitMarker();
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

        Vector3 aimDirection = (aimPoint - projectileLunchPoint.position).normalized;

        if (Physics.Raycast(projectileLunchPoint.position, aimDirection, out RaycastHit hit, 1000f))
        {
            direction = (hit.point - projectileLunchPoint.position).normalized;
        }
        else
        {
            direction = aimDirection;
        }

        objectPool.SpawnProjectile(railCannonProjectilePrefab, projectileLunchPoint.position, Quaternion.LookRotation(direction));
        emitter.PlayFollow("Rail_Cannon_Fire", playerTransform);
        cameraShake?.AddRecoil(new Vector2(Random.Range(-300f, 300f), 1000f));
        isInAftereffect = true;
    }
}
