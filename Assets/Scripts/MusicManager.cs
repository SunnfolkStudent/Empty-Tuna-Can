using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : PersistentSingleton<MusicManager> {
    [SerializeField] private List<AudioClip> audioClips;
    
    private AudioSource _audioSource;
    
    private int _currentClipIndex;
    
    protected override void Awake() {
        base.Awake();
        _audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    private void Start() {
        if (audioClips.Count > 0) {
            PlayNextClip();
        }
    }
    
    private void PlayNextClip() {
        if (_currentClipIndex < audioClips.Count) {
            _audioSource.clip = audioClips[_currentClipIndex];
            _audioSource.Play();
            StartCoroutine(WaitForClipToEnd());
            _currentClipIndex++;
        }
        else {
            Debug.Log("All audio clips played.");
            _currentClipIndex = 0;
            PlayNextClip();
        }
    }
    
    private IEnumerator WaitForClipToEnd() {
        while (_audioSource.isPlaying) {
            yield return null;
        }
        
        PlayNextClip();
    }
}