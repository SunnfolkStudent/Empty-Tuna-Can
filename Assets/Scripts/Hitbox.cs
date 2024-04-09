using UnityEngine;
using Utils.Entity;

public class Hitbox : MonoBehaviour {
    public int teamNumber;
    public float damage;
    public int height;
    
    private void OnTriggerStay2D(Collider2D other) {
        if (other.TryGetComponent(out Damageable damageable)) {
            Debug.Log("damageable");
            if (damageable.teamNumber == teamNumber || damageable.heightIndex != height) return;
            damageable.HandleTakeDamage(damage);
        }
    }
}