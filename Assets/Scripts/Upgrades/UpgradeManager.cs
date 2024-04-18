using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Upgrades {
    public class UpgradeManager : MonoBehaviour {
        public static readonly List<UpgradesUI> UpgradesUI = new ();

        private static void SetUpgradesUI() {
            foreach (var ui in UpgradesUI.Where(u => u.playerScript != null)) {
                ui.SetRandomUpgrades();
            }
        }
        
        private void Update() {
            if (Keyboard.current.numpad0Key.wasPressedThisFrame) {
                SetUpgradesUI();
            }
        }
        
#if UNITY_EDITOR
        [CustomEditor(typeof(UpgradeManager))]
       internal class DecalMeshHelperEditor : Editor {
           public override void OnInspectorGUI() {
               if (!GUILayout.Button("GiveUpgrade")) return;
               Debug.Log("GiveUpgrades");
               SetUpgradesUI();
           }
       }
    }
#endif
}
