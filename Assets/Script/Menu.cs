using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject MenuImage;

    public bool IsMenuOpen;

    [SerializeField] private PlayerInputManager playerInputManager;

    private void Start()
    {
        if (MenuImage == null)
        {
            Debug.LogError("MenuImage is NUll");
        }
    }

    public bool ToggleMenu()
    {
        IsMenuOpen = !IsMenuOpen;
        Time.timeScale = IsMenuOpen? 0 : 1;
        MenuImage.SetActive(IsMenuOpen);
        return IsMenuOpen;
    }

    public void Cancel()
    {
        ToggleMenu();
        playerInputManager.SwitchCurrentPlayerActionMap();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GoToTitleScene()
    {
        ToggleMenu();
        SceneManager.LoadSceneAsync("TitleScene");
    }
}
