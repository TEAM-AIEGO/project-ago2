using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;

    private float yaw;
    private float pitch;
    private Vector2 look;

    private CameraShake shake;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        shake = GetComponent<CameraShake>();
    }

    public void SetInput(Vector2 lookInput)
    {
        look = lookInput;
    }

    void Update()
    {
        yaw += look.x * mouseSensitivity;
        pitch -= look.y * mouseSensitivity;

        if (shake != null)
        {
            Vector2 r = shake.GetRecoil() * Time.deltaTime;
            pitch -= r.y;
            yaw += r.x;
        }

        pitch = Mathf.Clamp(pitch, -89f, 89f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}