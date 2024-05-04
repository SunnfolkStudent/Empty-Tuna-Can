using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Upgrades {
    public class UpgradeManager : MonoBehaviour {
        public static readonly List<UpgradesUI> UpgradesUI = new ();

        public static void SetUpgradesUI() {
            foreach (var ui in UpgradesUI.Where(u => u.playerScript != null)) {
                ui.SetRandomUpgrades();
            }
        }
        
        // private void Update() {
        //     if (Keyboard.current.numpad0Key.wasPressedThisFrame) {
        //         SetUpgradesUI();
        //     }
        // }
    }
}