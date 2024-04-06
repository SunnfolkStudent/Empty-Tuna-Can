using UnityEngine;
using Utils.Entity;

public class Hitbox : MonoBehaviour {
    public GameObject owner;
    public float damage;
    private void OnTriggerStay(Collider other) {
        if (other.gameObject == owner) return;
        
        if (other.TryGetComponent(out Damageable damageable)) {
            damageable.HandleTakeDamage(damage);
        }
    }
}