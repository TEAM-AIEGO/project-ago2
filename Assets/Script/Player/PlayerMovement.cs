using UnityEngine;
using Unity.Mathematics;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager playerManager;
    private Collider col;
    private Rigidbody rb;
    private Vector3 movement;

    [Header("Main Camera Transform")]
    public Transform cam;

    #region Stamina and Movement Stats
    public float CurrentStamina;
    public float MaxStamina;
    [Tooltip("per second")]
    public float StaminaRegenSpeed;
    public float WalkSpeed;
    public float SprintSpeed;
    public bool IsSprinting;
    public float JumpForce;
    #endregion

    #region Jump Stats
    [Header("Jump Settings")]
    public float JumpHoldTime;
    public float JumpBufferDuration;
    public float CoyoteTimeDuration;
    private bool wasGrounded;
    private bool isJumping;
    private float currentJumpTime;
    private float currentJumpBuffer;
    private float currentCoyoteTime;
    #endregion

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        HandleJumpe();
        HandleCounters();
    }

    private void FixedUpdate()
    {
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
        Debug.Log("Jump Input Received: " + IsJumping);
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
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * movement.z + camRight * movement.x;

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
            return;
        }

        float speed = IsSprinting && CurrentStamina > 0 ? SprintSpeed : WalkSpeed;

        rb.MovePosition(rb.position + moveDirection.normalized * speed * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        int layerMask = LayerMask.GetMask("Enemy", "Ground", "Platform", "Object");
        bool isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, col.bounds.extents.y + 0.2f, layerMask);
        wasGrounded = isGrounded;
        return isGrounded;
    }

    private void HandleJumpe()
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

    private void HandleCounters()
    {
        if (IsGrounded()) currentCoyoteTime = CoyoteTimeDuration;
        else currentCoyoteTime -= Time.deltaTime;

        currentJumpBuffer -= Time.deltaTime;
    }
}