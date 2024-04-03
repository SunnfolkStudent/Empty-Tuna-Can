using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utils.Singleton;

public class PauseScreenManager : PersistentSingleton<PauseScreenManager> {
    public event UnityAction PauseAction;
    public event UnityAction UnpauseAction;
    
    public bool isPaused;

    public void Pause() => PauseAction?.Invoke();

    public void Unpause() => UnpauseAction?.Invoke();

    public void LoadScene(string scene) {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }

    private void OnPauseAction() {
        
    }
}