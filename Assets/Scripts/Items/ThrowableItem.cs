using UnityEngine;

namespace Items {
    [CreateAssetMenu(fileName = "ThrowableItem", menuName = "Item/new ThrowableItem")]
    public class ThrowableItem : Item {
        public float velocity;

        public override void UseItem(PlayerScript playerScript) {
            base.UseItem(playerScript);
            playerScript.ThrowItem(this);
        }
    }
}