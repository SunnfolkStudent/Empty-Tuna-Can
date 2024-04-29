using Entities;
using Entities.Player;
using UnityEngine;
using Utils.CustomAttributes;

namespace Items {
    [CreateAssetMenu(fileName = "ThrowableItem", menuName = "Item/new ThrowableItem")]
    public class ThrowableItem : Item {
        [Header("ThrowableItem")]
        [RequiredField] [AssetPreviewIcon] public GameObject itemPrefab;
        public DamageInstance damageInstance;
        
        public override void UseItem(PlayerScript playerScript) {
            base.UseItem(playerScript);
            playerScript.ThrowItem(this);
        }
    }
}