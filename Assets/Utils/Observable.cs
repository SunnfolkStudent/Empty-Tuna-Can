using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Utils {
    [Serializable] 
    public class Observable<T> {
        public event UnityAction<T> OnValueChanged;
        
        private T _value;
        
        public T Value {
            get => _value;
            set {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        
        public Observable(T value = default) => _value = value;
    }
}