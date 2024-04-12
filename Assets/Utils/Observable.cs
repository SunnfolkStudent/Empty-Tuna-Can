using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utils {
    /// <summary>
    /// Holds a reference to a variable if type T and an event that gives the value to the variable when changed
    /// </summary>
    [Serializable] 
    public class Observable<T> {
        public event UnityAction<T> OnValueChanged = delegate { };
        
        [SerializeField] private T value;
        
        public T Value {
            get => value;
            set {
                // if (EqualityComparer<T>.Default.Equals(this.value, value)) return;
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        
        public Observable(T value = default) => this.value = value;

        public void ForceUpdate() {
            OnValueChanged.Invoke(Value);
        }
    }
}