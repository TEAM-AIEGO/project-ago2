using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour, IHittable
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float health = 100f;
    [HideInInspector] public UnityEvent Died;
    protected bool isDead;

    protected virtual void OnEnable()
    {
        isDead = false;
    }

    protected abstract void Update();

    public virtual void TakeDamage(float damage)
    {
        if (health <= 0)
            return;

        health -= damage;

        if (!isDead && health <= 0)
        {
            isDead = true;
            Died?.Invoke();
        }
    }

    public virtual void Heal(float HealAmount)
    {
        health += HealAmount;
    }
}
