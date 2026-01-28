using UnityEngine;

public class TestGun : MonoBehaviour
{
    public GameObject Projectile;
    public Transform ProjectileLunchPoint;
    public float FireInterval;
    public float FireTime;
    public bool IsFireAble;
    public LayerMask LayerMasks;

    private void OnEnable()
    {
        FireTime = FireInterval;
        IsFireAble = true;
    }

    void Update()
    {
        DebugLay();

        if (!IsFireAble)
        {
            FireTime += Time.deltaTime;
        }

        if (FireTime >= FireInterval)
        {
            FireTime = 0f;
            IsFireAble = true;
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

        Vector3 aimDirection = (aimPoint - ProjectileLunchPoint.position).normalized;

        Debug.DrawRay(ProjectileLunchPoint.position, aimDirection * 1000f, Color.red);
    }

    public void Fire()
    {
        Vector3 direction = Vector3.zero;

        Ray aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;

        if (Physics.Raycast(aimRay, out RaycastHit hitinfo, 1000, LayerMasks))
        {
            hitinfo.collider.TryGetComponent<MeleeEnemy>(out var hitbox);
            if (hitbox != null)
            {
                hitinfo.collider.GetComponent<MeleeEnemy>().TakeDamage(10);
                UIManager.Instance.ShowHitMarker();
            }

            aimPoint = hitinfo.point;
        }
        else
        {
            aimPoint = Camera.main.transform.position + Camera.main.transform.forward * 1000f;
        }

        Vector3 aimDirection = (aimPoint - ProjectileLunchPoint.position).normalized;

        if (Physics.Raycast(ProjectileLunchPoint.position, aimDirection, out RaycastHit hit, 1000f))
        {
            direction = (hit.point - ProjectileLunchPoint.position).normalized;
        }
        else
        {
            direction = aimDirection;
        }

        GameObject newProjectile = Instantiate(Projectile, ProjectileLunchPoint.position, Quaternion.LookRotation(direction));

        IsFireAble = false;
    }
}