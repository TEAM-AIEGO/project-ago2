using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    [SerializeField] private Transform playerTransfrom;
    [SerializeField] private SFXEmitter emitter;
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileLunchPoint;
    [SerializeField] private float fireInterval;
    [SerializeField] private float fireTime;
    [SerializeField] private bool isFireAble;
    public bool IsFireAble => isFireAble;

    [SerializeField] private LayerMask layerMasks;

    private UIManager uiManager;

    private void OnEnable()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        if (!uiManager)
        {
            Debug.LogWarning("UIManager not found! Hitmarker will not show.");
        }
        fireTime = fireInterval;
        isFireAble = true;
    }

    private void Update()
    {
        DebugLay();

        if (!IsFireAble)
        {
            fireTime += Time.deltaTime;
        }

        if (fireTime >= fireInterval)
        {
            fireTime = 0f;
            isFireAble = true;
        }
    }

    private void DebugLay()
    {
        Ray aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Debug.DrawRay(aimRay.origin, aimRay.direction * 1000f, Color.blue);

        Vector3 aimPoint;

        if (Physics.Raycast(aimRay, out RaycastHit hitinfo, 1000))
        {
            aimPoint = hitinfo.point;
        }
        else
        {
            aimPoint = Camera.main.transform.position + Camera.main.transform.forward * 1000f;
        }

        Vector3 aimDirection = (aimPoint - projectileLunchPoint.position).normalized;

        Debug.DrawRay(projectileLunchPoint.position, aimDirection * 1000f, Color.red);
    }

    public void Fire()
    {
        Vector3 direction;

        Ray aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;

        if (Physics.Raycast(aimRay, out RaycastHit hitinfo, 1000, layerMasks))
        {
            hitinfo.collider.TryGetComponent<EnemyBase>(out var enemyBase);
            if (enemyBase != null)
            {
                enemyBase.TakeDamage(10f);
                uiManager.ShowHitMarker();
            }

            if (hitinfo.collider.TryGetComponent(out IKnockable knockable))
            {
                knockable.TakeKnockback(aimRay.direction * 25, 0.05f);
            }

            if (hitinfo.collider.TryGetComponent(out GrenadeProjectile grenadeProjectile))
            {
                grenadeProjectile.OnExplosion();
            }

            if (hitinfo.collider.TryGetComponent(out EnemyGrenadeProjectile enemyGrenadeProjectile))
            {
                enemyGrenadeProjectile.OnExplosion();
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

        objectPool.SpawnProjectile(projectile.GetComponent<Projectile>(), projectileLunchPoint.position, Quaternion.LookRotation(direction), 300f);

        emitter.PlayFollow(AudioIds.WeaponRifleShot, playerTransfrom);
        isFireAble = false;
    }
}
