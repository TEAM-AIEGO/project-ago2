using UnityEngine;
using static UnityEngine.InputSystem.InputAction;


public class PlayerInputManager : MonoBehaviour
{
    private PlayerAction playerAction;
    private PlayerMovement playerMovement;
    private PlayerGunHandler playerGunHandler;
    private PlayerCameraMovement playerCameraMovement;
    private Menu menu;

    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        playerAction = new PlayerAction();
        playerMovement = GetComponent<PlayerMovement>();
        playerGunHandler = GetComponentInChildren<PlayerGunHandler>();
        playerCameraMovement = GetComponentInChildren<PlayerCameraMovement>();
        menu = FindFirstObjectByType<Menu>();
        if (!menu)
        {
            Debug.LogWarning("No Menu Found! Toggling menu will not work.");
        }

        Mapping();
    }

    private void OnEnable()
    {
        playerAction.Enable();
    }

    private void OnDestroy()
    {
        UnMapping();
        playerAction.Disable();
    }

    private void Mapping()
    {
        playerAction.Player.Move.performed += OnMove;
        playerAction.Player.Move.canceled += OnMove;
        playerAction.Player.Jump.performed += OnJump;
        playerAction.Player.Jump.canceled += OnJump;
        playerAction.Player.Sprint.performed += OnSprint;
        playerAction.Player.Sprint.canceled += OnSprint;
        playerAction.Player.GroundPound.performed += OnGroundPound;

        playerAction.Player.Look.performed += OnLook;
        playerAction.Player.Look.canceled += OnLook;

        playerAction.Player.Fire.started += OnFire;
        playerAction.Player.Fire.canceled += OnFire;

        playerAction.Player.Menu.started += OnMenu;

        playerAction.Enable();
    }

    private void UnMapping()
    {
        playerAction.Player.Move.performed -= OnMove;
        playerAction.Player.Move.canceled -= OnMove;
        playerAction.Player.Jump.performed -= OnJump;
        playerAction.Player.Jump.canceled -= OnJump;
        playerAction.Player.Sprint.performed -= OnSprint;
        playerAction.Player.Sprint.canceled -= OnSprint;
        playerAction.Player.GroundPound.performed -= OnGroundPound;

        playerAction.Player.Look.performed -= OnLook;
        playerAction.Player.Look.canceled -= OnLook;

        playerAction.Player.Fire.started -= OnFire;
        playerAction.Player.Fire.canceled -= OnFire;

        playerAction.Player.Menu.started -= OnMenu;

        playerAction.Enable();
    }

    public void OnGroundPound(CallbackContext context)
    {
        if (menu && menu.IsMenuOpen) return;
        playerMovement.HandleGroundPound();
    }

    public void OnMove(CallbackContext context)
    {
        if (menu && menu.IsMenuOpen) return;
        playerMovement.SetMovement(context.ReadValue<Vector2>());
    }

    public void OnJump(CallbackContext context)
    {
        if (menu && menu.IsMenuOpen) return;
        playerMovement.Jump(context.performed);
    }

    public void OnSprint(CallbackContext context) 
    {
        if (menu && menu.IsMenuOpen) return;
        playerMovement.Sprint(context.performed);
    }

    public void OnLook(CallbackContext context)
    {
        if (menu && menu.IsMenuOpen) return;
        playerCameraMovement.SetInput(context.ReadValue<Vector2>());
    }

    public void OnMenu(CallbackContext context)
    {
        if (!menu) return;
        menu.ToggleMenu();
    }

    public void OnFire(CallbackContext context)
    {
        if (menu && menu.IsMenuOpen) return;
        if (context.started)
        {
            playerGunHandler.OnFire(true);
            return;
        }

        if (context.canceled)   
        {
            playerGunHandler.OnFire(false);
            return;
        }
    }
}
