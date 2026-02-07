using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class BurstTurret : TurretBase
{
    [HideInInspector] public event Action<Projectile, Vector3, Quaternion, float, float> OnShootProjectile;

    [Header("Burst Settings")]
    [SerializeField] private int burstCount = 8;
    [SerializeField] private float burstInterval = 0.1f;
    [SerializeField] private float projectileSpeed = 40f;
    [SerializeField] private float maxLeadTime = 2f;

    [SerializeField] private Projectile projectilePrefab;
    public Projectile ProjectilePrefab => projectilePrefab;

    private int burstShotsRemaining;
    private float burstTimer;
    private bool isBursting;

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        muDAMUDAMUDAStrategy = new GermanysTechnologyStrategy(projectileSpeed, attackDamage, maxLeadTime, ShootPoint, projectilePrefab, OnShootProjectile);
        muKatteKuruNoKaStrategy = new DontMuKatteKuruNoKaStrategy();
    }

    protected override void Update()
    {
        base.Update();
        HandleBurstFire();
    }

    protected override void Attacking()
    {
        if (!isBursting)
        {
            isBursting = true;
            burstShotsRemaining = Mathf.Max(1, burstCount);
            burstTimer = 0f;
        }

        attackTime = 0f;
    }

    private void HandleBurstFire()
    {
        if (!isBursting)
            return;

        if (!EnsurePlayer() || muDAMUDAMUDAStrategy == null)
        {
            isBursting = false;
            return;
        }

        if (burstShotsRemaining <= 0)
        {
            isBursting = false;
            return;
        }

        burstTimer -= Time.deltaTime;

        if (burstTimer > 0f)
            return;

        muDAMUDAMUDAStrategy.Attacking(this, player.transform);
        burstShotsRemaining--;

        if (burstShotsRemaining <= 0)
        {
            isBursting = false;
            return;
        }

        burstTimer = Mathf.Max(0f, burstInterval);
    }

    private void OnDisable()
    {
        isBursting = false;
        burstShotsRemaining = 0;
        burstTimer = 0f;
    }
}
