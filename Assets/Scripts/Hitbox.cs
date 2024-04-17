using UnityEngine;
using Utils.Entity;

public class Hitbox : MonoBehaviour {
    public int teamNumber;
    public int height;

    public DamageInstance damageInstance;
    
    private void OnTriggerStay2D(Collider2D other) {
        if (!other.TryGetComponent(out Damageable damageable)) return;
        
        if (damageable.heightIndex == height && (damageable.teamNumber == 0 || damageable.teamNumber != teamNumber))
            damageable.HandleTakeDamage(damageInstance);
    }
}