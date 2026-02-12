using UnityEditor;
using UnityEngine;

enum MeleeState
{
    idle,
    moving,
    attacking
}

[RequireComponent(typeof(Rigidbody))]
public class MeleeEnemy : EnemyBase
{
    [SerializeField] private Vector3 attackVectorRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private HitFlash hitFlash;

    public override void Initialize(EnemyBase origin, int warpStage)
    {
        base.Initialize(origin, warpStage);

        muDAMUDAMUDAStrategy = new MeleeAttackStrategy(attackVectorRange, attackPoint);
        muKatteKuruNoKaStrategy = new MuKatteKuruNoKaStrategy();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void Idle()
    {
        base.Idle();
    }

    protected override void Moving()
    {
        //Debug.Log("Melee Enemy is Moving");
        //Debug.Log(player.transform.position);
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

    protected override void BeginAttack()
    {
        base.BeginAttack();
        rb.linearVelocity = Vector3.zero;
    }

    public override void OnAttack()
    {
        base.OnAttack();
    }

    protected override void Attacking()
    {
        Debug.Log("Melee Enemy is Attacking");
        muDAMUDAMUDAStrategy.Attacking(this, player.transform);
    }

    public override void OnAttackEnd()
    {
        base.OnAttackEnd();
    }

    public override void TakeDamage(float damage)
    {
        if (hitFlash)
        {
            hitFlash.Flash();
        }

        base.TakeDamage(damage);
    }

    public override void OnWarpStageChanged(int newStage)
    {
        base.OnWarpStageChanged(newStage);

        if (textures.Length == 0) return;
        switch (newStage)
        {
            case 0:
                up.material = textures[0];
                down.material = textures[1];
                break;
            case 1:
                up.material = textures[2];
                down.material = textures[3];
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(attackPoint.position, attackVectorRange);
    }
}