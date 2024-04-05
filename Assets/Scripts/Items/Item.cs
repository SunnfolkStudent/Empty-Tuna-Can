using UnityEngine;
using Utils.CustomAttributes;

namespace Items {
    public class Item : ScriptableObject {
        [Header("Item")] [RequiredField] [AssetPreviewIcon]
        public GameObject itemPrefab;

        public Item[] getItemsOnUse;
        
        public virtual void UseItem(PlayerScript playerScript) {
            playerScript.item.Remove(playerScript.selectedItem);

            foreach (var item in getItemsOnUse) {
                playerScript.item.Add(item);
            }

            if (playerScript.item.Count == 0) {
                playerScript.selectedItem = null;
                return;
            }

            playerScript.selectedItem = playerScript.item[0];
        }
    }
}