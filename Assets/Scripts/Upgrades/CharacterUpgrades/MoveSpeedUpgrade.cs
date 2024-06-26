﻿using Entities.Player;
using UnityEngine;

namespace Upgrades.CharacterUpgrades {
    [CreateAssetMenu(fileName = "MoveSpeedUpgrade", menuName = "Upgrade/CharacterUpgrade/new MoveSpeedUpgrade")]
    public class MoveSpeedUpgrade : CharacterUpgrade {
        public float moveSpeedIncrease;

        public override void GetUpgrade(PlayerScript playerScript) {
            playerScript.entityMovement.horizontalSpeed += moveSpeedIncrease;
            playerScript.entityMovement.verticalSpeed += moveSpeedIncrease / 3;
        }
    }
}