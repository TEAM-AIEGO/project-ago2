using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private float WalkSpeed;
    [SerializeField] private float SprintSpeed;

    private float knockbackStun;

    private Transform cam;
    private Rigidbody rb;
    private float speed;

    public void Initialized(Transform cam, Rigidbody rb)
    {
        Debug.Log("PlayerMovement Initialized");
        this.cam = cam;
        this.rb = rb;
    }

    void Update()
    {
        knockbackStun = Mathf.Max(0, knockbackStun - Time.deltaTime);
    }

    public void SetSpeed(float speed) => this.speed = speed;

    public void TranslationPosition(Vector3 movement)
    {
        rb.rotation = quaternion.Euler(0f, cam.transform.rotation.eulerAngles.y * Mathf.Deg2Rad, 0f);
        if (knockbackStun > 0) return;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 moveDirection = camForward * movement.z + camRight * movement.x;
        moveDirection.y = 0;
        moveDirection.Normalize();

        if (moveDirection == Vector3.zero)
        {
            rb.linearVelocity = new (0, rb.linearVelocity.y, 0);
            return;
        }

        moveDirection *= speed;
        moveDirection.y = rb.linearVelocity.y;
        rb.linearVelocity = moveDirection;
    }

    public void GetKnockbackStunned(float time)
    {
        knockbackStun += time;
    }
}