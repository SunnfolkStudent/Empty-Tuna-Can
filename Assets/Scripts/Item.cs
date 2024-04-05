using UnityEngine;
using Utils.CustomAttributes;

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

[CreateAssetMenu(fileName = "ThrowableItem", menuName = "Item/new ThrowableItem")]
public class ThrowableItem : Item {
    public float velocity;

    public override void UseItem(PlayerScript playerScript) {
        base.UseItem(playerScript);
        playerScript.ThrowItem(this, velocity);
    }
}

[CreateAssetMenu(fileName = "HealingItem", menuName = "Item/new HealingItem")]
public class HealingItem : Item {
    [Header("Healing Item")]
    [RequiredField] public float healingAmount;

    public override void UseItem(PlayerScript playerScript) {
        base.UseItem(playerScript);
        playerScript.HandleReceiveHealing(healingAmount);
    }
}