using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject victoryPanel;
    public GameObject gameOverPanel;

    private bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {

        Time.timeScale = 1f;
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        // kill all enemies
        int remainingEnemies = FindObjectsByType<EnemyTankAI>(FindObjectsSortMode.None).Length;
        if (remainingEnemies == 0)
        {
            TriggerVictory();
        }
    }

    public void TriggerVictory()
    {
        isGameOver = true;
        if (victoryPanel != null) victoryPanel.SetActive(true);
    }

    public void TriggerGameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }


    // next level
    public void NextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            // if last level
            SceneManager.LoadScene(0);
        }
    }

    // Retry Button
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // back home
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}