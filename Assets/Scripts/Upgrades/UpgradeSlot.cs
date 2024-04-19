using TMPro;
using UnityEngine;

namespace Upgrades {
    public class UpgradeSlot : MonoBehaviour {
        public TextMeshProUGUI textField;
        public Upgrade upgrade;
        public UpgradesUI upgradeUI;
    
        public void UpdateSlot() {
            textField.text = upgrade.upgradeName;
        }
        
        public void GetUpgrade() {
            upgradeUI.GetUpgrade(upgrade);
        }
    }
}