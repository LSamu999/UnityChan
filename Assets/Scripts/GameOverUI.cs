using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the Game Over canvas.
/// Auto-subscribes to PlayerHealth and wires buttons in Start().
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
        // Hide panel at game start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Auto-subscribe to PlayerHealth events
        var ph = FindObjectOfType<PlayerHealth>();
        if (ph != null)
        {
            ph.onLivesChanged.AddListener(UpdateHearts);
            ph.onGameOver.AddListener(ShowGameOver);
        }

        // Wire buttons by finding them in the hierarchy
        WireButtons();
    }

    private void WireButtons()
    {
        if (gameOverPanel == null) return;

        // Search recursively for buttons named REINTENTAR_Btn and SALIR_Btn
        var allButtons = gameOverPanel.GetComponentsInChildren<Button>(true);
        foreach (var btn in allButtons)
        {
            btn.onClick.RemoveAllListeners();

            if (btn.gameObject.name == "REINTENTAR_Btn")
                btn.onClick.AddListener(OnRetry);
            else if (btn.gameObject.name == "SALIR_Btn")
                btn.onClick.AddListener(OnQuit);
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
