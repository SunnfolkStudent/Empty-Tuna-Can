using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    [CreateAssetMenu(fileName = "StaggerUpgrade", menuName = "Upgrade/AttackUpgrade/new StaggerUpgrade")]
    public class StaggerUpgrade : AttackUpgrade {
        public float staggerIncrease;
    
        public override void GetUpgrade(DamageInstance damageInstance) {
            base.GetUpgrade(damageInstance);
            damageInstance.stagger += staggerIncrease;
        }
    }
}