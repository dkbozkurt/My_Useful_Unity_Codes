using System;
using UnityEngine;

namespace Simple_Inventory_System.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=2WnAOV7nHW0&ab_channel=CodeMonkey
    /// </summary>
    [Serializable]
    public class Item_InventorySys
    {
        public ItemType itemType = ItemType.Sword;
        public int amount;

        public Sprite GetSprite()
        {
            switch (itemType)
            {
                default:
                case ItemType.Sword:            return ItemAssets_InventorySys.Instance.SwordSprite;
                case ItemType.HealthPotion:     return ItemAssets_InventorySys.Instance.HealthPotionSprite;
                case ItemType.ManaPotion:       return ItemAssets_InventorySys.Instance.ManaPotionSprite;
                case ItemType.Coin:             return ItemAssets_InventorySys.Instance.CoinSprite;
                case ItemType.MedKit:           return ItemAssets_InventorySys.Instance.MedKitSprite;
            }
        }

        public bool IsStackable()
        {
            switch (itemType)
            {
                default:
                case ItemType.Coin:
                case ItemType.HealthPotion:
                case ItemType.ManaPotion:
                    return true;
                case ItemType.Sword:
                case ItemType.MedKit:
                    return false;

            }
        }
        public enum ItemType
        {
            Sword,
            HealthPotion,
            ManaPotion,
            Coin,
            MedKit,
        }
        
        
    }
    
    
}

