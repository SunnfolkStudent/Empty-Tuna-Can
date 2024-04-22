using System;
using System.Collections;
using Dialogue;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Utils.EventBus;

public class PauseMenu : MonoBehaviour {
    [SerializeField] private GameObject pauseButtons;
    [SerializeField] private EventSystem eventSystem;
    
    private EventBinding<PauseMenuEvent> pauseMenuEventBinding;
    
    public static Func<bool> UnpauseCondition = () => true;
    
    private void OnEnable() {
        pauseMenuEventBinding = new EventBinding<PauseMenuEvent>(SetPauseMenu);
        EventBus<PauseMenuEvent>.Register(pauseMenuEventBinding);
    }
    
    private void OnDisable() {
        EventBus<PauseMenuEvent>.Deregister(pauseMenuEventBinding);
    }
    
    public static void Pause() => EventBus<PauseMenuEvent>.Raise(new PauseMenuEvent(true));
    
    public static void Unpause() => EventBus<PauseMenuEvent>.Raise(new PauseMenuEvent(false));
    
    private void SetPauseMenu(PauseMenuEvent iPauseMenuEvent) {
        if (iPauseMenuEvent.State == false && !UnpauseCondition()) {
            StopAllCoroutines();
            StartCoroutine(CantStart());
            return;
        }
        pauseButtons.SetActive(iPauseMenuEvent.State);
        Time.timeScale = iPauseMenuEvent.State ? 0 : 1;
        PlayerScript.Paused = iPauseMenuEvent.State;
        eventSystem.enabled = iPauseMenuEvent.State;
    }
    
    private static IEnumerator CantStart() {
        EventBus<DialogueEvent>.Raise(new DialogueEvent(new TextEvent("Versus Mode", "Cant start Versus mode with only one player")));
        yield return new WaitForSecondsRealtime(3);
        EventBus<DialogueEvent>.Raise(new DialogueEvent(new EndEvent()));
    }
    
    public static void Settings() {
        throw new NotImplementedException();
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