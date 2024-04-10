using UnityEngine;

public class EnemyAwareness : MonoBehaviour {
    public bool awareOfPlayer;
    
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            awareOfPlayer = true;
        }
    }
}