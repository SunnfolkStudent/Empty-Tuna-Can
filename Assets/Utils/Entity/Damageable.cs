using System;
using System.Collections;
using UnityEngine;

namespace Utils.Entity {
    public abstract class Damageable : MonoBehaviour {
        public event Action OnDying;
        
        [Header("Damageable")]
        public ClampedFloat health = new(minValue: 0, maxValue: 0);
        
        [Tooltip("Time entity is invincible after getting hit")] 
        [SerializeField] private float invincibilityFrames = 0.5f;
        
        public float staggerResistance;
        public int heightIndex;
        public int teamNumber;
        
        private bool _canTakeDamage = true;
        
        public void HandleTakeDamage(float damage, float stagger) {
            if (!_canTakeDamage) return;
            
            health.Value -= damage;
            if (health.Value <= 0) OnDying?.Invoke();
            
            StartCoroutine(BecomeTemporarilyInvincible());
            
            if (staggerResistance <= stagger) Stagger();
        }
        
        protected abstract void Stagger();

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