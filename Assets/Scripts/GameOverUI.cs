using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the Game Over canvas.
/// Auto-subscribes to PlayerHealth.onGameOver on Start.
/// Also updates HUD hearts via PlayerHealth.onLivesChanged.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject gameOverPanel;

    [Header("Game Over broken hearts (inside panel)")]
    public Image[] gameOverHearts;

    [Header("Colors")]
    public Color heartEmptyColor = new Color(0.227f, 0.212f, 0.196f, 1f);

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Auto-subscribe to PlayerHealth
        var ph = FindObjectOfType<PlayerHealth>();
        if (ph != null)
        {
            ph.onLivesChanged.AddListener(UpdateHearts);
            ph.onGameOver.AddListener(ShowGameOver);
        }
    }

    public void UpdateHearts(int livesRemaining)
    {
        for (int i = 0; i < gameOverHearts.Length; i++)
        {
            if (gameOverHearts[i] != null)
                gameOverHearts[i].color = heartEmptyColor;
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuit()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
