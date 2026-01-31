using UnityEngine;

public class PlayerController : MonoBehaviour, IWarpObserver
{
    [SerializeField] private float walkingSpeedStage1 = 6.0f;
    [SerializeField] private float sprintingSpeedStage1 = 8.0f;
    [SerializeField] private float walkingSpeedStage2 = 10.0f;
    [SerializeField] private float sprintingSpeedStage2 = 16.0f;
    [SerializeField] private float walkingSpeedStage3 = 18.0f;
    [SerializeField] private float sprintingSpeedStage3 = 30.0f;

    [Header("Warp System Manager")]
    [SerializeField] private WarpSystemManager warpSystemManager;

    #region Components
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerJumpAction PlayerJumpAction { get; private set; }
    public PlayerGroundPoundAction PlayerGroundPoundAction { get; private set; }
    public PlayerStaminaCalc PlayerStaminaCalc { get; private set; }
    public PlayerSlideAction PlayerSlideAction { get; private set; }
    public PlayerDashAction PlayerDashAction { get; private set; }
    public PlayerGroundChecker PlayerGroundChecker { get; private set; }
    #endregion

    private PlayerStateMachine playerStateMachine;
    public PlayerStateMachine PlayerStateMachine => playerStateMachine;

    private PlayerManager playerManager;
    private Collider col;
    private Rigidbody rb;

    private Vector3 movement;
    public Vector3 Movement => movement;

    private bool isGrounded;
    public bool IsGrounded => isGrounded;

     private float nextDeshTime;
    [SerializeField] private float deshCoolDown;

    [Header("Main Camera Transform")]
    [SerializeField] private Transform cam;

    private bool IsDeshing;
    private bool cannotSprint = false;
    public bool CannotSprint => cannotSprint;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (!gameManager)
        {
            Debug.LogError("GameManager not found! Warping will not work. Be sure to add one");
        }

        playerManager = GetComponent<PlayerManager>();
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        Initialized();
    }

    public void Initialized()
    {
        FindComponents();

        warpSystemManager.RegisterWarpObserver(this);

        playerStateMachine = new PlayerStateMachine();
        PlayerMovement.Initialized(cam, rb);
        PlayerGroundPoundAction.Initialized(rb);
        PlayerSlideAction.Initialized(cam, rb);
        PlayerDashAction.Initialized(cam, rb);
        PlayerGroundChecker.Initialized(col);
        PlayerJumpAction.Initialized(rb, PlayerGroundChecker);
        PlayerStaminaCalc.Initialized(this);

        PlayerMovement.SetSpeed(GetSpeed(warpSystemManager.GetWarpStage()));

        playerStateMachine.ChangeState(new PlayerDefaultState(this));
    }

    private void OnDisable()
    {
        warpSystemManager.UnregisterWarpObserver(this);
    }

    private void FindComponents()
    {
        PlayerJumpAction = GetComponent<PlayerJumpAction>();
        PlayerGroundPoundAction = GetComponent<PlayerGroundPoundAction>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerStaminaCalc = GetComponent<PlayerStaminaCalc>();
        PlayerSlideAction = GetComponent<PlayerSlideAction>();
        PlayerDashAction = GetComponent<PlayerDashAction>();
        PlayerGroundChecker = GetComponent<PlayerGroundChecker>();

        if (!PlayerJumpAction) Debug.LogError("PlayerJumpAction component not found!");
        if (!PlayerGroundPoundAction) Debug.LogError("PlayerGroundPoundAction component not found!");
        if (!PlayerMovement) Debug.LogError("PlayerMovement component not found!");
        if (!PlayerStaminaCalc) Debug.LogError("PlayerStaminaCalc component not found!");
        if (!PlayerSlideAction) Debug.LogError("PlayerSlideAction component not found!");
        if (!PlayerDashAction) Debug.LogError("PlayerDashAction component not found!");
        if (!PlayerGroundChecker) Debug.LogError("PlayerGroundChecker component not found!");
    }

    private void Update()
    {
        playerStateMachine?.Update();

        isGrounded = PlayerGroundChecker.IsGrounded();

        if (IsDeshing)
            PlayerStaminaCalc.ConsumeStamina();
        else
            PlayerStaminaCalc.RegenerateStamina();
    }

    public void SetMovement(Vector2 movementInput)
    {
        movement = new Vector3(movementInput.x, 0, movementInput.y);
    }

    public void Desh(bool isDeshing)
    {
        // if (cannotSprint)
        //     return;

        // Debug.Log("Sprint Input Received: " + isSprinting);
        // IsSprinting = isSprinting;

        // PlayerMovement.SetSpeed(GetSpeed(0));

        if (!isDeshing && playerStateMachine.CurrentState is PlayerSlideState or PlayerGroundPoundState) return;
        if (PlayerStaminaCalc.IsStaminaUseable() && CanDesh())
        {
            nextDeshTime = Time.time + deshCoolDown;
            IsDeshing = true;
            playerStateMachine.ChangeState(new PlayerDashState(this));
        }

    }

    public void DeshAble(bool ornot)
    {
        if (!ornot)
        {
            IsDeshing = false;
            PlayerMovement.SetSpeed(GetSpeed(0));
            cannotSprint = true;
        }
        else
        {
            cannotSprint = false;
        }
    }

    private bool CanDesh()
    {
        return Time.time >= nextDeshTime;
    }

    public void Jump(bool isJumping)
    {
        if (playerStateMachine.CurrentState is PlayerDefaultState or PlayerJumpState)
            playerStateMachine.ChangeState(new PlayerJumpState(this, isJumping));

    }

    public void MovementAction1()
    {
        if (playerStateMachine.CurrentState is PlayerDefaultState && isGrounded)
        {
            playerStateMachine.ChangeState(new PlayerSlideState(this));
        }
        else if(!isGrounded && playerStateMachine.CurrentState is not PlayerGroundPoundState)
        {
            playerStateMachine.ChangeState(new PlayerGroundPoundState(this));
        }
    }

    public void OnWarpStageChanged(int newStage)
    {
        PlayerMovement.SetSpeed(GetSpeed(newStage));
    }

    private float GetSpeed(int warpStage)
    {
        // if (IsSprinting)
        // {
        //     return warpStage switch
        //     {
        //         0 => sprintingSpeedStage1,
        //         1 => sprintingSpeedStage2,
        //         //2
        //         _ => sprintingSpeedStage3,
        //     };
        // }
        // else
        // {
        //     return warpStage switch
        //     {
        //         0 => walkingSpeedStage1,
        //         1 => walkingSpeedStage2,
        //         //2
        //         _ => walkingSpeedStage3,
        //     };
        // }

        return warpStage switch
        {
            0 => sprintingSpeedStage1,
            1 => sprintingSpeedStage2,
            //2
            _ => sprintingSpeedStage3,
        };
    }
}
