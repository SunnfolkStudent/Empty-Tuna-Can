using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    [CreateAssetMenu(fileName = "StatusEffectTypeUpgrade", menuName = "Upgrade/AttackUpgrade/new StatusEffectUpgrade")]
    public class StatusEffectTypeUpgrade : AttackUpgrade {
        public StatusEffect StatusEffect;
    
        public override void GetUpgrade(DamageInstance damageInstance) {
            damageInstance.StatusEffect = StatusEffect;
        }
    }
}