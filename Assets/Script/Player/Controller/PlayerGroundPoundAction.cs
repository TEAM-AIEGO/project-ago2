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

    public void GroundPound()
    {
        groundPoundStartHeight = transform.position.y;
        rb.linearVelocity = Vector3.down * 50; //gpspeed
    }

    public void GroundPoundKaboom()
    {
        Debug.Log($"GP KABOOM POWER: {groundPoundStartHeight - transform.position.y}");
    }
}
