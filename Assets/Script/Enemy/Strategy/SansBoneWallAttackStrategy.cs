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
        OnShootProjectile?.Invoke(projectilePrefab, shooter.position, quaternion.identity, projectileSpeed, damage);
    }
}