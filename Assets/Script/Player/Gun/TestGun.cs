using UnityEngine;

public class TestGun : MonoBehaviour // 이 클래스 기능을 베이스로 추상 클래스를 만들어야함.
{
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

    void Update()
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
        Vector3 direction = Vector3.zero;

        Ray aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;

        if (Physics.Raycast(aimRay, out RaycastHit hitinfo, 1000, layerMasks))
        {
            hitinfo.collider.TryGetComponent<MeleeEnemy>(out var hitbox);
            if (hitbox != null)
            {
                hitinfo.collider.GetComponent<MeleeEnemy>().TakeDamage(10f);
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

        objectPool.ProjectileRequest.Invoke(projectile.GetComponent<Projectile>(), projectileLunchPoint.position, Quaternion.LookRotation(direction));

        isFireAble = false;
    }
}