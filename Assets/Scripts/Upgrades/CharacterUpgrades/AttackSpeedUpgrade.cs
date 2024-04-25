using Player;
using UnityEngine;

namespace Upgrades.CharacterUpgrades {
    [CreateAssetMenu(fileName = "AttackSpeedUpgrade", menuName = "Upgrade/CharacterUpgrade/new AttackSpeedUpgrade")]
    public class AttackSpeedUpgrade : CharacterUpgrade {
        public float attackSpeedIncrease;
        
        public override void GetUpgrade(PlayerScript playerScript) {
            playerScript.animator.speed += attackSpeedIncrease;
        }
    }
}