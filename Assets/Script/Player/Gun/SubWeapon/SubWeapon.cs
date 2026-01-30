using UnityEngine;
using UnityEngine.Events;

public abstract class SubWeapon : MonoBehaviour
{
    protected UnityEvent onSubWeaponUseComplete;

    [SerializeField] protected GameObject subWeaponObj;
    [SerializeField] protected float cooldownTime;

    protected float cooldownTimer;
    protected bool isReady = true;
    public bool IsReady => isReady;

    protected virtual void Awake()
    {
        cooldownTimer = 0f;
    }
    
    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                isReady = true;
            }
        }
    }

    public abstract void Initialize(UnityEvent completeEvent);

    public abstract void Use();
}
