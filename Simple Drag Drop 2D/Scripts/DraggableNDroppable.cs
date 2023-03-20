// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple_Drag_Drop_2D.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=BGr-7GZJNXg&list=RDCMUCFK6NCbuCIVzA6Yj1G_ZqCg&index=2
    /// </summary>

    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableNDroppable : MonoBehaviour, IPointerDownHandler,IBeginDragHandler,IEndDragHandler,IDragHandler
    {
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = .6f;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }
    }
}
