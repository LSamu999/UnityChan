using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public EnemyPatrol owner;

    void OnTriggerEnter(Collider other)
    {
        if (owner == null) return;

        bool isPlayer = other.CompareTag("Player") ||
                        other.transform.root.CompareTag("Player");
        if (!isPlayer) return;

        // 1. Enemy reacts (pause + flash red)
        owner.OnPlayerContact();

        // 2. Player loses a life
        GameObject playerRoot = other.CompareTag("Player")
            ? other.gameObject
            : other.transform.root.gameObject;

        var health = playerRoot.GetComponent<PlayerHealth>();
        if (health != null)
            health.TakeDamage();
    }
}
