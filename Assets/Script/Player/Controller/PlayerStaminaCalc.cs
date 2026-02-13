using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerStaminaCalc : MonoBehaviour, IStat
{
    private PlayerController playerController;
    private SFXEmitter emitter;

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
    [SerializeField] private float StaminaRecoveryReadyDelayTime;
    private bool isAudioPlay;
    private float StaminaRecoveryReadyTime;
    private void Awake()
    {
        OnValueChanged?.Invoke(CurrentStamina, MaxStamina);
        emitter = GetComponent<SFXEmitter>();
    }

    public void Initialized(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void RegenerateStamina()
    {
        if (CurrentStamina < MaxStamina && Time.time >= StaminaRecoveryReadyTime)
        {
            if (!isAudioPlay)
            {
                emitter.PlayFollow(AudioIds.PlayerRecovery, transform, false, 0.1f, 1f);
                isAudioPlay = true;
            }
            //Debug.Log("Regenerating Stamina");
            CurrentStamina += StaminaRegenSpeed * Time.deltaTime;
            CurrentStamina = Mathf.Min(CurrentStamina, MaxStamina);
            OnValueChanged?.Invoke(CurrentStamina, MaxStamina);
            if (playerController.CannotSprint)
                if (CurrentStamina >= MaxStamina / 5.5)
                {
                    //Debug.Log("Stamina Sufficient - Can Sprint Again");
                    playerController.DeshAble(true);
                }
        }
    }

    public void ConsumeStamina()
    {
        isAudioPlay = false; 
        //Debug.Log("Consuming Stamina");
        CanRecoverStamina();
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

    void CanRecoverStamina()
    {
        StaminaRecoveryReadyTime = Time.time + StaminaRecoveryReadyDelayTime;
    }
}
