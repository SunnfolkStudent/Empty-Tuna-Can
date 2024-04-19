using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using Utils;

public static class PlayerManager {
    public static List<Transform> AlivePlayersTransforms = new ();
    public static List<Transform> AllPlayersTransforms = new ();
    public static List<PlayerScript> AllPlayersScripts = new ();
    public static bool FriendlyFire = true;
    
    public static void RegisterPlayer(PlayerScript playerScript) {
        AllPlayersScripts.Add(playerScript);
        AllPlayersTransforms.Add(playerScript.transform);
        AlivePlayersTransforms.Add(playerScript.transform);
    }
    
    public static void DeregisterPlayer(PlayerScript playerScript) {
        AllPlayersScripts.Remove(playerScript);
        AllPlayersTransforms.Remove(playerScript.transform);
        AlivePlayersTransforms.Remove(playerScript.transform);
    }
    
    public static void ResetAlivePlayerTransforms() {
        AlivePlayersTransforms = AllPlayersTransforms;
    }
    
    public static void PlayerDead(PlayerScript playerScript) {
        AlivePlayersTransforms.Remove(playerScript.transform);
    }
    
    public static Vector3 GetSpawnPosition() {
        var gameObject = GameObject.FindWithTag("PlayerSpawnPositions").transform.GetImmediateChildren()
            .First(o => o.activeSelf);
        gameObject.SetActive(false);
        return gameObject.transform.position;
    }
    
    public static void ResetAvailableSpawnPositions() {
        var p = GameObject.FindWithTag("PlayerSpawnPositions").transform.GetImmediateChildren();
        foreach (var o in p) {
            o.SetActive(true);
        }
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void SetSpawnForPlayers() {
        foreach (var playerScript in AllPlayersScripts) {
            playerScript.transform.position = GetSpawnPosition();
        }
    }
}