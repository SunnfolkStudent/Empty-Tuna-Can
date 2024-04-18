﻿using UnityEngine;

namespace Upgrades {
    [CreateAssetMenu(fileName = "AttackSpeedUpgrade", menuName = "Upgrade/AttackUpgrade/new AttackSpeedUpgrade")]
    public class AttackSpeedUpgrade : CharacterUpgrade {
        public float attackSpeedIncrease;
        
        public override void GetUpgrade(PlayerScript playerScript) {
            playerScript.animator.speed += attackSpeedIncrease;
        }
    }
}