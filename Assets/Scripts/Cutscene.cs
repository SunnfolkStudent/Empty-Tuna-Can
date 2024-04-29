using ModeManagers;
using UnityEngine;
using Utils.EventBus;

public class Cutscene : MonoBehaviour {
    private void TriggerCutsceneOverEvent() {
        EventBus<GameModeEvent>.Raise(new CutsceneOverEvent());
        Destroy(gameObject);
    }
    
    private void Update() {
        if (Input.GetAxisRaw("Jump") != 0 || Input.GetAxisRaw("Submit") != 0) {
            TriggerCutsceneOverEvent();
            Debug.Log("Skipping cutscene");
        }
    }
}

public class CutsceneOverEvent : GameModeEvent {
}