using UnityEngine;
using Utils.CustomAttributes;

namespace Items {
    public class Item : ScriptableObject {
        [Header("Item")]
        [RequiredField] [AssetPreviewIcon] public Sprite itemSprite;
        
        public ItemInstance[] getItemsOnUse;
        
        public virtual void UseItem(PlayerScript playerScript) {
            foreach (var item in getItemsOnUse) {
                playerScript.inventory.AddItems(item.item, item.amount);
            }
        }
    }
}