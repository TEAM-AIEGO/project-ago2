using UnityEngine;

public class PlayerGroundPoundAction : MonoBehaviour
{
    private PlayerController playerController;

    private float groundPoundStartHeight;

    private Rigidbody rb;

    public void Initialized(Rigidbody rb)
    {
        this.rb = rb;
    }

    public void HandleGroundPound()
    {
        groundPoundStartHeight = transform.position.y;
        rb.linearVelocity = Vector3.down * 50; //gpspeed
    }

    //public void HandleGroundPound()
    //{
    //    bool isGrounded = IsGrounded();
    //    if (!isGrounded && !isGroundPounding)
    //    {
    //        isGroundPounding = true;
    //        isJumping = false;
    //        groundPoundStartHeight = transform.position.y;
    //        currentJumpBuffer = currentCoyoteTime = 0f;
    //        rb.linearVelocity = Vector3.down * 50; //gpspeed
    //    }
    //    else if (isGrounded)
    //    {
    //        if (!isSliding)
    //        {
    //            isSliding = true;
    //            currentSlideTime = 0;
    //        }
    //    }
    //}
}
