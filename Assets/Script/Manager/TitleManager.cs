using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject guideUiBG;

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("OutdoorsScene");
    }

    public void Guide()
    {
        if (guideUiBG.activeInHierarchy)
        {
            guideUiBG.SetActive(false);
        }
        else
        {
            guideUiBG.SetActive(true);
        }
    }

    public void GuideBgClose()
    {
        guideUiBG.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
} 