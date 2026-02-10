using UnityEngine;

public class PlayerManager : Unit, IStat
{
    #region IStat
    public float CurrentValue => health;
    public float MaxValue => maxHealth;
    public event System.Action<float, float> OnValueChanged;
    #endregion

    private GameManager gameManager;
    [SerializeField] private SFXEmitter emitter;
    private void Awake()
    {
        OnValueChanged?.Invoke(health, maxHealth);
    }
    
    protected override void Update()
    {
        ChangeWarpStage();
    }

    void ChangeWarpStage()
    {
        if (gameManager)
        {
            if (health >= 0 && health <= 100)
            {
                gameManager.WarpStage = 0;
            }
            else if (health > 100 && health < 200)
            {
                gameManager.WarpStage = 1;
            }
            else
            {
                gameManager.WarpStage = 2;
            }
        }
        else
        {
            gameManager = FindFirstObjectByType<GameManager>();
            if (!gameManager)
            {
                Debug.LogError("GameManager not found! Warping will not work. Be sure to add one.");
            }
        }
        
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);

        emitter.PlayFollow("hurt", transform);

        OnValueChanged?.Invoke(health, maxHealth);
        Debug.Log($"Player took {damageAmount} damage. Remaining Health: {health}");
    }

    public override void Heal(float HealAmount)
    {
        base.Heal(HealAmount);

        OnValueChanged?.Invoke(health, maxHealth);
        Debug.Log($"Player healed {HealAmount} damage. Remaining Health: {health}");
    }
}