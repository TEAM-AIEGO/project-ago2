using UnityEngine;

public class PlayerStaminaCalc : MonoBehaviour, IStat
{
    private PlayerController playerController;

    #region IStat
    public float CurrentValue => CurrentStamina;
    public float MaxValue => MaxStamina;
    public event System.Action<float, float> OnValueChanged;
    #endregion

    [Header("Stamina Settings")]
    [SerializeField] private float CurrentStamina;
    [SerializeField] private float MaxStamina;
    [SerializeField] private float StaminaRegenSpeed;
    [SerializeField] private float DeshStaminaCost;

    private void Awake()
    {
        OnValueChanged?.Invoke(CurrentStamina, MaxStamina);
    }

    public void Initialized(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void RegenerateStamina()
    {
        if (CurrentStamina < MaxStamina)
        {
            //Debug.Log("Regenerating Stamina");
            CurrentStamina += StaminaRegenSpeed * Time.deltaTime;
            CurrentStamina = Mathf.Min(CurrentStamina, MaxStamina);
            OnValueChanged?.Invoke(CurrentStamina, MaxStamina);
            if (playerController.CannotSprint)
                if (CurrentStamina >= MaxStamina / 5.5)
                {
                    Debug.Log("Stamina Sufficient - Can Sprint Again");
                    playerController.DeshAble(true);
                }
        }
    }

    public void ConsumeStamina()
    {
        //Debug.Log("Consuming Stamina");
        CurrentStamina -= DeshStaminaCost;
        
        CurrentStamina = Mathf.Max(CurrentStamina, 0);
        OnValueChanged?.Invoke(CurrentStamina, MaxStamina);
        //Debug.Log("Current Stamina: " + CurrentStamina);
        playerController.DeshAble(false);
        /* if (CurrentStamina == 0)
         {
             Debug.Log("Stamina Depleted");
             playerController.DeshAble(false);
         }*/
    }

    public bool IsStaminaUseable()
    {
        return CurrentStamina >= DeshStaminaCost;
    }
}
