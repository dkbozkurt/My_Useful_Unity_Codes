// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;
// Important
using UnityEngine.EventSystems;

namespace DraggableUIWindow
{
    /// <summary>
    ///
    /// Attach this script into the title (child object) of the window.
    ///  
    /// Sending selected object at the bottom of canvas so we can render the canvas top of other canvases.
    /// 
    /// Ref : https://www.youtube.com/watch?v=Mb2oua3FjZg&ab_channel=CodeMonkey
    /// </summary>
    public class DragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        // Parent window transform.
        [SerializeField] private RectTransform _dragRectTransform;
        
        // Game canvas
        [SerializeField] private Canvas _canvas;

        private void Awake()
        {
            if (_dragRectTransform == null)
            {
                _dragRectTransform = transform.parent.GetComponent<RectTransform>();
            }

            if (_canvas == null)
            {
                Transform testCanvasTransform = transform.parent;
                while (testCanvasTransform != null)
                {
                    _canvas = testCanvasTransform.GetComponent<Canvas>();
                    if(_canvas != null) break;
                    testCanvasTransform = testCanvasTransform.parent;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            _dragRectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        // Sending selected object at the bottom of canvas so we can render the canvas top of other canvases.
        public void OnPointerDown(PointerEventData eventData)
        {
            _dragRectTransform.SetAsLastSibling();
            
        }
    }
}
