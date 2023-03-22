using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Simple_Inventory_System.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=2WnAOV7nHW0&ab_channel=CodeMonkey
    /// </summary>
    public class Player_InventorySys : MonoBehaviour
    {
        [SerializeField] private UI_Inventory_InventorySys _uiInventory;
        private Inventory_InventorySys _inventory;
        
        private void Start()
        {
            _inventory = new Inventory_InventorySys(UseItem);
            _uiInventory.SetPlayer(this);
            _uiInventory.SetInventory(_inventory);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out ItemWorld_InventorySys itemWorld))
            {
                _inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
        }

        private void UseItem(Item_InventorySys item)
        {
            switch (item.itemType)
            {
                case Item_InventorySys.ItemType.HealthPotion:
                    Debug.Log("Health Potion used");
                    _inventory.RemoveItem(new Item_InventorySys{itemType = Item_InventorySys.ItemType.HealthPotion,amount = 1});
                    break;
                case Item_InventorySys.ItemType.ManaPotion:
                    Debug.Log("Mana Potion used");
                    _inventory.RemoveItem(new Item_InventorySys{itemType = Item_InventorySys.ItemType.ManaPotion,amount = 1});
                    break;
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}