using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Entities {
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
        
        public void HandleTakeDamage(DamageInstance damageInstance) {
            if (!_canTakeDamage) return;
            
            TakeDamage(damageInstance.damage);
            damageInstance.StatusEffect?.Apply(this);
            
            StartCoroutine(BecomeTemporarilyInvincible());
            
            if (staggerResistance <= damageInstance.stagger) Stagger();
        }

        private void TakeDamage(float damage) {
            health.Value -= damage;
            if (health.Value <= 0) OnDying?.Invoke();
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
        
        public void ReceiveFullHealing() {
            health.Value += health.Value;
        }

        public void HandleFreeze(Freeze freeze) {
            // TODO: Freeze movement of the Entity
        }
        
        public void HandleBurn(Burn burn) {
            StartCoroutine(TakeBurnDamage(burn));
        }

        public void StopStatusEffects() {
            StopAllCoroutines();
        }
        
        private IEnumerator TakeBurnDamage(Burn burn) {
            var startTime = Time.time;
            while (Time.time < startTime + burn.duration) {
                TakeDamage(burn.damage);
                yield return new WaitForSeconds(1 / burn.damageFrequency);
            }
        }
    }
}