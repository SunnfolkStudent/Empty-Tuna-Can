using System;
using UnityEngine;

namespace Utils {
    [Serializable] 
    public struct ClampedFloat {
        public event Action<float> OnIncreaseValue;
        public event Action<float> OnDecreaseValue;
        public event Action<float> OnValueChanged;
        
        public float minValue;
        public float maxValue;
        
        [SerializeField] private float value;
        
        public float Value {
            get => value;
            internal set {
                var oldValue = this.value;
                this.value = Mathf.Clamp(value, minValue, maxValue);
                // if (this.value == oldValue) return; // if we dont want anything to happen when the value is set to the same value
                
                OnValueChanged?.Invoke(this.value);
                
                if (value > oldValue) {
                    OnIncreaseValue?.Invoke(this.value);
                }
                else if (value < oldValue) {
                    OnDecreaseValue?.Invoke(this.value);
                }
            }
        }
        
        public ClampedFloat(float minValue = -Mathf.Infinity, float maxValue = Mathf.Infinity, float initialValue = 0) {
            this.minValue = minValue;
            this.maxValue = maxValue;
            
            value = Mathf.Clamp(initialValue, minValue, maxValue);
            
            OnValueChanged = null;
            OnDecreaseValue = null;
            OnIncreaseValue = null;
        }
        
        public static implicit operator float(ClampedFloat clampedFloat) => clampedFloat.Value;
    }
}