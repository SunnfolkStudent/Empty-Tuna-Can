using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EntityManager : MonoBehaviour {
    public static readonly List<TestEnemy> TestEnemies = new ();
    
    private readonly Func<bool> playersExists = () => PlayerManager.AlivePlayersTransforms.Count > 0;
    private readonly Func<bool> enemiesExists = () => TestEnemies.Count > 0;
    
    private void Start() {
        StartCoroutine(EnumeratorFunctions.WaitUntilCondition(playersExists, () => StartCoroutine(EnumeratorFunctions.ActionAtInterval(5, () => {
            if (!enemiesExists()) {
                StopAllCoroutines();
                return;
            }
            
            var e = TestEnemies.GetRandom();
            StartCoroutine(EnumeratorFunctions.ActionDuringTime(2, () => e.MoveToPosition(GetTargetPosition(e.transform.position))));
            StartCoroutine(EnumeratorFunctions.ActionAfterTime(2, () => e.Attack()));
        }))));
    }
    
    public static Vector3 GetTargetPosition(Vector3 position, float range = 3f) {
        var closestPlayer = PlayerManager.AlivePlayersTransforms[0];
        for (var index = 1; index < PlayerManager.AlivePlayersTransforms.Count; index++) {
            if (Vector3.Distance(position, PlayerManager.AlivePlayersTransforms[index].position) < Vector3.Distance(position, closestPlayer.position)) {
                closestPlayer = PlayerManager.AlivePlayersTransforms[index];
            }
        }
        
        return closestPlayer.position.WithOffset(position.x > closestPlayer.position.x ? range : -range);
    }
}