// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Line.Draw_Line_To_Clicked_Point_And_Generate_Shapes
{
    /// <summary>
    /// Mouse input detection
    ///
    /// Ref : https://www.youtube.com/watch?v=pcLn2ze9JQA
    /// </summary>
    
    public class DrawLineToClickPointAndGenerateShapes_PenCanvas : MonoBehaviour, IPointerClickHandler
    {
        public Action OnPenCanvasLeftClickEvent;
        public Action OnPenCanvasRightClickEvent;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            // Left click
            if (eventData.pointerId == -1)
            {
                OnPenCanvasLeftClickEvent?.Invoke();
            }
            
            // Right Click
            if (eventData.pointerId == -2)
            {
                OnPenCanvasRightClickEvent?.Invoke();
            }
                
        }
    }
}