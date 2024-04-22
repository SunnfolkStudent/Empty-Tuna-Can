using UnityEngine;
using Utils.EventBus;

namespace ModeManagers {
    public class EndlessModeManager : MonoBehaviour {
        private EventBinding<GameModeEvent> gameModeEventBinding;
        
        private void OnEnable() {
            gameModeEventBinding = new EventBinding<GameModeEvent>(PlayerEvent);
            EventBus<GameModeEvent>.Register(gameModeEventBinding);
        }
        
        private void OnDisable() {
            EventBus<GameModeEvent>.Deregister(gameModeEventBinding);
        }
    
        public void Start() {
            PlayerManager.FriendlyFire = false;
            
            // Load LevelSelect
            // SceneManager.LoadScene(playerScene.Name);
            // SceneManager.LoadScene(versusModeScene.Name, LoadSceneMode.Additive);
        }
        
        private void PlayerEvent(GameModeEvent gameModeEvent) {
            switch (gameModeEvent) {
                case PlayerDeathEvent playerDeathEvent:
                    PlayerManager.PlayerDead(playerDeathEvent.PlayerScript);
                    break;
            }
        }
    }
}
