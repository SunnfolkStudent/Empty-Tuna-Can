using System;
using Items;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

public class PlayerUIElement : MonoBehaviour {
    [SerializeField] private Image selectedItemIcon;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform deathVisual;

    [SerializeField] private GameObject healingItemVisual;
    [SerializeField] private GameObject throwableItemVisual;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [FormerlySerializedAs("upgrades")] [SerializeField] private Upgrades.UpgradesUI upgradesUI;
    
    public PlayerScript playerScript;
    public InputSystemUIInputModule inputSystemUIInputModule;
    
    private Sprite _emptySprite;
    
    private void Awake() {
        _emptySprite = GetComponent<Image>().sprite;
        gameObject.SetActive(false);
        upgradesUI.playerScript = playerScript;
    }

    private void Start() {
        upgradesUI.playerScript = playerScript;
    }

    public void UpdateSelectedItemIcon([CanBeNull] Item selectedItem) {
        selectedItemIcon.sprite = selectedItem ? selectedItem.itemSprite : _emptySprite;
        
        healingItemVisual.SetActive(selectedItem is HealingItem);
        throwableItemVisual.SetActive(selectedItem is ThrowableItem);
    }

    public void UpdateSelectedItemAmount(int amount) {
        itemAmount.text = amount.ToString();
    }
    
    public void UpdateHealthBar(float value, float maxValue) {
        healthBar.localScale = healthBar.localScale.With(x: value / maxValue);
    }

    public void ShowDeath() {
        deathVisual.gameObject.SetActive(true);
    }
}