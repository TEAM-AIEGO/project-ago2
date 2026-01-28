using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject MenuImage;

    public bool IsMenuOpen;

    private void Start()
    {
        if (MenuImage == null)
        {
            Debug.LogError("MenuImage is NUll");
        }
    }

    public void ToggleMenu()
    {
        IsMenuOpen = !IsMenuOpen;
        Time.timeScale = IsMenuOpen? 0 : 1;
        Cursor.lockState =  IsMenuOpen? CursorLockMode.None : CursorLockMode.Locked;
        MenuImage.SetActive(IsMenuOpen);
    }

    public void GoToTitleScene()
    {
        SceneManager.LoadSceneAsync("TitleScene");
    }
}
