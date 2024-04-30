using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Entities.Player {
    public static class PlayerManager {
        public static readonly List<PlayerScript> AllPlayers = new ();
        public static readonly List<PlayerScript> AlivePlayers = new ();
        public static bool FriendlyFire = true;
        
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        public static void RegisterPlayer(PlayerScript playerScript) {
            AllPlayers.Add(playerScript);
            AlivePlayers.Add(playerScript);
        }
    
        public static void DeregisterPlayer(PlayerScript playerScript) {
            AllPlayers.Remove(playerScript);
            AlivePlayers.Remove(playerScript);
        }
    
        public static void DeregisterAllPlayers() {
            AllPlayers.Clear();
            AlivePlayers.Clear();
        }
    
        public static void ReviveAllPlayers() {
            AlivePlayers.Clear();
            foreach (var playerScript in AllPlayers) {
                playerScript.dead = false;
                playerScript.health.Value = playerScript.health.maxValue;
                playerScript.StopStatusEffects();
                playerScript.animator.SetBool(IsDead, false);
                AlivePlayers.Add(playerScript);
            }
        }

        public static void HealAllPlayers() {
            foreach (var playerScript in AllPlayers) {
                playerScript.ReceiveFullHealing();
            }
        }
    
        public static void PlayerDead(PlayerScript playerScript) {
            AlivePlayers.Remove(playerScript);
            playerScript.dead = true;
            playerScript.movementEnabled = false;
        }
    
        public static Vector3 GetAvailableSpawnPosition() {
            var gameObject = GetSpawnPositions().First(o => o.activeSelf);
            gameObject.SetActive(false);
            return gameObject.transform.position;
        }
    
        public static void ResetAvailableSpawnPositions() {
            var p = GetSpawnPositions();
            foreach (var o in p) {
                o.SetActive(true);
            }
        }
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void SetSpawnForPlayers() {
            var spawnPos = GetSpawnPositions();
            if (spawnPos != null) {
                foreach (var o in spawnPos) {
                    o.gameObject.SetActive(true);
                } 
            }
        
            foreach (var playerScript in AllPlayers) {
                playerScript.transform.position = GetAvailableSpawnPosition();
            }
        }
    
        private static IEnumerable<GameObject> GetSpawnPositions() {
            return GameObject.FindWithTag("PlayerSpawnPositions")?.transform.GetImmediateChildren();
        }
    }
}