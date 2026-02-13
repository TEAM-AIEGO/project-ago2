using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("OutdoorsScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
} 