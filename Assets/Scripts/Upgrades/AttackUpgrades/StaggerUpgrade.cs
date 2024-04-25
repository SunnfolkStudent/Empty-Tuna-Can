using UnityEngine;
using Utils.Entity;

namespace Upgrades.AttackUpgrades {
    [CreateAssetMenu(fileName = "StaggerUpgrade", menuName = "Upgrade/AttackUpgrade/new StaggerUpgrade")]
    public class StaggerUpgrade : AttackUpgrade {
        public float staggerIncrease;
    
        public override void GetUpgrade(DamageInstance damageInstance) {
            damageInstance.stagger += staggerIncrease;
        }
    }
}