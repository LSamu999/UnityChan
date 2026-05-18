using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    public float patrolDistance = 8f;
    public float speed = 2f;

    [Header("Collision Pause")]
    public float pauseDuration = 4f;

    private Vector3 _startPos;
    private int _dir = 1;
    private Terrain _terrain;
    private float _pauseTimer = 0f;
    private bool _paused = false;
    private Transform _playerTransform;
    private Renderer[] _renderers;
    private Color[] _originalColors;

    void Start()
    {
        _startPos = transform.position;
        _terrain = Terrain.activeTerrain;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) _playerTransform = playerObj.transform;

        // Add detection trigger sphere
        var triggerGO = new GameObject("DetectionTrigger");
        triggerGO.transform.SetParent(transform);
        triggerGO.transform.localPosition = Vector3.zero;
        var sphere = triggerGO.AddComponent<SphereCollider>();
        sphere.radius = 0.8f;
        sphere.isTrigger = true;
        triggerGO.AddComponent<EnemyDetector>().owner = this;

        _renderers = GetComponentsInChildren<Renderer>();
        _originalColors = new Color[_renderers.Length];
        for (int i = 0; i < _renderers.Length; i++)
            _originalColors[i] = _renderers[i].material.color;
    }

    void Update()
    {
        if (_paused)
        {
            _pauseTimer -= Time.deltaTime;
            if (_pauseTimer <= 0f)
                Resume();
            return;
        }

        Vector3 pos = transform.position;
        pos += transform.right * _dir * speed * Time.deltaTime;
        if (_terrain != null)
            pos.y = _terrain.SampleHeight(pos) + 0.05f;
        transform.position = pos;

        float offset = transform.position.x - _startPos.x;
        if (offset >= patrolDistance || offset <= 0f)
        {
            _dir *= -1;
            Vector3 s = transform.localScale;
            s.x *= -1f;
            transform.localScale = s;
        }
    }

    public void OnPlayerContact()
    {
        if (_paused) return;
        _paused = true;
        _pauseTimer = pauseDuration;

        // Flash red to indicate collision
        foreach (var r in _renderers)
            r.material.color = Color.red;
    }

    void Resume()
    {
        _paused = false;
        for (int i = 0; i < _renderers.Length; i++)
            _renderers[i].material.color = _originalColors[i];
    }
}
