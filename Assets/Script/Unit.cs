using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float health = 100f;
    public UnityEvent Died; 

    protected virtual void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            Died?.Invoke();
        }
    }

    public virtual void Heal(int HealAmount)
    {
        health += HealAmount;
    }
}
