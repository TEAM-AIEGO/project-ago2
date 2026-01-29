using UnityEngine;

public abstract class SubWeapon : MonoBehaviour
{
    [SerializeField] protected float cooldownTime;

    protected float cooldownTimer;
    protected bool isReady = true;

    protected virtual void Awake()
    {
        cooldownTimer = 0f;

        Initialize();
    }
    
    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    protected abstract void Initialize();

    public abstract void Use();
}
