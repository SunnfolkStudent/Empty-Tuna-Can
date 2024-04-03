using UnityEngine;

namespace Utils.Singleton {
    public class PersistentSingleton<T> : Singleton<T> where T : Component {
        protected override void InitializeSingleton() {
            if (instance == null) {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this) Destroy(gameObject);
        }
    }
}