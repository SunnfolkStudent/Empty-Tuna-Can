using UnityEngine;
using Utils.Entity;

namespace Items {
    public class ThrowableObject : MonoBehaviour {
        public int teamNumber;
        public int height;
        public float damage;
        public float stagger;
    
        private void OnTriggerStay2D(Collider2D other) {
            if (other.TryGetComponent(out Damageable damageable)) {
                if (damageable.teamNumber == teamNumber || damageable.heightIndex != height) return;
                damageable.HandleTakeDamage(new DamageInstance(damage, stagger));
            }
            
            Destroy(gameObject);
        }

        private void Destroy() {
            Destroy(gameObject);
        }
    }
}
