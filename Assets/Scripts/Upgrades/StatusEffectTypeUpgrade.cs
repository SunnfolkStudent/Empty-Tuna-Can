using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    [CreateAssetMenu(fileName = "StatusEffectTypeUpgrade", menuName = "Upgrade/new StatusEffectUpgrade")]
    public class StatusEffectTypeUpgrade : Upgrade {
        public IStatusEffect StatusEffect;
    
        public override void GetUpgrade(DamageInstance damageInstance) {
            base.GetUpgrade(damageInstance);
            damageInstance.StatusEffect = StatusEffect;
        }
    }
}