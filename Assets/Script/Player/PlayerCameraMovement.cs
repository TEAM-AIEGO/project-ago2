using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerAction;

    public float MouseSensitivity;
    public float GamepadSensitivity;

    private float yaw;
    private float pitch;

    Vector2 look;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetInput(Vector2 lookInput)
    {
        look = lookInput;
    }

    void Update()
    {
        yaw += look.x * MouseSensitivity;
        pitch -= look.y * MouseSensitivity;

        pitch = Mathf.Clamp(pitch, -40f, 40f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    public void MouseFixUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}