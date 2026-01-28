using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float Health = 100f;
    public UnityEvent Died; 

    protected virtual void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
            Died?.Invoke();
        }
    }
}
