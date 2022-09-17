// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace RayCast
{
    /// <summary>
    /// Fav Ray
    /// 
    /// Ref : https://www.youtube.com/watch?v=vqc9f7HU-Vc && https://www.youtube.com/watch?v=cMp3kTyDmpw
    /// </summary>

    public enum RayDetectionType
    {
        Single,
        Multiple
    }
    public class DetectingWithRay : MonoBehaviour
    {
        [Header("Ray properties")] 
        [SerializeField] private Transform rayOrigin;
        [SerializeField] private Vector3 rayDirection= Vector3.forward;
        [SerializeField] private float maxRayDistance = Single.MaxValue;
        [SerializeField] private LayerMask detectionLayerOfRay = LayerMask.NameToLayer("Default");
        
        public Vector3 collision = Vector3.zero;


        [Header("Detection Type")]
        [SerializeField] private RayDetectionType rayDetectionType = RayDetectionType.Single;
        private void Update()
        {
            if (rayDetectionType == RayDetectionType.Single)
            {
                SendRayAndDetect(rayOrigin, rayDirection, maxRayDistance, detectionLayerOfRay);    
            }
            else if (rayDetectionType == RayDetectionType.Multiple)
            {
                SendRayAndDetectMultiple(rayOrigin, rayDirection, maxRayDistance, detectionLayerOfRay);
            }
            
        }

        private void SendRayAndDetect(Transform origin, Vector3 direction,float maxDistance, LayerMask detectionLayer)
        {
            Ray ray = new Ray(origin.position,direction);
            // Avoiding null error by defining out hit info before checking it.
            RaycastHit hitInfo;
            
            Debug.DrawRay(origin.position, direction * maxDistance, Color.green);

            if (Physics.Raycast(ray,out hitInfo, maxRayDistance, detectionLayer))
            {
                Debug.Log("Detected object is : " + hitInfo.transform.name);
                collision = hitInfo.point;
            }

        }
        
        // It will go all through and It won't return when it is touched with single object. 
        private void SendRayAndDetectMultiple(Transform origin, Vector3 direction,float maxDistance, LayerMask detectionLayer)
        {
            Ray ray = new Ray(origin.position,direction);
            RaycastHit hitInfo;

            Debug.DrawRay(origin.position, direction * maxDistance, Color.green);

            var multipleHits = Physics.RaycastAll(ray,maxRayDistance,detectionLayer);
            foreach(var raycastHit in multipleHits)
            {
                Debug.Log("Detected objects are : " + raycastHit.transform.name);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(collision,0.2f);
        }
    }
}
