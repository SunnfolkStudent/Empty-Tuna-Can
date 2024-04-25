using System;
using System.Threading.Tasks;
using Eflatun.SceneReference;
using Entities.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Upgrades;
using Utils;
using Utils.EventBus;

namespace ModeManagers {
    public class VersusModeManager : MonoBehaviour {
        [SerializeField] private SceneReference[] versusStages;
        private SceneReference currentScene;
    
        private static readonly Func<bool> HasEnoughPlayers = () => PlayerManager.AllPlayers.Count > 1;
        private const int WinsToWin = 5;
    
        private EventBinding<GameModeEvent> playerEventBinding;
    
        private void OnEnable() {
            playerEventBinding = new EventBinding<GameModeEvent>(PlayerEvent);
            EventBus<GameModeEvent>.Register(playerEventBinding);
        }
    
        private void OnDisable() {
            EventBus<GameModeEvent>.Deregister(playerEventBinding);
        }

        private void Awake() {
            SceneManager.LoadScene(GetNextScene(), LoadSceneMode.Additive);
        }

        public void Start() {
            PlayerManager.FriendlyFire = true;
        
            PauseMenu.Pause();
            PauseMenu.UnpauseCondition = HasEnoughPlayers;
        }

        private void PlayerEvent(GameModeEvent gameModeEvent) {
            switch (gameModeEvent) {
                case PlayerDeathEvent playerDeathEvent:
                    PlayerManager.PlayerDead(playerDeathEvent.PlayerScript);
                    _ = OnPlayerDeath();
                    break;
            }
        }

        private async Task OnPlayerDeath() {
            if (PlayerManager.AlivePlayers.Count > 1) return;
            PlayerScript.Paused = true;
            foreach (var playerScript in PlayerManager.AlivePlayers) {
                playerScript.wins++;
                if (playerScript.wins < WinsToWin) continue;
                // TODO: Player wins
                SceneManager.LoadScene("MainMenu");
                return;
            }
        
            UpgradeManager.SetUpgradesUI();
            await SceneManager.UnloadSceneAsync(currentScene.Name);
            SceneManager.LoadScene(GetNextScene(), LoadSceneMode.Additive);
        
            PlayerManager.ReviveAllPlayers();
            PlayerManager.SetSpawnForPlayers();
            PauseMenu.Pause();
        }

        private string GetNextScene() {
            while (true) {
                var i = versusStages.GetRandom();
                if (i == currentScene) continue;
                currentScene = i;
                return currentScene.Name;
            }
        }
    }

    public class GameModeEvent : IEvent {
    }

    public class PlayerDeathEvent : GameModeEvent {
        public readonly PlayerScript PlayerScript;

        public PlayerDeathEvent(PlayerScript playerScript) {
            PlayerScript = playerScript;
        }
    }
}