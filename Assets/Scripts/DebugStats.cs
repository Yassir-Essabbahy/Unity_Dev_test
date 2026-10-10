using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using TMPro;

public class DebugStats : MonoBehaviour
{
    [Header("UI References")]
    public GameObject debugPanel;
    public TextMeshProUGUI debugText;

    [Header("QA References")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    private bool isOpen = false;

    // Performance Metrics
    private float smoothDeltaTime = 0f;
    private float currentFps;
    private float avgFps;
    private float frameTimeMs;

    // Cheats
    public static bool isGodMode = false;

    void Start()
    {
        if (debugPanel != null)
        {
            debugPanel.SetActive(false);
        }
    }

    void Update()
    {
        // open buttons
        bool pressedF3 = Keyboard.current != null && Keyboard.current.f3Key.wasPressedThisFrame;
        bool pressedSelect = Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame;

        if (pressedF3 || pressedSelect)
        {
            isOpen = !isOpen;
            if (debugPanel != null) debugPanel.SetActive(isOpen);
        }

        // if its open
        if (isOpen)
        {
            CalculateMetrics();
            UpdateDebugDisplay();
        }
    }

    private void CalculateMetrics()
    {
        float dt = Time.unscaledDeltaTime;
        frameTimeMs = dt * 1000f;
        currentFps = 1f / Mathf.Max(dt, 0.0001f);

        smoothDeltaTime += (dt - smoothDeltaTime) * 0.1f;
        avgFps = 1f / Mathf.Max(smoothDeltaTime, 0.0001f);
    }

    private void UpdateDebugDisplay()
    {
        // Memory
        float memoryMB = Profiler.GetTotalAllocatedMemoryLong() / (1024f * 1024f);

        // Enemmy & BUllets
        int enemiesCount = FindObjectsByType<EnemyTankAI>(FindObjectsSortMode.None).Length;
        int bulletsCount = FindObjectsByType<Projectile>(FindObjectsSortMode.None).Length;

        // Text
        debugText.text =
            "<b>DEBUG OVERLAY</b>\n\n" +
            $"FPS: {Mathf.RoundToInt(currentFps)} (Avg: {Mathf.RoundToInt(avgFps)})\n" +
            $"Frame Time: {frameTimeMs:F1} ms\n" +
            $"Memory: {memoryMB:F1} MB\n" +
            $"Entities: Enemies [{enemiesCount}] | Bullets [{bulletsCount}]\n\n" +
            $"God Mode: {(isGodMode ? "<color=green>ON</color>" : "<color=red>OFF</color>")}\n" +
            $"TimeScale: {Time.timeScale:F1}x";
    }

    //Cheats

    public void ToggleGodMode()
    {
        isGodMode = !isGodMode;
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position + Vector3.forward * 5f;
            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }

    public void SetGameSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}