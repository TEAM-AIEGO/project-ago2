using UnityEngine;

public class PlayerController : MonoBehaviour, IWarpObserver
{
    [SerializeField] private float sprintingSpeedStage1 = 8.0f;
    [SerializeField] private float sprintingSpeedStage2 = 16.0f;
    [SerializeField] private float sprintingSpeedStage3 = 30.0f;

    [SerializeField] private float warpStage1ChangeCriteria = 100.0f;
    [SerializeField] private float warpStage2ChangeCriteria = 200.0f;

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

    [Header("Main Camera Transform")]
    [SerializeField] private Transform cam;

    private bool IsSprinting;
    private bool cannotSprint = false;
    public bool CannotSprint => cannotSprint;


    private void Awake()
    {
        if (!warpSystemManager)
        {
            warpSystemManager = FindFirstObjectByType<WarpSystemManager>();
            if (!warpSystemManager)
            {
                Debug.LogError("WarpSystemManager not found! Please ensure there is one in the scene.");
            }
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

        playerManager.OnValueChanged += OnHpChanged;
    }

    private void OnDisable()
    {
        warpSystemManager.UnregisterWarpObserver(this);
        playerManager.OnValueChanged -= OnHpChanged;
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

        if (IsSprinting)
            PlayerStaminaCalc.ConsumeStamina();
        else
            PlayerStaminaCalc.RegenerateStamina();
    }

    public void SetMovement(Vector2 movementInput)
    {
        movement = new Vector3(movementInput.x, 0, movementInput.y);
    }

    public void Sprint(bool isSprinting)
    {
        // if (cannotSprint)
        //     return;

        // Debug.Log("Sprint Input Received: " + isSprinting);
        // IsSprinting = isSprinting;

        // PlayerMovement.SetSpeed(GetSpeed(0));

        if (!isSprinting && playerStateMachine.CurrentState is PlayerSlideState or PlayerGroundPoundState) return;
        playerStateMachine.ChangeState(new PlayerDashState(this));

    }

    public void SprintAble(bool ornot)
    {
        if (!ornot)
        {
            IsSprinting = false;
            PlayerMovement.SetSpeed(GetSpeed(0));
            cannotSprint = true;
        }
        else
        {
            cannotSprint = false;
        }
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

    private void OnHpChanged(float currentHp, float maxHp)
    {
        if (warpSystemManager.GetWarpStage() == 0)
        {
            if (currentHp >= warpStage2ChangeCriteria)
            {
                warpSystemManager.SetChageWarpStage(1);
            }
        }
        else if (warpSystemManager.GetWarpStage() == 1)
        {
            if (currentHp < warpStage1ChangeCriteria)
            {
                warpSystemManager.SetChageWarpStage(0);
            }
        }
    }

    private float GetSpeed(int warpStage)
    {
        return warpStage switch
        {
            0 => sprintingSpeedStage1,
            1 => sprintingSpeedStage2,
            //2
            _ => sprintingSpeedStage3,
        };
    }
}
