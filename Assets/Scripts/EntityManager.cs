using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EntityManager : MonoBehaviour {
    public static readonly List<Transform> PlayersTransforms = new ();
    public static readonly List<TestEnemy> TestEnemies = new ();
    
    private readonly Func<bool> playersExists = () => PlayersTransforms.Count > 0;
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
        var closestPlayer = PlayersTransforms[0];
        for (var index = 1; index < PlayersTransforms.Count; index++) {
            if (Vector3.Distance(position, PlayersTransforms[index].position) < Vector3.Distance(position, closestPlayer.position)) {
                closestPlayer = PlayersTransforms[index];
            }
        }
        
        return closestPlayer.position.WithOffset(position.x > closestPlayer.position.x ? range : -range);
    }
}