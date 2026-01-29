using UnityEngine;

public class RailCannon : SubWeapon
{
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private GameObject railCannonProjectilePrefab;
    [SerializeField] private Transform projectileLunchPoint;

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
            }
        }
    }

    protected override void Initialize()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        if (!uiManager)
        {
            Debug.LogWarning("UIManager not found! Hitmarker will not show.");
        }

        delayTimer = 0f;
        isLunching = false;
        isInAftereffect = false;
    }

    public override void Use()
    {
        isLunching = true;
    }

    private void Fire()
    {
        Vector3 direction = Vector3.zero;

        Ray aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;

        if (Physics.Raycast(aimRay, out RaycastHit hitinfo, 1000, LayerMask.GetMask("Enemy")))
        {
            hitinfo.collider.TryGetComponent<MeleeEnemy>(out var hitbox);
            if (hitbox != null)
            {
                hitinfo.collider.GetComponent<MeleeEnemy>().TakeDamage(60f);
                uiManager.ShowHitMarker();
            }

            aimPoint = hitinfo.point;
        }
        else
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

        objectPool.ProjectileRequest.Invoke(railCannonProjectilePrefab.GetComponent<Projectile>(), projectileLunchPoint.position, Quaternion.LookRotation(direction));
    }
}
