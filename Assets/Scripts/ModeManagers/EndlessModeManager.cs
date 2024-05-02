using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;
using Entities.Player;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Upgrades;
using Utils;
using Utils.EventBus;
using Random = UnityEngine.Random;

namespace ModeManagers {
    public class EndlessModeManager : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI waveCounter;
        [SerializeField] private SceneReference[] endlessModeScenes;
        [SerializeField] private SceneReference gameOverScene;
        [SerializeField] private Transform enemySpawnPosition;
        private Transform[] _enemySpawnPositions;
        
        [SerializeField] private EnemyType[] enemies;
        [SerializeField] private float waveBudget;
        
        [Serializable] private struct EnemyType {
            [SerializeField] internal GameObject prefab;
            [SerializeField] internal float cost;
            [SerializeField] internal float spawnProbability;
        }
        
        private EventBinding<GameModeEvent> gameModeEventBinding;
        private event Action WaveCompleted = () => { };
        public Observable<int> currentWave;
        
        private void Awake() {
            currentWave.Value = 0;
            SetEnemySpawnPositions();
            
            SceneManager.LoadScene(endlessModeScenes.GetRandom().Name, LoadSceneMode.Additive);
            
            WaveCompleted += PlayerManager.ReviveAllPlayers;
            WaveCompleted += UpgradeManager.SetUpgradesUI;
            WaveCompleted += PauseMenu.Pause;
            WaveCompleted += StartNextWave;
            
            currentWave.OnValueChanged += value => waveCounter.text = "Wave: " + value;
        }
        
        private void OnEnable() {
            gameModeEventBinding = new EventBinding<GameModeEvent>(PlayerEvent);
            EventBus<GameModeEvent>.Register(gameModeEventBinding);
        }
        
        private void OnDisable() {
            EventBus<GameModeEvent>.Deregister(gameModeEventBinding);
        }
        
        public void Start() {
            PlayerManager.FriendlyFire = false;
            
            PauseMenu.Pause();
            
            StartNextWave();
        }
        
        private void SetEnemySpawnPositions() => _enemySpawnPositions = enemySpawnPosition.GetImmediateChildren().Select(x => x.transform).ToArray();
        
        private void PlayerEvent(GameModeEvent gameModeEvent) {
            switch (gameModeEvent.Event) {
                case PlayerDeathEvent playerDeathEvent:
                    PlayerManager.PlayerDead(playerDeathEvent.PlayerScript);
                    if (PlayerManager.AlivePlayers.Count == 0) SceneManager.LoadScene(gameOverScene.Name);
                    break;
                case WaveOver:
                    WaveCompleted.Invoke();
                    break;
            }
        }
        
        private void StartNextWave() {
            currentWave.Value++;
            Debug.Log("***Start Wave " + currentWave.Value);
            
            waveBudget = 2 + 2 * currentWave.Value;
            var remainingBudget = waveBudget;
            var enemiesToSpawn = new List<EnemyType>();
        
            while (remainingBudget > 0) {
                var selectedEnemy = SelectEnemyTypeWithBias();
                
                if (selectedEnemy.cost <= remainingBudget) {
                    enemiesToSpawn.Add(selectedEnemy);
                    remainingBudget -= selectedEnemy.cost;
                }
                else {
                    if (enemies.OrderBy(x => x.cost).LastOrDefault().cost < remainingBudget) break;
                }
            }
            
            foreach (var enemyType in enemiesToSpawn) {
                SpawnEnemy(enemyType);
            }
        }

        private EnemyType SelectEnemyTypeWithBias() {
            var totalWeight = enemies.Sum(enemy => enemy.spawnProbability);
            var rnd = Random.Range(0, totalWeight);
            
            float sum = 0;
            foreach (var enemyType in enemies) {
                sum += enemyType.spawnProbability;
                if (sum < rnd) continue;
                return enemyType;
            }
            
            return default;
        }
        
        private void SpawnEnemy(EnemyType enemyType) {
            if (_enemySpawnPositions == null) SetEnemySpawnPositions();
            var spawnPos = _enemySpawnPositions.GetRandom().position;
            Instantiate(enemyType.prefab, spawnPos, quaternion.identity);
        }
    }
}