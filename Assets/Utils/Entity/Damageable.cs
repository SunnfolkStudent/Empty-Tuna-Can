using System;
using Unity.Mathematics;
using UnityEngine;

namespace Utils.Entity {
    public class Damageable : MonoBehaviour {
        public event Action OnDying;
        
        public ClampedFloat health = new(minValue: 0, maxValue: 0);
        public ClampedFloat armor = new (minValue: 0, maxValue: 0);
        
        public void SetInitialValues(float healthValue, float armorValue = 0) {
            health.maxValue = healthValue;
            health.Value = healthValue;
            
            armor.maxValue = armorValue;
            armor.Value = armorValue;
        }
        
        public void HandleTakeDamage(float damage, float piercingValue) {
            var armorDamageReduction = math.max(0, (armor.Value - piercingValue));
            var modifiedDamage = math.max(0, damage - armorDamageReduction);
            Debug.Log($"damage: {damage}, piercingValue: {piercingValue}, modifiedDamage: {modifiedDamage}");
            health.Value -= modifiedDamage;
            if (health.Value <= 0) OnDying?.Invoke();
        }
        
        public void HandleReceiveHealing(float amount) {
            health.Value += amount;
        }
    }
}