using Unity.Mathematics;
using UnityEngine;

public class SansBoneWallAttackStrategy : IMUDAMUDAMUDAStrategy
{
    private float projectileSpeed = 50f;
    private float maxLeadTime = 2f;
    private Transform shooter;
    private float damage;
    private Projectile projectilePrefab;
    private System.Action<Projectile, Vector3, Quaternion, float, float> OnShootProjectile;

    public SansBoneWallAttackStrategy(float projectileSpeed, float damage, float maxLeadTime, Transform shooter, Projectile projectilePrefab, System.Action<Projectile, Vector3, Quaternion, float, float> shootAction)
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
        Vector3 randomPosition = shooter.position + new Vector3(UnityEngine.Random.Range(0, 2) == 1 ? 100 : -100, UnityEngine.Random.Range(-3f, 4f), UnityEngine.Random.Range(0, 2) == 1 ? 100 : -100);
        Vector3 relative = target.position - randomPosition;
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);


        OnShootProjectile?.Invoke(projectilePrefab, shooter.position + randomPosition, rotation, projectileSpeed, damage);
    }
}