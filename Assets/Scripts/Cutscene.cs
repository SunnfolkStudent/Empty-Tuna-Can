using ModeManagers;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.EventBus;

public class Cutscene : MonoBehaviour {
    private void TriggerCutsceneOverEvent() {
        EventBus<GameModeEvent>.Raise(new GameModeEvent(new CutsceneOverEvent()));
        Destroy(gameObject);
    }
    
    private void Update() {
        if (Input.GetAxisRaw("Jump") != 0 || Input.GetAxisRaw("Submit") != 0 || Keyboard.current.jKey.wasPressedThisFrame) {
            TriggerCutsceneOverEvent();
            Debug.Log("Skipping cutscene");
        }
    }
}