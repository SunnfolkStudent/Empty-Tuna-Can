using System.Linq;
using UnityEngine;
using Utils;

namespace Player {
    public static class PlayerUIFactory {
        public static void CreatePlayerUI(PlayerScript playerScript) {
            var o = GameObject.FindWithTag("PlayerUI").transform.GetImmediateChildren().First(i => !i.activeSelf);
            o.SetActive(true);
        
            var playerUIElement = o.GetComponent<PlayerUIElement>();
        
            playerScript.OnDying += playerUIElement.ShowDeath;
        
            playerScript.health.OnValueChanged += (value) => playerUIElement.UpdateHealthBar(value, playerScript.health.maxValue);
            playerUIElement.UpdateHealthBar(playerScript.health.Value, playerScript.health.maxValue);
        
            playerScript.inventory.selectedItem.OnValueChanged += (value) => playerUIElement.UpdateSelectedItemIcon(value.item);
            playerScript.inventory.selectedItem.OnValueChanged += (value) => playerUIElement.UpdateSelectedItemAmount(value.amount);
        
            playerUIElement.UpdateSelectedItemIcon(playerScript.inventory.selectedItem.Value.item);
            playerUIElement.UpdateSelectedItemAmount(playerScript.inventory.selectedItem.Value.amount);

            playerScript.input.uiInputModule = playerUIElement.inputSystemUIInputModule;

            playerUIElement.playerScript = playerScript;
        }
    }
}