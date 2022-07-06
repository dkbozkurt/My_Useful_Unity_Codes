// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Line.Draw_Line_To_Clicked_Point_And_Generate_Shapes
{
    /// <summary>
    /// Attach this script onto "dotPrefab"
    ///
    /// Ref : https://www.youtube.com/watch?v=pcLn2ze9JQA
    /// </summary>
    
    public class DrawLineToClickPointAndGenerateShapes_DotController : MonoBehaviour, IDragHandler, IPointerClickHandler
    {
        public DrawLineToClickPointAndGenerateShapes_LineController lineController;
        
        public Action<DrawLineToClickPointAndGenerateShapes_DotController> OnDragEvent;
        public Action<DrawLineToClickPointAndGenerateShapes_DotController> OnRightClickEvent;
        public Action<DrawLineToClickPointAndGenerateShapes_DotController> OnLeftClickEvent;

        public int index;
        
        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Right Click
            if (eventData.pointerId == -2)
            {
                OnRightClickEvent?.Invoke(this);
            }
            
            // Left Click
            if (eventData.pointerId == -1)
            {
                OnLeftClickEvent?.Invoke(this);
            }
            
        }

        public void SetLine(DrawLineToClickPointAndGenerateShapes_LineController line)
        {
            this.lineController = line;
        }
    }
}