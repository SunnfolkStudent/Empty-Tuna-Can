using System;
using System.Collections.Generic;
using Items;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Entities.Player {
    [Serializable] public class Inventory {
        [SerializeField] private List<ItemInstance> inventory;
        public Observable<ItemInstance> selectedItem;
    
        public void Initialize() {
            UpdateSelectedItem(0);
        }
    
        #region ---Item---
        public void UseSelectedItem(PlayerScript playerScript) {
            if (selectedItem.Value.amount <= 0) return;
            selectedItem.Value.item.UseItem(playerScript);
            RemoveItem(inventory[GetIndexOfItem(selectedItem.Value.item)]);
        }

        public void AddItem(Item item) {
            var existingItem = inventory.Find(i => i.item != null && i.item == item);
            if (existingItem is not null) {
                existingItem.amount += 1;
                selectedItem.ForceUpdate();
            } 
            else {
                inventory.Add(new ItemInstance {
                    item = item,
                    amount = 1
                });
            }
        }

        public void AddItems(Item item, int count) {
            for (var i = 0; i < count; i++) {
                AddItem(item);
            }
        }
    
        public void RemoveItem(ItemInstance item) {
            item.amount -= 1;
            selectedItem.ForceUpdate();

            if (item.amount <= 0) {
                inventory.Remove(item);
                PreviousItem();
            }
        }
        #endregion

        #region ---SelectedItem---
        public void NextItem() {
            if (selectedItem.Value.item == null) return;
            var indexOfNextItem = GetIndexOfItem(selectedItem.Value.item) + 1;
            if (inventory.Count <= indexOfNextItem) return;
        
            UpdateSelectedItem(indexOfNextItem);
        }
    
        public void PreviousItem() {
            if (selectedItem.Value.item == null) return;
            var indexOfPreviousItem = GetIndexOfItem(selectedItem.Value.item) - 1;
            indexOfPreviousItem = math.max(0, indexOfPreviousItem);
        
            UpdateSelectedItem(indexOfPreviousItem);
        }

        private void UpdateSelectedItem(int index) {
            if (inventory.Count == 0) return;
            selectedItem.Value = inventory[index];
        }
    
        public int GetIndexOfItem(Item item) {
            for (var i = 0; i < inventory.Count; i++) {
                var observable = inventory[i].item;
                if (observable != null && observable == item) {
                    return i;
                }
            }
            return -1;
        }
        #endregion
    }

    [Serializable] public class ItemInstance {
        public Item item;
        public int amount;
    }
}