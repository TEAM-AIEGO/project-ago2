using UnityEngine;

public class PlayerStaminaCalc : MonoBehaviour
{
    private PlayerController playerController;

    [Header("Stamina Settings")]
    [SerializeField] private float CurrentStamina;
    [SerializeField] private float MaxStamina;
    [SerializeField] private float StaminaRegenSpeed;
    [SerializeField] private float SprintStaminaCost;

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
                    playerController.SprintAble(true);
                }
        }
    }

    public void ConsumeStamina()
    {
        //Debug.Log("Consuming Stamina");
        CurrentStamina -= Time.deltaTime * SprintStaminaCost;
        CurrentStamina = Mathf.Max(CurrentStamina, 0);
        //Debug.Log("Current Stamina: " + CurrentStamina);

        if (CurrentStamina == 0)
        {
            Debug.Log("Stamina Depleted");
            playerController.SprintAble(false);
        }
    }
}
