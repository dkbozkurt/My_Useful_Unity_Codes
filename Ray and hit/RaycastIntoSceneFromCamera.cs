// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace RayCast
{
    /// <summary>
    /// 
    /// Ref : https://www.youtube.com/watch?v=cMp3kTyDmpw 
    /// </summary>

    public class RaycastIntoSceneFromCamera : MonoBehaviour
    {
        
        [SerializeField] private Camera camera;
        
        [SerializeField] private float maxRayDistance = Single.MaxValue;
        [SerializeField] private LayerMask detectionLayerOfRay = LayerMask.NameToLayer("Default");
        private void OnValidate()
        {
            if (!camera) { camera = Camera.main; }
        }

        private void Update()
        {
            SendRaysFromCamera(maxRayDistance,detectionLayerOfRay);
           
        }

        private void SendRaysFromCamera(float maxDistance, LayerMask detectionLayer)
        {
            Vector2 mouseScreenPosition = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mouseScreenPosition);
            RaycastHit hit;
            
            Debug.DrawRay(ray.origin,ray.direction * 10f,Color.green);
            // Debug.DrawRay(camera.transform.position,ray.direction, Color.green);

            if (Physics.Raycast(ray, out hit, maxDistance, detectionLayer))
            {
                Debug.Log("Detected object is : " + hit.transform.name);
            }
        }
    }
}
