using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject gameOverRoot;
    [SerializeField] private bool pauseOnGameOver;
    [SerializeField] private bool showCursor;

    private bool hasShown;

    private void Awake()
    {
        if (gameOverRoot != null)
        {
            gameOverRoot.SetActive(false);
        }

        if (playerManager == null)
        {
            playerManager = FindFirstObjectByType<PlayerManager>();
        }

        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
    }

    private void OnEnable()
    {
        if (playerManager == null)
        {
            Debug.LogWarning("PlayerManager not found.");
            return;
        }

        playerManager.Died.AddListener(ShowGameOver);
    }

    private void OnDisable()
    {
        if (playerManager == null)
        {
            return;
        }

        playerManager.Died.RemoveListener(ShowGameOver);
    }

    private void ShowGameOver()
    {
        if (hasShown)
        {
            return;
        }

        hasShown = true;

        if (gameOverRoot != null)
        {
            gameOverRoot.SetActive(true);
        }
        else
        {
            Debug.LogWarning("GameOver root is not assigned.");
        }

        if (pauseOnGameOver)
        {
            Time.timeScale = 0f;
        }

        if (gameManager != null)
        {
            gameManager.SetGameOver();
        }
        else
        {
            Debug.LogWarning("GameManager not found.");
        }

        if (showCursor)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
