using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simple_Inventory_System.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=2WnAOV7nHW0&ab_channel=CodeMonkey
    /// </summary>
    public class Inventory_InventorySys
    {
        public event EventHandler OnItemListChanged;

        private Action<Item_InventorySys> _useItemAction;
        private List<Item_InventorySys> _itemList;

        public Inventory_InventorySys(Action<Item_InventorySys> useItemAction)
        {
            _useItemAction = useItemAction;
            _itemList = new List<Item_InventorySys>();
            TestAddItems();
        }

        private void TestAddItems()
        {
            AddItem(new Item_InventorySys{itemType = Item_InventorySys.ItemType.Sword,amount = 1});
            AddItem(new Item_InventorySys{itemType = Item_InventorySys.ItemType.HealthPotion,amount = 1});
            AddItem(new Item_InventorySys{itemType = Item_InventorySys.ItemType.ManaPotion,amount = 1});
            Debug.Log(_itemList.Count);
        }

        public void AddItem(Item_InventorySys item)
        {
            if (item.IsStackable())
            {

                bool itemAlreadyExist = false;

                foreach (Item_InventorySys inventoryItem in _itemList)
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        inventoryItem.amount += item.amount;
                        itemAlreadyExist = true;
                    }
                }
                if(!itemAlreadyExist) _itemList.Add(item);
            }
            else
            {
                _itemList.Add(item);    
            }
            
            OnItemListChanged?.Invoke(this,EventArgs.Empty);
        }

        public void RemoveItem(Item_InventorySys item)
        {
            if (item.IsStackable())
            {
                Item_InventorySys itemInInventory = null;
                foreach (Item_InventorySys inventoryItem in _itemList)
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        inventoryItem.amount -= item.amount;
                        itemInInventory = inventoryItem;
                    }
                }

                if (itemInInventory != null && itemInInventory.amount <= 0)
                {
                    _itemList.Remove(itemInInventory);
                }
            }
            else
            {
                _itemList.Remove(item);    
            }
            
            OnItemListChanged?.Invoke(this,EventArgs.Empty);
        }

        public void UseItem(Item_InventorySys item)
        {
            _useItemAction(item);
        }

        public List<Item_InventorySys> GetItemList()
        {
            return _itemList;
        }
    }
}