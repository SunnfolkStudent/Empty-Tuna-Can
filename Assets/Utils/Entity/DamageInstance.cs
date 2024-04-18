using System;
using MyBox;
using UnityEngine;

namespace Utils.Entity {
    [Serializable]
    public class DamageInstance {
        public float damage;
        public float stagger;
        
        [Header("StatusEffect")]
        [SerializeField] private StatusEffects statusEffect;
        
        [ConditionalField("statusEffect",false, StatusEffects.Freeze)] [SerializeField] 
        private Freeze freezeProperties = new (2);
        
        [ConditionalField("statusEffect", false, StatusEffects.Burn)] [SerializeField] 
        private Burn burnProperties = new (1,2,2);
        
        public StatusEffect StatusEffect;
        
        public DamageInstance(float damage, float stagger = 0, StatusEffect statusEffect = null) {
            this.damage = damage;
            this.stagger = stagger;
            StatusEffect = statusEffect;
        }
        
        public void Initialize() {
            StatusEffect = statusEffect switch {
                StatusEffects.None => null,
                StatusEffects.Freeze => freezeProperties,
                StatusEffects.Burn => burnProperties,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    
    [Serializable] public enum StatusEffects{None, Freeze, Burn}
    
    [Serializable] public abstract class StatusEffect {
        public void Apply(Damageable damageable) {
        }
    }
    
    [Serializable] public class Freeze : StatusEffect {
        public float duration;
        
        public Freeze(float freezeDuration) {
            duration = freezeDuration;
        }

        public void Apply(Damageable damageable) {
            damageable.HandleFreeze(this);
        }
    }
    
    [Serializable] public class Burn : StatusEffect {
        public float damage;
        public float duration;
        public float damageFrequency;

        public Burn(float burnDamage, float burnDuration, float burnDamageFrequency) {
            damage = burnDamage;
            duration = burnDuration;
            damageFrequency = burnDamageFrequency;
        }

        public void Apply(Damageable damageable) {
            damageable.HandleBurn(this);
        }
    }
}