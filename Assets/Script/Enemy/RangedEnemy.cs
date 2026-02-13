using System;
using UnityEngine;

enum RangedState
{
    idle,
    moving,
    attacking
}

[RequireComponent(typeof(Rigidbody))]
public class RangedEnemy : EnemyBase
{
    [HideInInspector] public event Action<Projectile, Vector3, Quaternion, float, float> OnShootProjectile;

    [SerializeField] private MeshRenderer rUp;
    [SerializeField] private MeshRenderer rDown;
    [SerializeField] private Transform ShootPoint;
    [SerializeField] private float projectileSpeed = 40f;
    [SerializeField] private float maxLeadTime = 2f;
    [SerializeField] private HitFlash hitFlash;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject firstBody;
    [SerializeField] private GameObject secondBody;

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        muDAMUDAMUDAStrategy = new GermanysTechnologyStrategy(projectileSpeed, attackDamage, maxLeadTime, ShootPoint, projectilePrefab, OnShootProjectile);
        muKatteKuruNoKaStrategy = new DontMuKatteKuruNoKaStrategy();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void LookTarget()
    {
        base.LookTarget();

        Vector3 direction = (player.transform.position - head.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(head.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    protected override void Idle()
    {
        base.Idle();
    }

    protected override void Moving()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
        {
            
            emitter?.Play(AudioIds.RobotTurretRobotMoving, false, 0.2f, 0.6f);
            footstepTimer = interval;
        }
        muKatteKuruNoKaStrategy.KonoDIOniMuKatteKuruNoKa(this, player.transform);
    }

    protected override void AttackCheck()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < AttackDictance)
        {
            if (!canAttack)
                return;

            state = EnemyState.attacking;
        }
    }

    protected override void Attacking()
    {
        emitter.PlayFollow(AudioIds.RobotTurretRobotShot, transform, false, 0.4f, 0.7f);
        muDAMUDAMUDAStrategy.Attacking(this, player.transform);
        attackTime = 0f;
    }

    public override void OnWarpStageChanged(int newStage)
    {
        base.OnWarpStageChanged(newStage);

        if (textures.Length == 0) return;
        switch (newStage)
        {
            case 0:
                muKatteKuruNoKaStrategy = new DontMuKatteKuruNoKaStrategy();
                secondBody.SetActive(false);
                firstBody.SetActive(true);
                rUp.material = textures[0];
                rDown.material = textures[0];
                break;
            case 1:
                muKatteKuruNoKaStrategy = new MuKatteKuruNoKaStrategy();
                secondBody.SetActive(true);
                firstBody.SetActive(false);
                rUp.material = textures[1];
                rDown.material = textures[1];
                break;
            default:
                Debug.LogError("Invalid warp stage");
                break;
        }
    }

    public override void TakeDamage(float damage)
    {
        if (hitFlash)
        {
            hitFlash.Flash();
        }

        base.TakeDamage(damage);
    }
}
