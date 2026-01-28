using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float gamepadSensitivity;

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
        yaw += look.x * mouseSensitivity;
        pitch -= look.y * mouseSensitivity;

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