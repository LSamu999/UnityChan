using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// HUD top-left: panel stone + "VIDAS" label + 3 hearts + counter.
/// Auto-subscribes to PlayerHealth on Start — no manual Inspector wiring needed.
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    [Header("Heart Images (3)")]
    public Image[] heartImages;

    [Header("X Marks on empty hearts")]
    public GameObject[] heartXMarks;

    [Header("Counter text (e.g. 3/3)")]
    public TextMeshProUGUI counterText;

    [Header("Colors")]
    public Color heartFullColor  = new Color(0.80f, 0.067f, 0.067f, 1f); // #cc1111
    public Color heartEmptyColor = new Color(0.227f, 0.212f, 0.196f, 1f); // #3a3530
    public Color counterNormal   = new Color(0.776f, 0.651f, 0.392f, 1f); // gold
    public Color counterLow      = new Color(0.545f, 0.000f, 0.000f, 1f); // blood red

    private int _maxLives;

    void Start()
    {
        _maxLives = heartImages != null ? heartImages.Length : 3;

        // Auto-subscribe to PlayerHealth
        var ph = FindObjectOfType<PlayerHealth>();
        if (ph != null)
            ph.onLivesChanged.AddListener(SetLives);

        SetLives(_maxLives);
    }

    public void SetLives(int lives)
    {
        lives = Mathf.Clamp(lives, 0, _maxLives);

        for (int i = 0; i < heartImages.Length; i++)
        {
            bool full = i < lives;
            if (heartImages[i] != null)
                heartImages[i].color = full ? heartFullColor : heartEmptyColor;
            if (heartXMarks != null && i < heartXMarks.Length && heartXMarks[i] != null)
                heartXMarks[i].SetActive(!full);
        }

        if (counterText != null)
        {
            counterText.text  = lives + "/" + _maxLives;
            counterText.color = (lives <= 1) ? counterLow : counterNormal;
        }
    }
}
