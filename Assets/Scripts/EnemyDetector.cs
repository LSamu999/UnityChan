using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public EnemyPatrol owner;

    void OnTriggerEnter(Collider other)
    {
        if (owner == null) return;
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
            owner.OnPlayerContact();
    }
}
