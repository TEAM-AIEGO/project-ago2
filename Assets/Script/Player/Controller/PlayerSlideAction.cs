using UnityEngine;

public class PlayerSlideAction : MonoBehaviour
{
    [Header("Slide Settings")]
    [SerializeField] private float SlideForce;
    [SerializeField] private float SlideTime;

    private float currentSlideTime;

    private Transform cam;
    private Rigidbody rb;

    public void Initialized(Transform cam, Rigidbody rb)
    {
        this.cam = cam;
        this.rb = rb;
    }

    private void Slide(Vector3 movement)
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 moveDirection = (movement == Vector3.zero) ? camForward : camForward * movement.z + camRight * movement.x;
        moveDirection.y = 0;
        moveDirection.Normalize();

        rb.linearVelocity = moveDirection * SlideForce;
    }
}
