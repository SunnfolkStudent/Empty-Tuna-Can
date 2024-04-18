using UnityEngine;
using Utils.Entity;

namespace Upgrades {
    [CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Upgrade/new DamageUpgrade")]
    public class DamageUpgrade : Upgrade {
        public float damageIncrease;

        public override void GetUpgrade(DamageInstance damageInstance) {
            base.GetUpgrade(damageInstance);
            damageInstance.damage += damageIncrease;
        }
    }
}