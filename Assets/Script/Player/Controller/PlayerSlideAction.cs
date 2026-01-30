using UnityEngine;

public class PlayerSlideAction : MonoBehaviour
{
    [Header("Slide Settings")]
    [SerializeField] private float SlideForce;
    [SerializeField] private float SlideTime;

    private float currentSlideTime;
    private Vector3 moveDirection = Vector3.zero;

    private Transform cam;
    private Rigidbody rb;

    public void Initialized(Transform cam, Rigidbody rb)
    {
        this.cam = cam;
        this.rb = rb;
    }

    void Update()
    {
        currentSlideTime -= Time.deltaTime;
        if (CheckSlideEnded()) return;
        rb.linearVelocity = moveDirection * SlideForce;
    }

    public void Slide(Vector3 movement)
    {
        currentSlideTime = SlideTime;
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;

        moveDirection = (movement == Vector3.zero) ? camForward : camForward * movement.z + camRight * movement.x;
        moveDirection.y = 0;
        moveDirection.Normalize();
    }

    public bool CheckSlideEnded()
    {
        return currentSlideTime <= 0;
    }
}
