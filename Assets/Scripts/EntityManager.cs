using System.Collections.Generic;
using ModeManagers;
using Player;
using UnityEngine;
using Utils;
using Utils.EventBus;

public class EntityManager : MonoBehaviour {
    public static readonly List<Enemy> TestEnemies = new ();
    
    public static (Vector3 targetPos, Vector3 playerPos) GetTargetPosition(Vector3 position, float range = 1f) {
        if (PlayerManager.AlivePlayers.Count == 0) return default;
        var closestPlayer = PlayerManager.AlivePlayers[0];
        for (var index = 1; index < PlayerManager.AlivePlayers.Count; index++) {
            if (Vector3.Distance(position, PlayerManager.AlivePlayers[index].transform1.position) < Vector3.Distance(position, closestPlayer.transform1.position)) {
                closestPlayer = PlayerManager.AlivePlayers[index];
            }
        }
        
        return (closestPlayer.transform1.position.WithOffset(position.x > closestPlayer.transform1.position.x ? range : -range),
            closestPlayer.transform1.position - position);
    }

    public static void EnemyDeath(Enemy enemy) {
        TestEnemies.Remove(enemy);
        if (TestEnemies.Count == 0) {
            EventBus<GameModeEvent>.Raise(new WaveOver());
        }
    }
}