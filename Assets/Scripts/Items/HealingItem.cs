using UnityEngine;
using Utils.CustomAttributes;

namespace Items {
    [CreateAssetMenu(fileName = "HealingItem", menuName = "Item/new HealingItem")]
    public class HealingItem : Item {
        [Header("Healing Item")] [RequiredField]
        public float healingAmount;

        public override void UseItem(PlayerScript playerScript) {
            base.UseItem(playerScript);
            playerScript.HandleReceiveHealing(healingAmount);
        }
    }
}