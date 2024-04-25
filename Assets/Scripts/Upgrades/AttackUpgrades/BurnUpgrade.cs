using UnityEngine;
using Utils.Entity;

namespace Upgrades.AttackUpgrades {
    [CreateAssetMenu(fileName = "BurnUpgrade", menuName = "Upgrade/AttackUpgrade/new BurnUpgrade")]
    public class BurnUpgrade : AttackUpgrade {
        public float damageIncrease;
        public float durationIncrease;
        public float damageFrequencyIncrease;
        
        public override void GetUpgrade(DamageInstance damageInstance) {
            if (damageInstance.StatusEffect is not Burn burn) {
                damageInstance.StatusEffect = new Burn(damageIncrease, durationIncrease, damageFrequencyIncrease);
                return;
            }
            burn.duration += durationIncrease;
            burn.damage += damageIncrease;
            burn.damageFrequency += damageFrequencyIncrease;
        }
    }
}