// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple_Drag_Drop_2D.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=BGr-7GZJNXg&list=RDCMUCFK6NCbuCIVzA6Yj1G_ZqCg&index=2
    /// </summary>

    public class DroppableItemSlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if(eventData.pointerDrag == null) return;

            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
