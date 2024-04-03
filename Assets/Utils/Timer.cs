using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Utils {
    public class Timer {
        public event Action<float> Progress;
        public event Action OnComplete;
        public event Action OnStop;
        
        public float TotalTime => _totalTime;
        public float ProgressTime => _totalTime * CurrentProgress;
        public float TimeRemaining => _totalTime - ProgressTime;
        
        private readonly float _totalTime;
        private float _currentProgress;
        private float CurrentProgress {
            get => _currentProgress;
            set {
                _currentProgress = value;
                Progress?.Invoke(_currentProgress);
            }
        }
        
        public Timer(float time) {
            _totalTime = time;
            CoroutineRunner.instance.StartCoroutine(StartTimer(time));
        }
        
        public void StopTimer() {
            CoroutineRunner.instance.StopAllCoroutines();
            OnStop?.Invoke();
        }
        
        private IEnumerator StartTimer(float seconds) {
            var progressTime = 0f;
            
            while (progressTime < seconds) {
                yield return null;
                progressTime += Time.deltaTime;
                CurrentProgress = progressTime / seconds;
            }
            CurrentProgress = 1;
            
            OnComplete?.Invoke();
        }
    }
}