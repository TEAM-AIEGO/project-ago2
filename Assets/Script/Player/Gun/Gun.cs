using UnityEngine;

[System.Serializable]
public struct GunStats
{
    public float FireInterval;
    public float Damage;
    public float GunRange;
    public float XRecoil;
    public float YRecoil;
    public Transform projectileLaunchPoint;
    public float HitForce;
    public GameObject GunModel;
}

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransfrom;
    [SerializeField] private SFXEmitter emitter;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private GameObject projectile;
    [SerializeField] private LayerMask layerMasks;

    [Header("Gun Settings")]
    public GunStats[] stats;
    private GunStats currentStats;
    private GameObject currentGunModel;

    #region Gun Properties
    private float damage;
    private float gunRange;
    private float xRecoil;
    private float yRecoil;
    private float hitForce;
    private float fireInterval;
    private float fireTime;
    private bool isFireAble;
    public bool IsFireAble => isFireAble;
    private Transform projectileLaunchPoint;
    #endregion

    private UIManager uiManager;

    private void OnEnable()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        if (!uiManager)
        {
            Debug.LogWarning("UIManager not found! Hitmarker will not show.");
        }
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

        if (transform.localRotation.eulerAngles .z > .2)
        {
            transform.localRotation = Quaternion.Euler(0, -90, Mathf.LerpAngle(transform.localRotation.eulerAngles.z, 0, Time.deltaTime * 10));
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
    }

    public void OnGunChange(int gunIndex)
    {
        switch (gunIndex)
        {
               case 0:
                currentStats = stats[0];
                Initialize();
                break;
            case 1:
                currentStats = stats[1];
                Initialize();
                break;
            default:
                currentStats = stats[0];
                Initialize();
                break;
        }
    }

    private void Initialize()
    {
        if (currentGunModel != null)
        {
            currentGunModel.SetActive(false);
        }

        fireInterval = currentStats.FireInterval;
        damage = currentStats.Damage;
        gunRange = currentStats.GunRange;
        xRecoil = currentStats.XRecoil;
        yRecoil = currentStats.YRecoil;
        hitForce = currentStats.HitForce;
        projectileLaunchPoint = currentStats.projectileLaunchPoint;
        currentGunModel = currentStats.GunModel;

        currentStats.GunModel.SetActive(true);

        fireTime = fireInterval;
        isFireAble = true;
    }

    private void DebugLay()
    {
        Ray aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Debug.DrawRay(aimRay.origin, aimRay.direction * gunRange, Color.blue);

        Vector3 aimPoint;

        if (Physics.Raycast(aimRay, out RaycastHit hitinfo, gunRange))
        {
            aimPoint = hitinfo.point;
        }
        else
        {
            aimPoint = Camera.main.transform.position + Camera.main.transform.forward * gunRange;
        }

        Vector3 aimDirection = (aimPoint - projectileLaunchPoint.position).normalized;

        Debug.DrawRay(projectileLaunchPoint.position, aimDirection * gunRange, Color.red);
    }

    public void Fire()
    {
        transform.localRotation = Quaternion.Euler(0, -90, 15);
        Vector3 direction;

        Ray aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;

        if (Physics.Raycast(aimRay, out RaycastHit hitinfo, gunRange, layerMasks))
        {
            //Transform t = hitinfo.collider.transform;

            //for (int i = 0; i <= 2 && t != null; i++)
            //{
                if (hitinfo.collider.transform.TryGetComponent<IHittable>(out var hittable))
                {
                    hittable.TakeDamage(damage);
                    uiManager?.ShowHitMarker();
                    //break;
                }

                //t = t.parent;
            //}
            //if (hitinfo.collider.TryGetComponent(out IHittable hittable))
            //{
            //    hittable.TakeDamage(damage);
            //    uiManager?.ShowHitMarker();
            //}

            if (hitinfo.collider.TryGetComponent(out IKnockable knockable))
            {
                knockable.TakeKnockback(aimRay.direction * hitForce, 0.25f);
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
            aimPoint = Camera.main.transform.position + Camera.main.transform.forward * gunRange;
        }

        Vector3 aimDirection = (aimPoint - projectileLaunchPoint.position).normalized;

        if (Physics.Raycast(projectileLaunchPoint.position, aimDirection, out RaycastHit hit, gunRange))
        {
            direction = (hit.point - projectileLaunchPoint.position).normalized;
        }
        else
        {
            direction = aimDirection;
        }

        objectPool.SpawnProjectile(projectile.GetComponent<Projectile>(), projectileLaunchPoint.position, Quaternion.LookRotation(direction), 300f);
        cameraShake?.AddRecoil(new Vector2(Random.Range(-xRecoil, xRecoil), yRecoil));

        emitter.PlayFollow("Gun_Shot", playerTransfrom);
        isFireAble = false;
    }
}