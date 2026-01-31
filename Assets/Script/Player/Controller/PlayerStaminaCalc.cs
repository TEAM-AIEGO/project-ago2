using UnityEngine;

public class PlayerStaminaCalc : MonoBehaviour
{
    private PlayerController playerController;

    [Header("Stamina Settings")]
    [SerializeField] private float CurrentStamina;
    [SerializeField] private float MaxStamina;
    [SerializeField] private float StaminaRegenSpeed;
    [SerializeField] private float DeshStaminaCost;

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
