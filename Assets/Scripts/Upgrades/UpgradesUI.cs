using UnityEngine;
using Utils;

namespace Upgrades {
    public class UpgradesUI : MonoBehaviour {
        [SerializeField] private UpgradeSlot[] upgradeSlots;
        public PlayerScript playerScript;
        
        [SerializeField] private Upgrade[] allUpgrades;
        
        private void Awake() {
            allUpgrades = ScrubUtils.GetAllScrubsInResourceFolder<Upgrade>("Upgrades");
            UpgradeManager.UpgradesUI.Add(this);
            foreach (var upgradeSlot in upgradeSlots) {
                upgradeSlot.upgradeUI = this;
            }
        }
        
        private void ShowUpgrades() {
            foreach (var upgradeSlot in upgradeSlots) {
                upgradeSlot.gameObject.SetActive(true);
            }
        }
        
        private void HideUpgrades() {
            foreach (var upgradeSlot in upgradeSlots) {
                upgradeSlot.gameObject.SetActive(false);
            }
        }
        
        public void SetRandomUpgrades() {
            foreach (var upgradeSlot in upgradeSlots) {
                upgradeSlot.upgrade = allUpgrades.GetRandom();
                upgradeSlot.playerScript = playerScript;
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