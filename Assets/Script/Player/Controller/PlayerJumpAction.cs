using UnityEngine;

public class PlayerJumpAction : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerGroundChecker groundChecker;

    #region Jump Stats
    [Header("Jump Settings")]
    [SerializeField] private float JumpForce;
    [SerializeField] private float JumpHoldTime;
    [SerializeField] private float JumpBufferDuration;
    [SerializeField] private float CoyoteTimeDuration;
    #endregion

    private float currentJumpTime;
    private float currentJumpBuffer;
    private float currentCoyoteTime;

    private Rigidbody rb;
    private SFXEmitter emitter;

    bool isJumping;

    public void Initialized(Rigidbody rb, PlayerGroundChecker groundChecker, SFXEmitter emitter)
    {  
        //Debug.Log("PlayerJumpAction Initialized");
        this.rb = rb;
        this.groundChecker = groundChecker;
        this.emitter = emitter;
    }

    private void Update()
    {
        HandleCounters();
        HandleJump();
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

    private void HandleJump()
    {
        if (currentJumpBuffer > 0f && currentCoyoteTime > 0f)
        {
            isJumping = true;
            emitter.PlayFollow("Jump", transform, false, 0.4f);
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
        if (groundChecker.IsGrounded()) currentCoyoteTime = CoyoteTimeDuration;
        else currentCoyoteTime -= Time.deltaTime;

        currentJumpBuffer -= Time.deltaTime;
        
    }
}
