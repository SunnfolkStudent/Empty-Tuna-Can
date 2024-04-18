using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    [CreateAssetMenu(fileName = "FreezeUpgrade", menuName = "Upgrade/AttackUpgrade/new FreezeUpgrade")]
    public class FreezeUpgrade : AttackUpgrade {
        public float durationIncrease;
    
        public override void GetUpgrade(DamageInstance damageInstance) {
            if (damageInstance.StatusEffect is not Freeze freeze) return;
            freeze.duration += durationIncrease;
        }
    }
}