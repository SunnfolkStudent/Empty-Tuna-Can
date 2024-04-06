using UnityEngine;
using Utils.CustomAttributes;

namespace Items {
    public class Item : ScriptableObject {
        [Header("Item")]
        [RequiredField] [AssetPreviewIcon] public Sprite itemSprite;

        public Item[] getItemsOnUse;
        
        public virtual void UseItem(PlayerScript playerScript) {
            playerScript.item.Remove(playerScript.SelectedItem);

            foreach (var item in getItemsOnUse) {
                playerScript.item.Add(item);
            }

            if (playerScript.item.Count == 0) {
                playerScript.SelectedItem = null;
                return;
            }

            playerScript.SelectedItem = playerScript.item[0];
        }
    }
}