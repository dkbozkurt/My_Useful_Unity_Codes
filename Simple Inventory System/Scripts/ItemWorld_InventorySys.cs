using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Simple_Inventory_System.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=2WnAOV7nHW0&ab_channel=CodeMonkey
    /// </summary>
    public class ItemWorld_InventorySys : MonoBehaviour
    {
        public TextMeshProUGUI ItemWorldAmountText;
        
        public static ItemWorld_InventorySys SpawnItemWorld(Vector3 position, Item_InventorySys item)
        {
            Transform transform =
                Instantiate(ItemAssets_InventorySys.Instance.ItemWorld, position, quaternion.identity);

            ItemWorld_InventorySys itemWorld = transform.GetComponent<ItemWorld_InventorySys>();
            itemWorld.SetItem(item);

            return itemWorld;
        }

        public static ItemWorld_InventorySys DropItem(Vector3 dropPosition,Item_InventorySys item)
        {
            Vector3 randomDir = GetRandomDir();
            ItemWorld_InventorySys itemWorld = SpawnItemWorld(dropPosition + randomDir * 5f, item);
            itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 5f,ForceMode2D.Impulse);
            return itemWorld;
        }

        private Item_InventorySys _item;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetItem(Item_InventorySys item)
        {
            this._item = item;
            _spriteRenderer.sprite = item.GetSprite();
            var itemAmount = item.amount > 1 ? item.amount.ToString() : "";
            ItemWorldAmountText.SetText(itemAmount);
        }

        public Item_InventorySys GetItem()
        {
            return _item;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        private static Vector3 GetRandomDir()
        {
            var x = Random.Range(-2f,2f);
            var y = Random.Range(-2f,2f);
            if (x == 0 & y == 0) x = 2;
            return new Vector3(x, y, 0);
        }
    }
}