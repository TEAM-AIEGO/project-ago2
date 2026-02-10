using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RailCannon : SubWeapon
{
    [SerializeField] private SFXEmitter emitter;
    [SerializeField] private Projectile railCannonProjectilePrefab;
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

        List<GameObject> duplicationObjs = new();
        RaycastHit[] hitInfo = Physics.RaycastAll(aimRay, 1000, LayerMask.GetMask("Enemy", "Hittable", "Ground"));

        for (int i = 0; i < hitInfo.Length; i++)
        {
            if (duplicationObjs.Contains(hitInfo[i].transform.gameObject))
                continue;
            else
                duplicationObjs.Add(hitInfo[i].transform.gameObject);

            var currentObject = hitInfo[i].collider.gameObject;

            if (currentObject.TryGetComponent(out GrenadeProjectile grenadeProjectile))
            {
                grenadeProjectile.OnExplosion(true);
                continue;
            }

            if (currentObject.TryGetComponent(out EnemyGrenadeProjectile enemyGrenadeProjectile))
            {
                enemyGrenadeProjectile.OnExplosion();
                continue;
            }

            Transform t = currentObject.transform;

            for (int j = 0; j <= 2 && t != null; j++)
            {
                if (t.TryGetComponent(out IHittable hittable))
                {
                    // if (duplicationObjs.Contains(t.gameObject))
                    //     break;

                    print(damage);
                    hittable.TakeDamage(damage);
                    uiManager?.ShowHitMarker();
                    duplicationObjs.Add(t.gameObject);
                    break;
                }

                t = t.parent;
            }

            //if (currentObject.TryGetComponent(out IHittable hittable))
            //{
            //    hittable.TakeDamage(99f);
            //    uiManager?.ShowHitMarker();
            //}

            if (currentObject.TryGetComponent(out IKnockable knockable))
            {
                knockable.TakeKnockback(aimRay.direction * 60, 0.5f);
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

        //objectPool.SpawnProjectile(railCannonProjectilePrefab, projectileLaunchPoint.position, Quaternion.LookRotation(direction), 00);
        emitter.PlayFollow("Rail_Cannon_Fire", playerTransform);
        cameraShake?.AddRecoil(new Vector2(Random.Range(-300f, 300f), 1000f));
        isInAftereffect = true;
    }
}
