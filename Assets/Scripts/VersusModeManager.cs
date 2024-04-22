using System;
using Utils.Singleton;

public class VersusModeManager : PersistentSingleton<VersusModeManager> {
    public static Func<bool> HasEnoughPlayers = () => PlayerManager.AllPlayersTransforms.Count > 1;
    
    private void Start() {
        PauseMenu.Pause();
        PauseMenu.UnpauseCondition = HasEnoughPlayers;
    }
}