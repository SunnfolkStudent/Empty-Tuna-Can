using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static List<Transform> AlivePlayersTransforms = new ();
    public static List<Transform> AllPlayersTransforms = new ();

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
    
    public static void RegisterPlayer(PlayerScript playerScript) {
        AllPlayersTransforms.Add(playerScript.transform);
        AlivePlayersTransforms.Add(playerScript.transform);
    }
    
    public static void ResetAlivePlayerTransforms() {
        AlivePlayersTransforms = AllPlayersTransforms;
    }
    
    public static void PlayerDead(PlayerScript playerScript) {
        AlivePlayersTransforms.Remove(playerScript.transform);
    }
}