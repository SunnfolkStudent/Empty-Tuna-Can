using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    [CreateAssetMenu(fileName = "StatusEffectTypeUpgrade", menuName = "Upgrade/AttackUpgrade/new StatusEffectUpgrade")]
    public class StatusEffectTypeUpgrade : AttackUpgrade {
        public IStatusEffect StatusEffect;
    
        public override void GetUpgrade(DamageInstance damageInstance) {
            base.GetUpgrade(damageInstance);
            damageInstance.StatusEffect = StatusEffect;
        }
    }
}