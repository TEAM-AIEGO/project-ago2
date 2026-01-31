using System;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    [SerializeField] int healAmount;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Unit unit))
        {
            unit.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
