using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject victoryPanel;
    public GameObject gameOverPanel;

    [Header("Settings")]
    public bool requiresKillingEnemies = true;

    private bool isGameOver = false;
    private float checkTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // Test frame
        checkTimer = 0.5f;
    }

    void Update()
    {
        if (isGameOver || !requiresKillingEnemies) return;

        // optimize
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            checkTimer = 0.2f;

            int remainingEnemies = FindObjectsByType<EnemyTankAI>(FindObjectsSortMode.None).Length;
            if (remainingEnemies == 0)
            {
                TriggerVictory();
            }
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void TriggerVictory()
    {
        if (isGameOver) return;
        isGameOver = true;
        if (victoryPanel != null) victoryPanel.SetActive(true);
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    // Next Level
    public void NextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            // Back
            SceneManager.LoadScene(0);
        }
    }

    // Retry
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Back Home
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}