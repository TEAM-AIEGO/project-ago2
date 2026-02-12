using UnityEngine;
using System;

public class LaserTurret : TurretBase
{
    [HideInInspector] public event Action<Projectile, Vector3, Quaternion, float, float> OnShootProjectile;

    [Header("Laserrrr Setting")]
    [SerializeField] private LineRenderer laserLineRenderer;
    [SerializeField] private LayerMask laserMask;
    [SerializeField] private float ziiiiingDistance;
    [SerializeField] private float ziiiiingDuration;
    [SerializeField] private float ziiiiingFire;
    [SerializeField] private float ziiiiingTimer;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform targetTransform;

    private Vector3 targetVector;
    private bool isZiiiiingStart;
    private bool isZiiiiingEnd;

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        laserLineRenderer.positionCount = 2;
        laserLineRenderer.enabled = false;

        muDAMUDAMUDAStrategy = new GermanysTechnologyStrategy(projectileSpeed, attackDamage, 0f, ShootPoint, projectilePrefab, OnShootProjectile);
        muKatteKuruNoKaStrategy = new DontMuKatteKuruNoKaStrategy();
    }

    protected override void Update()
    {
        if (!isDead && health <= 0)
        {
            isDead = true;
            Died?.Invoke();
        }

        if (health <= 0) // idk how
        {
            enabled = false;
            return;
        }

        switch (state)
        {
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.moving:
                //Debug.Log("Enemy is moving");
                if (knockbackStun <= 0)
                {
                    if (muKatteKuruNoKaStrategy is DontMuKatteKuruNoKaStrategy)
                        LookTarget();
                    Moving();
                }
                else
                {
                    //Debug.Log(knockbackStun);
                    knockbackStun = Mathf.Max(0, knockbackStun - Time.deltaTime);
                }
                AttackCheck();
                break;
            case EnemyState.attacking:
                LookTarget();

                if (isZiiiiingEnd)
                {
                    laserLineRenderer.enabled = false;
                    Attacking();
                }
                break;
        }

        if (attackTime < attackCooldown)
        {
            attackTime += Time.deltaTime;

            canAttack = false;
        }
        else
        {
            canAttack = true;
        }

        if (isZiiiiingStart)
        {
            if (ziiiiingTimer >= ziiiiingDuration)
            {
                if (ziiiiingTimer >= ziiiiingFire)
                {
                    isZiiiiingStart = false;
                    isZiiiiingEnd = true;
                    ziiiiingTimer = 0;
                }
            }
            else
            {
                targetVector = player.transform.position -  player.transform.right.normalized/3; //dont ask why. this has weird offset thing. PLZ CHANGE THIS TO RAYCAST PLZ
            }

            ziiiiingTimer += Time.deltaTime;
            Ziiiiing();
        }
    }

    private void Ziiiiing()
    {
        Vector3 start = ShootPoint.position;
        Vector3 direction = (targetVector - ShootPoint.position).normalized;
        Vector3 end = start + direction * ziiiiingDistance;

        targetTransform.position = end;

        if (Physics.Raycast(start, direction, out RaycastHit hitInfo, ziiiiingDistance, laserMask, QueryTriggerInteraction.Ignore))
        {
            end = hitInfo.point;
            targetTransform.position = hitInfo.point;
        }

        laserLineRenderer.startColor = Color.red;
        laserLineRenderer.endColor = Color.red;
        laserLineRenderer.SetPosition(0, start);
        laserLineRenderer.SetPosition(1, end);
    }

    protected override void AttackCheck()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < AttackDictance)
        {
            if (!canAttack)
                return;
            canAttack = false;
            attackTime = 0;

            isZiiiiingStart = true;
            laserLineRenderer.enabled = true;
            state = EnemyState.attacking;
        }
    }

    protected override void Attacking()
    {
        muDAMUDAMUDAStrategy.Attacking(this, targetTransform);
        isZiiiiingEnd = false;
        state = EnemyState.moving;
    }

    protected override void Dead()
    {
        laserLineRenderer.enabled = false;
        base.Dead();
    }
}