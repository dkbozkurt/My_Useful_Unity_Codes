using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MousePosition___MouseClick.Mouse_Click_Over_UI_and_Game
{
    public class IsPointerOverUIOrNot : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(IsPointerOverUIObject()) return; // Clicked on UI
                
                Debug.Log("Clicked On Game");
            }
        
        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition,results);
            return results.Count > 0;
        }
    }
}
