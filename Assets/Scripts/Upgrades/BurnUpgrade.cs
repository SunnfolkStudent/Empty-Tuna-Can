using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    [CreateAssetMenu(fileName = "BurnUpgrade", menuName = "Upgrade/AttackUpgrade/new BurnUpgrade")]
    public class BurnUpgrade : AttackUpgrade {
        public float durationIncrease;
        public float damageIncrease;
        public float damageFrequencyIncrease;
    
        public override void GetUpgrade(DamageInstance damageInstance) {
            if (damageInstance.StatusEffect is not Burn burn) return;
            burn.duration += durationIncrease;
            burn.damage += damageIncrease;
            burn.damageFrequency += damageFrequencyIncrease;

        }
    }
}