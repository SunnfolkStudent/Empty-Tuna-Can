using System;
using Eflatun.SceneReference;
using Entities.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Upgrades;
using Utils;
using Utils.EventBus;
using Utils.Singleton;

namespace ModeManagers {
    public class VersusModeManager : Singleton<VersusModeManager> {
        [SerializeField] private SceneReference[] versusStages;
        [SerializeField] private int winsToWin = 3;
        
        private SceneReference currentScene;
        
        private static readonly Func<bool> HasEnoughPlayers = () => PlayerManager.AllPlayers.Count > 1;
        
        private EventBinding<GameModeEvent> playerEventBinding;
        
        public static string WinText = "Nobody Wins";
        
        private void OnEnable() {
            playerEventBinding = new EventBinding<GameModeEvent>(PlayerEvent);
            EventBus<GameModeEvent>.Register(playerEventBinding);
        }
        
        private void OnDisable() {
            EventBus<GameModeEvent>.Deregister(playerEventBinding);
        }
        
        protected override void Awake() {
            base.Awake();
            
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
                    OnPlayerDeath();
                    break;
            }
        }
        
        private void OnPlayerDeath() {
            if (PlayerManager.AlivePlayers.Count > 1) return;
            PlayerScript.Paused = true;
            foreach (var playerScript in PlayerManager.AlivePlayers) {
                playerScript.wins++;
                if (playerScript.wins < winsToWin) continue;
                WinText = $"{playerScript.gameObject.name} wins";
                SceneManager.LoadScene("VersusModeWin");
                return;
            }
            
            UpgradeManager.SetUpgradesUI();
            SceneManager.UnloadSceneAsync(currentScene.Name);
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