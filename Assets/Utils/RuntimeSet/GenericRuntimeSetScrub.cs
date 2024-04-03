using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.RuntimeSet {
    public abstract class GenericRuntimeSetScrub<T> : ScriptableObject {
        public List<T> items = new ();
        
        public event Action<int> CountChanged;
        
        public void Add(T item) {
            if (!items.Contains(item)) items.Add(item);
            CountChanged?.Invoke(items.Count);
        }
        
        public void Remove(T item) {
            if (items.Contains(item)) items.Remove(item);
            CountChanged?.Invoke(items.Count);
        }
    }
}