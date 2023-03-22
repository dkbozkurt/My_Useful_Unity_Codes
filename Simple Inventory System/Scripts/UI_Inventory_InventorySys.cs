using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simple_Inventory_System.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=2WnAOV7nHW0&ab_channel=CodeMonkey
    /// </summary>
    public class UI_Inventory_InventorySys : MonoBehaviour
    {
        [SerializeField] private Transform _itemSlotContainer;
        [SerializeField] private RectTransform _itemSlotTemplatePrefab;
        [SerializeField] private float _spacingOffSetValue = 10f;
        
        private Inventory_InventorySys _inventory;
        private Player_InventorySys _player;

        public void SetInventory(Inventory_InventorySys inventory)
        {
            this._inventory = inventory;
            inventory.OnItemListChanged += Invemtory_OnItemListChanged;
            RefreshInventoryItems();
        }

        public void SetPlayer(Player_InventorySys player)
        {
            _player = player;
        }

        private void Invemtory_OnItemListChanged(object sender, System.EventArgs e)
        {
            RefreshInventoryItems();
        }

        private void RefreshInventoryItems()
        {
            foreach (Transform child in _itemSlotContainer)
            {
                Destroy(child.gameObject);
            }
            int x = 0;
            int y = 0;
            float itemSlotCellSize = 
                _itemSlotTemplatePrefab.sizeDelta.x + _spacingOffSetValue;
            
            foreach (Item_InventorySys item in _inventory.GetItemList())
            {
                // DestroyAllItemTransforms();
                
                RectTransform itemSlotRectTransform =
                    Instantiate(_itemSlotTemplatePrefab, _itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
                
                ItemSlotTemplate_InventorySys itemSlotTemplate =
                    itemSlotRectTransform.GetComponent<ItemSlotTemplate_InventorySys>();
                
                // Use item
                itemSlotTemplate.MouseLeftClickFunc = () =>
                {
                    _inventory.UseItem(item);
                };

                // Drop item
                itemSlotTemplate.MouseRightClickFunc = () =>
                {
                    Item_InventorySys duplicateItem = new Item_InventorySys()
                        {itemType = item.itemType, amount = item.amount};
                    _inventory.RemoveItem(item);
                    ItemWorld_InventorySys.DropItem(_player.GetPosition(),duplicateItem);
                };
                

                Image itemSlotImage = itemSlotTemplate.ItemImage;
                itemSlotImage.sprite = item.GetSprite();
                
                TextMeshProUGUI itemSlotText = itemSlotTemplate.AmountText;

                var amountToSet = item.amount > 1 ? item.amount.ToString() : "";
                
                itemSlotText.SetText(amountToSet);
                
                
                x++;
                
                // Open a new line for items
                if (x > 4)
                {
                    x = 0;
                    y++;
                }
            }
        }

        private void DestroyAllItemTransforms()
        {
            foreach (Transform child in _itemSlotContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}