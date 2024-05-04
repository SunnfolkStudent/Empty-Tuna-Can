using System;
using Eflatun.SceneReference;
using Entities.Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Upgrades;
using Utils.EventBus;

namespace ModeManagers {
    public class StoryModeManager : MonoBehaviour {
        [Header("Scenes")]
        [SerializeField] private SceneReference playerScene;
        [SerializeField] private SceneReference gameOverScene;
        [SerializeField] private SceneReference winScene;
        
        [Header("Areas")]
        [SerializeField] private SceneReference[] areaScenes;
        [SerializeField] private GameObject[] areaEnemies;
        [SerializeField] private GameObject cutsceneCamera;
        
        private EventBinding<GameModeEvent> playerEventBinding;
        
        private static event Action LevelCompleted = () => { };
        
        [SerializeField] private int currentLevel;
        
        public static bool ExitingToMenu;
        
        public void Start() {
            PlayerManager.FriendlyFire = false;
            
            PauseMenu.Pause();
        }
        
        private void OnEnable() {
            playerEventBinding = new EventBinding<GameModeEvent>(PlayerEvent);
            EventBus<GameModeEvent>.Register(playerEventBinding);
            LevelCompleted += StartNextLevel;
        }
        
        private void OnDisable() {
            EventBus<GameModeEvent>.Deregister(playerEventBinding);
            LevelCompleted -= StartNextLevel;
        }
        
        private void PlayerEvent(GameModeEvent gameModeEvent) {
            if (ExitingToMenu) return;
            switch (gameModeEvent.Event) {
                case PlayerDeathEvent playerDeathEvent:
                    PlayerManager.PlayerDead(playerDeathEvent.PlayerScript);
                    if (PlayerManager.AlivePlayers.Count == 0) {
                        ExitingToMenu = true;
                        SceneManager.LoadScene(gameOverScene.Name);
                    }
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
            currentLevel = 0;
            Time.timeScale = 0;
            PlayerScript.Paused = true;
            
            cutsceneCamera.SetActive(false);
            SceneManager.LoadScene(playerScene.Name, LoadSceneMode.Additive);
            SceneManager.LoadScene(areaScenes[currentLevel].Name, LoadSceneMode.Additive);
            areaEnemies[currentLevel].SetActive(true);
        }
        
        private void StartNextLevel() {
            currentLevel++;
            
            Debug.Log($"StartNextLevel | {currentLevel}");
            
            
            
            if (currentLevel == areaScenes.Length) {
                SceneManager.LoadScene(winScene.Name);
                return;
            }
            
            SceneManager.UnloadSceneAsync(areaScenes[currentLevel - 1].Name);
            SceneManager.LoadScene(areaScenes[currentLevel].Name, LoadSceneMode.Additive);
            
            if (areaEnemies[currentLevel] == null) return;
            areaEnemies[currentLevel].SetActive(true);
            PlayerManager.ReviveAllPlayers();
            PlayerManager.SetSpawnForPlayers();
            UpgradeManager.SetUpgradesUI();
            PauseMenu.Pause();
        }
        
        public static void CallLevelCompleted() {
            LevelCompleted.Invoke();
        }
    }
}