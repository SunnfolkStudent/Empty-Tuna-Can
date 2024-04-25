using Entities.Player;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using Utils;

namespace Upgrades {
    public class UpgradesUI : MonoBehaviour {
        [SerializeField] private UpgradeSlot[] upgradeSlots;
        [SerializeField] private MultiplayerEventSystem eventSystem;
        public PlayerScript playerScript;
        
        private void Awake() {
            UpgradeManager.UpgradesUI.Add(this);
            foreach (var upgradeSlot in upgradeSlots) {
                upgradeSlot.upgradeUI = this;
            }
            
            eventSystem.gameObject.SetActive(false);
        }
        
        private void ShowUpgrades() {
            foreach (var upgradeSlot in upgradeSlots) {
                upgradeSlot.gameObject.SetActive(true);
            }
            
            eventSystem.gameObject.SetActive(true);
        }
        
        private void HideUpgrades() {
            foreach (var upgradeSlot in upgradeSlots) {
                upgradeSlot.gameObject.SetActive(false);
            }
            
            eventSystem.gameObject.SetActive(false);
        }
        
        public void SetRandomUpgrades() {
            foreach (var upgradeSlot in upgradeSlots) {
                upgradeSlot.upgrade = playerScript.allUpgrades.GetRandom();
                upgradeSlot.UpdateSlot();
            }
            
            ShowUpgrades();
        }
        
        public void GetUpgrade(Upgrade upgrade) {
            playerScript.GetUpgrade(upgrade);
            Debug.Log("Upgrade selected");
            HideUpgrades();
        }
    }
}