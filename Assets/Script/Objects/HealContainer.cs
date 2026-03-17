using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HealContainer : BreakableObject
{
    [SerializeField] private GameObject healObj;

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (health <= 0)
        {
            HealObjSpawn();
        }
    }

    private void HealObjSpawn()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = transform.position.y + 0.3f;
        Instantiate(healObj, spawnPos, transform.rotation);
    }
}
