using ModeManagers;
using UnityEngine;

public class CasinoCutscene : MonoBehaviour {
    private void GoToNextLevel() {
        StoryModeManager.CallLevelCompleted();
    }
}