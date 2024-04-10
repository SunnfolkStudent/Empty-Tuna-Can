using UnityEngine;
using Utils.Entity;

public class ThrowableObject : MonoBehaviour {
    public int teamNumber;
    public int height;
    public float damage;
    
    private void OnTriggerStay2D(Collider2D other) {
        if (other.TryGetComponent(out Damageable damageable)) {
            if (damageable.teamNumber == teamNumber || damageable.heightIndex != height) return;
            damageable.HandleTakeDamage(damage);
        }
        
        Destroy(gameObject);
    }

    private void Destroy() {
        Destroy(gameObject);
    }
}
