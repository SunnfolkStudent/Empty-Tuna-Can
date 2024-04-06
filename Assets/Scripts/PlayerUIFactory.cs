using System.Linq;
using UnityEngine;
using Utils;

public static class PlayerUIFactory {
    public static void CreatePlayerUI(PlayerScript playerScript) {
        var o = GameObject.FindWithTag("PlayerUI").transform.GetImmediateChildren().First(i => !i.activeSelf);
        o.SetActive(true);
        
        var playerUIElement = o.GetComponent<PlayerUIElement>();
        
        playerScript.OnDying += playerUIElement.ShowDeath;
        
        playerScript.health.OnValueChanged += (value) => playerUIElement.UpdateHealthBar(value, playerScript.health.maxValue);
        playerUIElement.UpdateHealthBar(playerScript.health.Value, playerScript.health.maxValue);
        
        playerScript.OnSelectItemChanged += playerUIElement.UpdateSelectedItemIcon;
        playerUIElement.UpdateSelectedItemIcon(playerScript.SelectedItem != null ? playerScript.SelectedItem : null);
    }
}