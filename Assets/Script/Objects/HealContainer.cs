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
        Instantiate(healObj, transform.position, transform.rotation);
    }
}
