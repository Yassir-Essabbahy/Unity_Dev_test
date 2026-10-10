using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Image healthFillImage; // Reference to the fill image

    public GameObject GameOverPanel;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void takeDamage(int damage)
    {
        // if god mode is enabled
        if (DebugStats.isGodMode) return;

        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }
            Destroy(this.gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}
