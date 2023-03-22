using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Simple_Inventory_System.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=2WnAOV7nHW0&ab_channel=CodeMonkey
    /// </summary>
    public class ItemSlotTemplate_InventorySys : MonoBehaviour, IPointerClickHandler
    {
        public delegate void MouseAction();
        
        public Image ItemImage;
        public TextMeshProUGUI AmountText;

        public MouseAction MouseLeftClickFunc;
        public MouseAction MouseRightClickFunc;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            // Left Click
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                MouseLeftClickFunc?.Invoke();
            }
            // Right Click
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                MouseRightClickFunc?.Invoke();
            }
        }
    }
}