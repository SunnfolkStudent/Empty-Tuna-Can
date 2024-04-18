using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    [CreateAssetMenu(fileName = "StaggerUpgrade", menuName = "Upgrade/new StaggerUpgrade")]
    public class StaggerUpgrade : Upgrade {
        public float staggerIncrease;
    
        public override void GetUpgrade(DamageInstance damageInstance) {
            base.GetUpgrade(damageInstance);
            damageInstance.stagger += staggerIncrease;
        }
    }
}