using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.EventBus;

public class PauseMenu : MonoBehaviour {
    [SerializeField] private GameObject pauseButtons; 
    
    private EventBinding<PauseMenuEvent> pauseMenuEventBinding;
    
    private void OnEnable() {
        pauseMenuEventBinding = new EventBinding<PauseMenuEvent>(SetPauseMenu);
        EventBus<PauseMenuEvent>.Register(pauseMenuEventBinding);
        
        Resume();
    }
    
    private void OnDisable() {
        EventBus<PauseMenuEvent>.Deregister(pauseMenuEventBinding);
    }

    private void SetPauseMenu(PauseMenuEvent iPauseMenuEvent) {
        pauseButtons.SetActive(iPauseMenuEvent.State);
        Time.timeScale = iPauseMenuEvent.State ? 0 : 1;
        PlayerScript.Paused = iPauseMenuEvent.State;
    }
    
    public static void Resume() {
        EventBus<PauseMenuEvent>.Raise(new PauseMenuEvent(false));
    }
    
    public static void Settings() {
    }
    
    public static void Quit() {
        SceneManager.LoadScene("MainMenu");
    }
}

public class PauseMenuEvent : IEvent {
    public readonly bool State;

    public PauseMenuEvent(bool state) {
        State = state;
    }
}