using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the Game Over canvas.
/// Attach to the GameOverPanel root GameObject.
/// Wire up the public references in the Inspector, then hook:
///   PlayerHealth.onLivesChanged -> GameOverUI.UpdateHearts
///   PlayerHealth.onGameOver     -> GameOverUI.ShowGameOver
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject gameOverPanel;   // the full-screen dark panel

    [Header("HUD Hearts (in-game)")]
    public Image[] hudHearts;          // 3 heart images shown during play

    [Header("Game Over Hearts")]
    public Image[] gameOverHearts;     // 3 broken hearts inside the Game Over panel

    [Header("Colors")]
    public Color heartFullColor    = new Color(0.82f, 0.07f, 0.07f);   // blood red
    public Color heartEmptyColor   = new Color(0.23f, 0.21f, 0.19f);   // dark stone

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // ─── Called by PlayerHealth.onLivesChanged ──────────────────────────────
    public void UpdateHearts(int livesRemaining)
    {
        // HUD hearts
        for (int i = 0; i < hudHearts.Length; i++)
        {
            if (hudHearts[i] == null) continue;
            hudHearts[i].color = (i < livesRemaining) ? heartFullColor : heartEmptyColor;
        }

        // Game Over panel hearts  (all shown as empty/broken in game over)
        for (int i = 0; i < gameOverHearts.Length; i++)
        {
            if (gameOverHearts[i] == null) continue;
            gameOverHearts[i].color = heartEmptyColor;
        }
    }

    // ─── Called by PlayerHealth.onGameOver ──────────────────────────────────
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;  // freeze the game
    }

    // ─── Button callbacks ────────────────────────────────────────────────────
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
