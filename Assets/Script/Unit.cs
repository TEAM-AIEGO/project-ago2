using UnityEngine;

public class Unit : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float Health = 100f;

    void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
