using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    [CreateAssetMenu(fileName = "FreezeUpgrade", menuName = "Upgrade/new FreezeUpgrade")]
    public class FreezeUpgrade : Upgrade {
        public float durationIncrease;
    
        public override void GetUpgrade(DamageInstance damageInstance) {
            base.GetUpgrade(damageInstance);
            if (damageInstance.StatusEffect is not Freeze freeze) return;
            freeze.duration += durationIncrease;
        }
    }
}