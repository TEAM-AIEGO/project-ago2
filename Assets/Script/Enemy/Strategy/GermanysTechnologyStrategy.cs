using UnityEngine;

public class GermanysTechnologyStrategy : IMUDAMUDAMUDAStrategy
{
    private float projectileSpeed = 50f;
    private float maxLeadTime = 2f;
    private Transform shooter;
    private float damage;
    private Projectile projectilePrefab;
    private System.Action<Projectile, Vector3, Quaternion, float, float> OnShootProjectile;

    public GermanysTechnologyStrategy(float projectileSpeed, float damage, float maxLeadTime, Transform shooter, Projectile projectilePrefab, System.Action<Projectile, Vector3, Quaternion, float, float> shootAction)
    {
        this.projectileSpeed = projectileSpeed;
        this.maxLeadTime = maxLeadTime;
        this.shooter = shooter;
        this.damage = damage;
        this.projectilePrefab = projectilePrefab;
        OnShootProjectile += shootAction;
    }

    public void Attacking(EnemyBase enemy, Transform target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (projectilePrefab == null)
            return;

        Vector3 targetVelocity = rb.linearVelocity;

        if (TryGetLeadDirection(shooter, target, targetVelocity, 50f, out Vector3 leadDirection))
        {
            Quaternion shootRotation = Quaternion.LookRotation(leadDirection, Vector3.up);
            OnShootProjectile?.Invoke(projectilePrefab, shooter.position, shootRotation, projectileSpeed, damage);
        }
        else
        {
            leadDirection = (target.position - shooter.position).normalized;

            Quaternion shootRotation = Quaternion.LookRotation(leadDirection, Vector3.up);
            OnShootProjectile?.Invoke(projectilePrefab, shooter.position, shootRotation, projectileSpeed, damage);
        }
    }

    private bool TryGetLeadDirection(Transform shooter, Transform target, Vector3 targetVelocity, float projectileSpeed, out Vector3 leadDirection, float maxLeadTime = 2f)
    {
        leadDirection = Vector3.zero;

        Vector3 toTarget = target.position - shooter.position;

        float v2 = Vector3.Dot(targetVelocity, targetVelocity);
        float s2 = projectileSpeed * projectileSpeed;

        float a = v2 - s2;
        float b = 2f * Vector3.Dot(targetVelocity, toTarget);
        float c = Vector3.Dot(toTarget, toTarget);

        const float epsilon = 1e-6f;

        if (Mathf.Abs(a) < epsilon)
        {
            if (Mathf.Abs(b) < epsilon)
            {
                if (toTarget.sqrMagnitude > epsilon)
                    leadDirection = toTarget.normalized;

                return false;
            }

            float time = -c / b;
            if (time <= 0f)
                return false;

            time = Mathf.Min(time, maxLeadTime);

            Vector3 aimPoint = target.position + targetVelocity * time;
            Vector3 direction = aimPoint - shooter.position;

            if (direction.sqrMagnitude < epsilon)
                return false;

            leadDirection = direction.normalized;
            return true;
        }

        float discriminant = b * b - 4f * a * c;

        if (discriminant < 0f)
            return false;

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);

        float t1 = (-b - sqrtDiscriminant) / (2f * a);
        float t2 = (-b + sqrtDiscriminant) / (2f * a);

        float t = float.PositiveInfinity;

        if (t1 > 0f)
            t = t1;

        if (t2 > 0f)
            t = Mathf.Min(t, t2);

        if (!float.IsFinite(t))
            return false;

        t = Mathf.Min(t, maxLeadTime);

        Vector3 predicted = target.position + targetVelocity * t;
        Vector3 aimDir = predicted - shooter.position;

        if (aimDir.sqrMagnitude < epsilon)
            return false;

        leadDirection = aimDir.normalized;

        return true;
    }
}