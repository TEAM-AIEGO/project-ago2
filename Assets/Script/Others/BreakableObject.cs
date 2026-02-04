using UnityEngine;

public class BreakableObject : MonoBehaviour, IHittable
{
    [SerializeField] protected float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
