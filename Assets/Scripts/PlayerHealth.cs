using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Lives")]
    public int maxLives = 3;

    [Header("Invincibility after hit")]
    public float invincibleDuration = 2f;

    // Events
    public UnityEvent<int> onLivesChanged;   // fired every time lives change (passes new value)
    public UnityEvent     onGameOver;         // fired when lives reach 0

    private int   _lives;
    private bool  _invincible;
    private float _invincibleTimer;

    void Start()
    {
        _lives = maxLives;
        onLivesChanged?.Invoke(_lives);
    }

    void Update()
    {
        if (_invincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer <= 0f)
                _invincible = false;
        }
    }

    public void TakeDamage()
    {
        if (_invincible) return;

        _lives = Mathf.Max(0, _lives - 1);
        _invincible      = true;
        _invincibleTimer = invincibleDuration;

        onLivesChanged?.Invoke(_lives);

        if (_lives <= 0)
            onGameOver?.Invoke();
    }

    public int GetLives() => _lives;
}
