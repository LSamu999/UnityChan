using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the HUD health bar panel.
/// Shows 3 heart icons (red = full, dark gray = empty) + a "X/3" counter.
/// Hook PlayerHealth.onLivesChanged -> HealthBarUI.SetLives in the Inspector.
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    [Header("Heart Icons (assign 3 Image components)")]
    public Image[] heartImages;         // 3 heart Image components

    [Header("Heart Overlay X marks")]
    public GameObject[] heartXMarks;    // X mark child on each heart (hidden when full)

    [Header("Counter Text")]
    public TextMeshProUGUI counterText; // "3/3" display

    [Header("Colors")]
    public Color heartFullColor  = new Color(0.80f, 0.07f, 0.07f, 1f);  // #cc1111
    public Color heartEmptyColor = new Color(0.23f, 0.21f, 0.19f, 1f);  // #3a3530
    public Color counterNormal   = new Color(0.776f, 0.651f, 0.392f, 1f); // gold
    public Color counterLow      = new Color(0.545f, 0.000f, 0.000f, 1f); // blood red

    private int _maxLives = 3;
    private int _currentLives = 3;

    void Start()
    {
        _maxLives = heartImages != null ? heartImages.Length : 3;
        SetLives(_maxLives);
    }

    /// <summary>Call this from PlayerHealth.onLivesChanged.</summary>
    public void SetLives(int lives)
    {
        _currentLives = Mathf.Clamp(lives, 0, _maxLives);

        for (int i = 0; i < heartImages.Length; i++)
        {
            bool full = i < _currentLives;

            // Heart colour
            if (heartImages[i] != null)
                heartImages[i].color = full ? heartFullColor : heartEmptyColor;

            // X mark visibility
            if (heartXMarks != null && i < heartXMarks.Length && heartXMarks[i] != null)
                heartXMarks[i].SetActive(!full);
        }

        // Counter text
        if (counterText != null)
        {
            counterText.text  = _currentLives + "/" + _maxLives;
            counterText.color = (_currentLives <= 1) ? counterLow : counterNormal;
        }
    }
}
