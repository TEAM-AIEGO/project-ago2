using UnityEngine;
using UnityEngine.Events;

public abstract class SubWeapon : MonoBehaviour
{
    protected UnityEvent onSubWeaponUseComplete;

    [Header("References")]
    [SerializeField] protected ObjectPool objectPool;
    [SerializeField] protected Transform playerTransform;
    [SerializeField] protected CameraShake cameraShake;

    [Header("SubWeapon Stats")]
    [SerializeField] protected float damage;
    [SerializeField] protected float cooldownTime;
    [SerializeField] protected float launchDelayTime;
    [SerializeField] protected float launchAftereffectTime;
    [SerializeField] protected float delayTimer;
    [SerializeField] protected GameObject subWeaponObj;

    protected bool isLaunching;
    protected bool isInAftereffect;

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
            Debug.Log("SubWeapon Cooldown: " + cooldownTimer);
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                isReady = true;
            }
        }

        if (isLaunching)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= launchDelayTime)
            {
                Fire();
                isLaunching = false;
                isInAftereffect = true;
                delayTimer = 0f;
            }
            return;
        }

        if (isInAftereffect)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= launchAftereffectTime)
            {
                isInAftereffect = false;
                delayTimer = 0f;
                cooldownTimer = cooldownTime;

                subWeaponObj.SetActive(false);
                onSubWeaponUseComplete.Invoke();
            }
        }
    }

    public abstract void Initialize(UnityEvent completeEvent);

    public abstract void Use();

    protected abstract void Fire();
}
