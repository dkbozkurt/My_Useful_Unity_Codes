using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Simple_Inventory_System.Scripts
{
    /// <summary>
    /// Ref : 
    /// </summary>
    public class ItemWorldSpawner_InventorySys : MonoBehaviour
    {
        [SerializeField] public List<Item_InventorySys> _itemsToSpawn;
        
        private void Start()
        {
            SpawnItems();    
        }

        private void SpawnItems()
        {
            foreach (Item_InventorySys item in _itemsToSpawn)
            {
                ItemWorld_InventorySys.SpawnItemWorld(GetRandomPosition(-20,20), item);    
            }
        }
        
        private Vector3 GetRandomPosition(float from,float to)
        {
            var x = GetRandomFloat(from, to);   
            var y = GetRandomFloat(from, to);
            return new Vector3(x, y);
        }

        private float GetRandomFloat(float from,float to)
        {
            return Random.Range(from, to);
        }
        
    }
}