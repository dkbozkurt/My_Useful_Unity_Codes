using System;
using UnityEngine;

namespace Simple_Inventory_System.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=2WnAOV7nHW0&ab_channel=CodeMonkey
    /// </summary>
    public class ItemAssets_InventorySys : MonoBehaviour
    {
        #region Singleton

        public static ItemAssets_InventorySys Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        [Header("Asset Sprite Info")] 
        public Sprite SwordSprite;
        public Sprite HealthPotionSprite;
        public Sprite ManaPotionSprite;
        public Sprite CoinSprite;
        public Sprite MedKitSprite;

        public Transform ItemWorld;
        
        private void Start()
        {
        
        }

        private void Update()
        {
        
        }
    
    }
}