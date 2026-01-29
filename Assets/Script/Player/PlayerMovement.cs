using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    const float WALKING_SPEED_STAGE1 = 6.0f;
    const float SPRINTING_SPEED_STAGE1 = 8.0f;
    const float WALKING_SPEED_STAGE2 = 10.0f;
    const float SPRINTING_SPEED_STAGE2 = 16.0f;
    const float WALKING_SPEED_STAGE3 = 18.0f;
    const float SPRINTING_SPEED_STAGE3 = 30.0f;

    private PlayerManager playerManager;
    private Collider col;
    private Rigidbody rb;
    private Vector3 movement;

    [Header("Main Camera Transform")]
    [SerializeField] private Transform cam;

    #region Stamina and Movement Stats
    [SerializeField] private float CurrentStamina;
    [SerializeField] private float MaxStamina;
    [Tooltip("per second")]
    [SerializeField] private float StaminaRegenSpeed;
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private bool IsSprinting;
    [SerializeField] private float JumpForce;
    [SerializeField] private float SlideForce;
    [SerializeField] private float SlideTime;
    #endregion

    #region Jump Stats
    [Header("Jump Settings")]
    [SerializeField] private float JumpHoldTime;
    [SerializeField] private float JumpBufferDuration;
    [SerializeField] private float CoyoteTimeDuration;
    
    #endregion

    #region floats
    private float currentJumpTime;
    private float currentJumpBuffer;
    private float currentCoyoteTime;
    private float groundPoundStartHeight;
    private float currentSlideTime;
    #endregion

    #region booleans
    private bool isJumping;
    private bool isGroundPounding;
    private bool isSliding;
    #endregion

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

    }

    void Update()
    {
        HandleCounters();
        if (isSliding) return; // temporay solution for sliding. will be changed in favor of states
        HandleJump();
        TranslationPosition();
    }

    public void SetMovement(Vector2 movementInput)
    {
        movement = new Vector3(movementInput.x, 0, movementInput.y);
    }

    public void Sprint(bool isSprinting)
    {
        Debug.Log("Sprint Input Received: " + isSprinting);
        IsSprinting = isSprinting;
    }

    public void Jump(bool IsJumping)
    {
        if (IsJumping)
        {
            currentJumpBuffer = JumpBufferDuration;
        }
        else
        {
            isJumping = false;
        }
    }

    private void TranslationPosition()
    {
        rb.rotation = quaternion.Euler(0f, cam.transform.rotation.eulerAngles.y * Mathf.Deg2Rad, 0f);

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 moveDirection = camForward * movement.z + camRight * movement.x;
        moveDirection.y = 0;
        moveDirection.Normalize();

        if (IsSprinting)
        {
            CurrentStamina = Mathf.Min(MaxStamina, CurrentStamina + Time.fixedDeltaTime * StaminaRegenSpeed);
            if (CurrentStamina >= MaxStamina)
            {
                CurrentStamina = 0;
                IsSprinting = false;
            }
        }
        else
        {
            CurrentStamina = Mathf.Max(0f, CurrentStamina - Time.fixedDeltaTime);
        }

        if (moveDirection == Vector3.zero)
        {
            IsSprinting = false;
            rb.linearVelocity = new (0, rb.linearVelocity.y, 0);
            return;
        }

        float speed = GetSpeed(gameManager.WarpStage);
        moveDirection *= speed;
        moveDirection.y = rb.linearVelocity.y;
        rb.linearVelocity = moveDirection;
    }

    private bool IsGrounded()
    {
        int layerMask = LayerMask.GetMask("Ground");
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.2f, layerMask);
        if (isGrounded && isGroundPounding)
        {
            isGroundPounding = false;
            GroundPound();
        }
        return isGrounded;
    }

    public void HandleGroundPound()
    {
        bool isGrounded = IsGrounded();
        if (!isGrounded && !isGroundPounding)
        {
            isGroundPounding = true;
            isJumping = false;
            groundPoundStartHeight = transform.position.y;
            currentJumpBuffer = currentCoyoteTime = 0f;
            rb.linearVelocity = Vector3.down * 50; //gpspeed
        }
        else if (isGrounded)
        {
            if (!isSliding)
            {
                isSliding = true;
                currentSlideTime = 0;
            }
            Slide();
        }
    }

    private void GroundPound()
    {
        // TODO: make this actually damage things
    }

    private void HandleJump()
    {
        if (currentJumpBuffer > 0f && currentCoyoteTime > 0f)
        {
            isJumping = true;
            currentJumpTime = 0f;
            currentJumpBuffer = currentCoyoteTime = 0f;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpForce);
        }

        if (isJumping && currentJumpTime < JumpHoldTime)
        {
            currentJumpTime += Time.deltaTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
        }
    }

    private void Slide()
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 moveDirection = (movement == Vector3.zero) ? camForward : camForward * movement.z + camRight * movement.x ;
        moveDirection.y = 0;
        moveDirection.Normalize();
        
        
        rb.linearVelocity = moveDirection * SlideForce;
    }

    private void HandleCounters()
    {
        if (IsGrounded()) currentCoyoteTime = CoyoteTimeDuration;
        else currentCoyoteTime -= Time.deltaTime;

        currentJumpBuffer -= Time.deltaTime;
        currentSlideTime += Time.deltaTime;
        if (SlideTime <= currentSlideTime)
        {
            isSliding = false;
        }
    }

    private float GetSpeed(int warpStage)
    {
        if (IsSprinting && CurrentStamina > 0)
        {
            return warpStage switch
            {
                0 => SPRINTING_SPEED_STAGE1,
                1 => SPRINTING_SPEED_STAGE2,
                //2
                _ => SPRINTING_SPEED_STAGE3,
            };
        }
        else
        {
            return warpStage switch
            {
                0 => WALKING_SPEED_STAGE1,
                1 => WALKING_SPEED_STAGE2,
                //2
                _ => WALKING_SPEED_STAGE3,
            };
        }
    }
}