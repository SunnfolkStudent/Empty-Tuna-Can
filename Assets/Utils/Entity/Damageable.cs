using System;
using System.Collections;
using UnityEngine;

namespace Utils.Entity {
    public class Damageable : MonoBehaviour {
        public event Action OnDying;
        
        [Header("Damageable")]
        public ClampedFloat health = new(minValue: 0, maxValue: 0);
        
        public float staggerResistance;
        public int heightIndex;
        public int teamNumber;
        
        [SerializeField] private float invincibilityFrames = 0.5f;
        
        private bool _canTakeDamage = true;
        
        public void HandleTakeDamage(float damage) {
            if (!_canTakeDamage) {
                Debug.Log("IFrame");
                return;
            }
            
            Debug.Log("takeDamage: " + damage);
            
            health.Value -= damage;
            if (health.Value <= 0) OnDying?.Invoke();
            
            _canTakeDamage = false;

            StartCoroutine(BecomeTemporarilyInvincible());
        }
        
        private IEnumerator BecomeTemporarilyInvincible() {
            _canTakeDamage = false;
            yield return new WaitForSeconds(invincibilityFrames);
            _canTakeDamage = true;
        }
        
        public void HandleReceiveHealing(float amount) {
            health.Value += amount;
        }
    }
}