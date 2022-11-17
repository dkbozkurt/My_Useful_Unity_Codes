using System;
using Pathfinding;
using UnityEngine;

namespace AI.Astar_Package.AdditionalScripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=PUJSvd53v4k&ab_channel=CGCookie-UnityTraining
    /// </summary>
    public class CarryTargetToMousePosition : MonoBehaviour
    {
        [SerializeField] private AIDestinationSetter _aiDestinationSetter;
        
        private Plane _plane = new Plane(Vector3.up, 0);

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                transform.position = GetMouseWorldPosition();
                _aiDestinationSetter.AITargetPositionSet(transform.position);
            }
        }
        
        private Vector3 GetMouseWorldPosition()
        {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out distance))
            {
                return ray.GetPoint(distance);
            }

            return Vector3.zero;
        }
        
    }
}
