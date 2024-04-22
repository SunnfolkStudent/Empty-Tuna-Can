using System;
using Player;
using UnityEngine;

public class VersusModeManager : MonoBehaviour {
    public static Func<bool> HasEnoughPlayers = () => PlayerManager.AllPlayersTransforms.Count > 1;
    
    public void Start() {
        PauseMenu.Pause();
        PauseMenu.UnpauseCondition = HasEnoughPlayers;
        Time.timeScale = 0;
        PlayerScript.Paused = true;
    }
}