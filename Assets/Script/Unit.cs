using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float health = 100f;
    [HideInInspector] public UnityEvent Died; 

    protected virtual void Update()
    {
        if (health <= 0)
        {
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
