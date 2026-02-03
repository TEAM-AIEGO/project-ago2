using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerInputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController playerController;
    private PlayerGunHandler playerGunHandler;
    private PlayerCameraMovement playerCameraMovement;
    private Menu menu;

    [SerializeField] private InputActionReference MoveActionReference, JumpActionReference, SprintActionReference, MovementAction1ActionReference, LookActionReference, FireActionReference, PlayerMenuActionReference, UiMenuActionReference, UseSubActionReference; 

    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        playerInput = GetComponent<PlayerInput>();
        playerController = GetComponent<PlayerController>();
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
        Mapping();
    }

    private void OnDestroy()
    {
        UnMapping();
    }

    private void Mapping()
    {
        MoveActionReference.action.performed += OnMove;
        MoveActionReference.action.canceled += OnMove;
        JumpActionReference.action.performed += OnJump;
        JumpActionReference.action.canceled += OnJump;
        SprintActionReference.action.performed += OnDesh;
        //SprintActionReference.action.canceled += OnDesh;
        MovementAction1ActionReference.action.performed += OnGroundPound;

        LookActionReference.action.performed += OnLook;
        LookActionReference.action.canceled += OnLook;

        FireActionReference.action.started += OnFire;
        FireActionReference.action.canceled += OnFire;

        PlayerMenuActionReference.action.started += OnMenu;
        UiMenuActionReference.action.started += OnMenu;

        UseSubActionReference.action.performed += OnUseSub;
    }

    private void UnMapping()
    {
        MoveActionReference.action.performed -= OnMove;
        MoveActionReference.action.canceled -= OnMove;
        JumpActionReference.action.performed -= OnJump;
        JumpActionReference.action.canceled -= OnJump;
        SprintActionReference.action.performed -= OnDesh;
        //SprintActionReference.action.canceled -= OnDesh;
        MovementAction1ActionReference.action.performed -= OnGroundPound;

        LookActionReference.action.performed -= OnLook;
        LookActionReference.action.canceled -= OnLook;

        FireActionReference.action.started -= OnFire;
        FireActionReference.action.canceled -= OnFire;

        UiMenuActionReference.action.started += OnMenu;

        UseSubActionReference.action.performed -= OnUseSub;
    }

    public void OnGroundPound(CallbackContext context)
    {
        if (menu && menu.IsMenuOpen) return;
        playerController.MovementAction1();
    }

    public void OnMove(CallbackContext context)
    {
        if (menu && menu.IsMenuOpen) return;
        playerController.SetMovement(context.ReadValue<Vector2>());
    }

    public void OnJump(CallbackContext context)
    {
        if (menu && menu.IsMenuOpen) return;
        playerController.Jump(context.performed);
    }

    public void OnDesh(CallbackContext context) 
    {
        if (menu && menu.IsMenuOpen) return;
        playerController.Desh(context.performed);
    }

    public void OnLook(CallbackContext context)
    {
        if (menu && menu.IsMenuOpen) return;
        playerCameraMovement.SetInput(context.ReadValue<Vector2>());
    }

    public void OnMenu(CallbackContext context)
    {
        if (!menu) return;
        if (menu.ToggleMenu())
        {
            playerInput.SwitchCurrentActionMap("UI");
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Player");
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    public void OnFire(CallbackContext context)
    {
        if (menu && menu.IsMenuOpen) return;
        if (context.started)
        {
            playerGunHandler.Fire(true);
            return;
        }

        if (context.canceled)   
        {
            playerGunHandler.Fire(false);
            return;
        }
    }

    public void OnUseSub(CallbackContext context)
    {
        if (menu && menu.IsMenuOpen) return;
        if (!context.performed) return;

        if (playerGunHandler == null)
        {
            Debug.LogWarning("No SubWeapon Found");
            return;
        }

        playerGunHandler.UseSubWeapon(context.control.path);
    }
}
