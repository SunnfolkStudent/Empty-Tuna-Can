using UnityEngine;
using Utils.CustomAttributes;

namespace Items {
    public class Item : ScriptableObject {
        [Header("Item")]
        [RequiredField] [AssetPreviewIcon] public Sprite itemSprite;

        public Item[] getItemsOnUse;
        
        public virtual void UseItem(PlayerScript playerScript) {
            playerScript.itemInventory.Remove(playerScript.SelectedItem);

            foreach (var item in getItemsOnUse) {
                playerScript.itemInventory.Add(item);
            }

            if (playerScript.itemInventory.Count == 0) {
                playerScript.SelectedItem = null;
                return;
            }

            playerScript.SelectedItem = playerScript.itemInventory[0];
        }
    }
}