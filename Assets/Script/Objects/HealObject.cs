using System;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    [SerializeField] int healAmount;
    private SFXEmitter emitter;

    private void Awake()
    {
        emitter = GetComponent<SFXEmitter>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Unit unit))
        {
            emitter.PlayFollow(AudioIds.ObjectItemPickup, transform);
            unit.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
