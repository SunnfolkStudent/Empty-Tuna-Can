using System;
using Eflatun.SceneReference;
using Entities.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Upgrades;
using Utils.EventBus;

namespace ModeManagers {
    public class StoryModeManager : MonoBehaviour {
        [Header("Scenes")]
        [SerializeField] private SceneReference playerScene;
        [SerializeField] private SceneReference gameOverScene;
        
        [Header("Areas")]
        [SerializeField] private SceneReference[] areaScenes;
        [SerializeField] private GameObject[] areaEnemies;
        [SerializeField] private GameObject cutsceneCamera;
        
        private EventBinding<GameModeEvent> playerEventBinding;
        
        private event Action LevelCompleted = () => { };
        
        private int currentLevel;

        private void Awake() {
            LevelCompleted += StartNextLevel;
            LevelCompleted += PlayerManager.ReviveAllPlayers;
            LevelCompleted += UpgradeManager.SetUpgradesUI;
            LevelCompleted += PauseMenu.Pause;
        }
        
        public void Start() {
            PlayerManager.FriendlyFire = false;
            
            PauseMenu.Pause();
            Time.timeScale = 0;
            PlayerScript.Paused = true;
        }
        
        private void OnEnable() {
            playerEventBinding = new EventBinding<GameModeEvent>(PlayerEvent);
            EventBus<GameModeEvent>.Register(playerEventBinding);
        }
        
        private void OnDisable() {
            EventBus<GameModeEvent>.Deregister(playerEventBinding);
        }
        
        private void PlayerEvent(GameModeEvent gameModeEvent) {
            switch (gameModeEvent) {
                case PlayerDeathEvent playerDeathEvent:
                    PlayerManager.PlayerDead(playerDeathEvent.PlayerScript);
                    if (PlayerManager.AlivePlayers.Count == 0) SceneManager.LoadScene(gameOverScene.Name);
                    break;
                case WaveOver:
                    LevelCompleted.Invoke();
                    break;
                case CutsceneOverEvent:
                    StartLevel1();
                    break;
            }
        }
        
        private void StartLevel1() {
            cutsceneCamera.SetActive(false);
            SceneManager.LoadScene(playerScene.Name, LoadSceneMode.Additive);
            SceneManager.LoadScene(areaScenes[currentLevel].Name, LoadSceneMode.Additive);
            areaEnemies[currentLevel].SetActive(true);
        }
        
        private void StartNextLevel() {
            currentLevel++;
            SceneManager.LoadScene(areaScenes[currentLevel].Name, LoadSceneMode.Additive);
            areaEnemies[currentLevel].SetActive(true);
        }
    }
}