using UnityEngine;

namespace Upgrades {
    [CreateAssetMenu(fileName = "AttackSpeedUpgrade", menuName = "Upgrade/new AttackSpeedUpgrade")]
    public class AttackSpeedUpgrade : Upgrade {
        public float attackSpeedIncrease;
        
        public void GetUpgrade(Animator animator) {
            animator.speed += attackSpeedIncrease;
        }
    }
}