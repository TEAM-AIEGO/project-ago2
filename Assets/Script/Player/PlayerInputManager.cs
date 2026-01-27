using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


public class PlayerInputManager : MonoBehaviour
{
    private PlayerAction playerAction;
    private PlayerMovement playerMovement;
    private PlayerCameraMovement playerCameraMovement;

    private void Awake()
    {
        playerAction = new PlayerAction();
        playerMovement = GetComponent<PlayerMovement>();
        playerCameraMovement = GetComponentInChildren<PlayerCameraMovement>();

        Mapping();
    }

    private void OnEnable()
    {
        playerAction.Enable();
    }

    private void Mapping()
    {
        playerAction.Player.Move.performed += OnMove;
        playerAction.Player.Move.canceled += OnMove;
        playerAction.Player.Jump.performed += OnJump;
        playerAction.Player.Jump.canceled += OnJump;
        playerAction.Player.Sprint.performed += OnSprint;
        playerAction.Player.Sprint.canceled += OnSprint;

        playerAction.Player.Look.performed += OnLook;
        playerAction.Player.Look.canceled += OnLook;

        playerAction.Enable();
    }

    public void OnMove(CallbackContext context)
    {
        playerMovement.SetMovement(context.ReadValue<Vector2>());
    }

    public void OnJump(CallbackContext context)
    {
        playerMovement.Jump(context.performed);
    }

    public void OnSprint(CallbackContext context) 
    {
        playerMovement.Sprint(context.performed);
    }

    public void OnLook(CallbackContext context)
    {
        playerCameraMovement.SetInput(context.ReadValue<Vector2>());
    }
}
