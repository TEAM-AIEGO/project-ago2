using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour, IHittable
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float health = 100f;
    [HideInInspector] public UnityEvent Died;
    protected bool isDead;

    protected virtual void OnEnable()
    {
        isDead = false;
    }

    protected virtual void Update()
    {
        if (!isDead && health <= 0)
        {
            isDead = true;
            Died?.Invoke();
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
    }

    public virtual void Heal(float HealAmount)
    {
        health += HealAmount;
    }
}
