using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Utils {
    public abstract class Spawner : MonoBehaviour {
        protected IFactory factory;
        private Coroutine _spawningCoroutine;
        
        protected virtual void SpawnGameObject([CanBeNull] params object[] parameters) {
            if (factory == null) {
                Debug.LogWarning("No factory assigned to spawner");
                return;
            }
            
            Instantiate(factory.CreateGameObject(parameters));
        }

        #region ---IntervalSpawning;
        protected void StartSpawningAtInterval(float interval) {
            if (_spawningCoroutine != null) return; // Only one coroutine can run at a time
            _spawningCoroutine = StartCoroutine(SpawningAtInterval(interval));
        }
        
        protected void StopSpawningAtInterval() {
            if (_spawningCoroutine == null) return;
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }
        
        private IEnumerator SpawningAtInterval(float interval) {
            while (true) {
                yield return new WaitForSeconds(interval);
                SpawnGameObject();
            }
            // ReSharper disable once IteratorNeverReturns
        }
        #endregion
    }
}