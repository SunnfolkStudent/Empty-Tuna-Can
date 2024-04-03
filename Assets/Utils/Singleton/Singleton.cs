using UnityEngine;

namespace Utils.Singleton {
    public class Singleton<T> : MonoBehaviour where T : Component {
        protected static T instance;
        
        public static bool HasInstance => instance != null;
        public static T TryGetInstance() => HasInstance ? instance : null;
        
        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindAnyObjectByType<T>();
                    
                    if (instance == null) {
                        var go = new GameObject(typeof(T).Name + " Auto-Generated");
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Make sure to call base.Awake() in override if you need awake.
        /// </summary>
        protected virtual void Awake() {
            transform.parent = null;
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton() {
            instance = this as T;
        }
    }
}