using Player;
using UnityEngine;

namespace Upgrades.CharacterUpgrades {
    [CreateAssetMenu(fileName = "MaxHealthUpgrade", menuName = "Upgrade/CharacterUpgrade/new MaxHealthUpgrade")]
    public class MaxHealthUpgrade : CharacterUpgrade {
        public float maxHealthIncrease;
    
        public override void GetUpgrade(PlayerScript playerScript) {
            playerScript.health.maxValue += maxHealthIncrease;
            playerScript.health.Value += maxHealthIncrease;
        }
    }
}