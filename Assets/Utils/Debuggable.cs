using System;
using UnityEngine;

namespace Utils {
    [Serializable] 
    public class DebuggableMonoBehaviour : MonoBehaviour {
        [Header("isDebugging")]
        public bool isDebugging;
        
        protected void DebugLog(string message) {
            if (isDebugging) Debug.Log(message);
        }
    }
}