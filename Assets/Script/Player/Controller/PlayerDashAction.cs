using UnityEngine;

public class PlayerDashAction : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float DashForce;
    [SerializeField] private float DashTime;

    private float currentDashTime;
    private Vector3 moveDirection = Vector3.zero;

    private Transform cam;
    private Rigidbody rb;
    private SFXEmitter emitter;

    public void Initialized(Transform cam, Rigidbody rb, SFXEmitter emitter)
    {
        this.cam = cam;
        this.rb = rb;
        this.emitter = emitter;
    }

    void Update()
    {
        currentDashTime -= Time.deltaTime;
        if (CheckDashEnded()) return;
        rb.linearVelocity = moveDirection * DashForce;
    }

    public void Dash(Vector3 movement)
    {
        currentDashTime = DashTime;
        emitter.PlayFollow(AudioIds.Dash, transform);
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;

        moveDirection = (movement == Vector3.zero) ? camForward : camForward * movement.z + camRight * movement.x;
        moveDirection.y = 0;
        moveDirection.Normalize();
    }

    public bool CheckDashEnded()
    {
        return currentDashTime <= 0;
    }
}
