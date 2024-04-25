using Entities;
using UnityEngine;
using Utils.Entity;

namespace Upgrades.AttackUpgrades {
    [CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Upgrade/AttackUpgrade/new DamageUpgrade")]
    public class DamageUpgrade : AttackUpgrade {
        public float damageIncrease;

        public override void GetUpgrade(DamageInstance damageInstance) {
            damageInstance.damage += damageIncrease;
        }
    }
}