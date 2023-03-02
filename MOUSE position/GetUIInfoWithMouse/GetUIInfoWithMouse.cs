// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MousePosition___MouseClick.GetUIInfoWithMouse
{
    /// <summary>
    /// Ref : https://stackoverflow.com/questions/69986439/interact-with-ui-via-raycast-unity
    /// </summary>
    public class GetUIInfoWithMouse : MonoBehaviour
    {
        // UI layer index is 5 so _layerMask value is 5.
        private LayerMask _layerMask = 5;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SendRayToUINGetInformation();
            }
        }

        private void SendRayToUINGetInformation()
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            
            if(results.Where(r => r.gameObject.layer == _layerMask).Count() > 0)
            {
                Debug.Log("Touched UI name: " + results[0].gameObject.name);
            }
        }
    }
}
