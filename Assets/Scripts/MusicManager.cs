using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    [SerializeField] private List<AudioClip> audioClips;
    
    private AudioSource _audioSource;
    private int _currentClipIndex;
    
    private void Awake() {
        if (GameObject.FindGameObjectsWithTag("MusicManager").Length > 1) {
            Destroy(gameObject);
        }
    }
    
    private void Start() {
        _audioSource = gameObject.AddComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        
        PlayNextClip();
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
        }
    }
    
    private IEnumerator WaitForClipToEnd() {
        while (_audioSource.isPlaying) {
            yield return null;
        }
        
        PlayNextClip();
    }
}