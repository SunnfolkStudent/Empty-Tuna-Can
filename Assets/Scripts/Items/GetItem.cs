using Player;
using UnityEngine;

namespace Items {
    public class GetItem : InteractBase {
        [SerializeField] private ItemInstance item;
        protected override void InteractionLogic(PlayerScript player) {
            player.inventory.AddItems(item.item, item.amount);
        }
    }
}