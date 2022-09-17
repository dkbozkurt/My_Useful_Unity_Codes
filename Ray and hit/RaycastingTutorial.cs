// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;

namespace RayCast
{
    /// <summary>
    /// Raycasting
    /// 
    /// Ref : https://www.youtube.com/watch?v=EINgIoTG8D4
    /// </summary>

    public class RaycastingTutorial : MonoBehaviour
    {
        private void Start()
        {
            // Can be used as
            // Physics.Raycast(Vector3 origin, Vector3 direction, RaycastHit hitInfo, float distance, int LayerMask);
            
            // Also can be used as
            // Ray myRay = new Ray(Vector3 origin, Vector3 direction);
            // Physics.Raycast(myRay, RaycastHit hitInfo, float distance, int LayerMask);
            
            // In general:
            // RaycastHit hit;
            // Ray myRay = new Ray(transform.position, Vector3.down);
            // if(Physics.Raycast(myRay, out hitInfo, groundDistance));
        }
    }
}
