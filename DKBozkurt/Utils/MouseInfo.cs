// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace DKBozkurt.Utils
{
    /// <summary>
    /// My own Utils class for getting Mouse Info.
    /// </summary>
    public static partial class DKBozkurtUtils
    {
        private static readonly int _maxRayDistance = 1000;
        /// <summary>
        /// Used for ignoring World clicks through UI.
        /// </summary>
        /// <returns>Is Mouse over a UI Element?</returns>
        public static bool IsPointerOverUI()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return true;
            else
            {
                PointerEventData _eventDataCurrentPosition = new PointerEventData(EventSystem.current)
                    {position = Input.mousePosition};
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(_eventDataCurrentPosition, hits);
                return hits.Count > 0;
            }
        }

        /// <summary>
        /// Get Mouse screen position reference to left bottom corner as 0,0
        /// </summary>
        /// <returns>Mouse screen position </returns>
        public static Vector3 GetMouseScreenPosition()
        {
            return Input.mousePosition;
        }

        /// <summary>
        /// It takes mouses's pixel position and converts it into a real world position.
        /// Method will return x,y plane value seen by the camera in the 3D world with editable distanceFromCamera value on the z axis.
        /// </summary>
        /// <param name="distanceFromCamera">The z value that will be added to worldPointsDistance from the camera. </param>
        /// <returns>Screen to world point position with a selected distance value from the camera.</returns>
        public static Vector3 GetScreenToWorldPointWithDistance(float distanceFromCamera=0f)
        {
            var screenPosition = GetMouseScreenPosition();
            screenPosition.z = Camera.nearClipPlane + distanceFromCamera;

            var worldPosition = Camera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        /// <summary>
        /// It takes mouses's pixel position on the screen and sends a ray from this point into the
        /// to corresponding position in the real world position.
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetScreenPointToRay()
        {
            var screenPosition = GetMouseScreenPosition();

            Ray ray = Camera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.point;
            }

            return Vector3.zero;
        }
        
        /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Get Mouse Position in World with Z = 0f for Orthographic.
        /// Call in update function.
        /// </summary>
        /// <returns> Mouse position. </returns>
        public static Vector3 GetMouseWorldPositionOrthographic()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera);
            vec.z = 0f;
            return vec;
        }
        
        /// <summary>
        /// Get Mouse Position in World with Z value respect to Camera
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <param name="worldCamera"></param>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        /// <summary>
        /// Get Mouse Position in World.
        /// </summary>
        /// <returns> Mouse position. </returns>
        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera);
        }

        /// <summary>
        /// Get Mouse Position in World with respect to camera.
        /// </summary>
        /// <returns> Mouse position. </returns>
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        /// <summary>
        /// Get Mouse Position in World with nearclipplane value distance diff for orthographic camera.
        /// Call in update function.
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionRespectToNearClipPlane()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.nearClipPlane;
            return Camera.ScreenToWorldPoint(mousePosition);
        }

        /// <summary>
        /// Favourite for Perspective Camera.
        /// Call in update function.
        /// Get mouse position in world respect to camera with Z distance value. 
        /// </summary>
        /// <param name="distanceFromCamera">Distance from camera to mouse position.</param>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionWithDistance(float distanceFromCamera)
        {
            return Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                distanceFromCamera));
        }

        /// <summary>
        /// Get mouse position in world respect to camera with Z distance value and offset value.
        /// Call in update function. 
        /// </summary>
        /// <param name="distanceFromCamera">Distance from camera to mouse position.</param>
        /// <param name="xOffset">x off set value</param>
        /// <param name="yOffset">y off set value</param>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPositionWithDistanceAndOffset(float distanceFromCamera, float xOffset = 0f,
            float yOffset = 0f)
        {
            return Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                distanceFromCamera));
        }

        private static Plane _plane = new Plane(Vector3.up, 0);

        /// <summary>
        /// Get mouse position on specified plane.
        /// Call in update function.
        /// </summary>
        /// <returns>Mouse position on Z Plane</returns>
        public static Vector3 GetMousePositionByCreatingPlaneOnZAxis()
        {
            float distance;

            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out distance)) return ray.GetPoint(distance);
            return Vector3.zero;
        }

        /// <summary>
        /// Get Mouse Position on an object with a collider.
        /// Call in update function.
        /// </summary>
        /// <returns>Mouse position in 3D space.</returns>
        public static Vector3 GetMousePositionOnAllObjectsWithCollider()
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _maxRayDistance)) return hit.point;
            return Vector3.zero;
        }

        /// <summary>
        /// Get Mouse Position on an object with collider and a specified tag.
        /// Call in update function.
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static Vector3 GetMousePositionOnSpecifiedTag(string tagName)
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _maxRayDistance) && hit.transform.CompareTag(tagName)) { return hit.point; }
            return Vector3.zero;
        }

        /// <summary>
        /// Get Mouse Position on an object with specified layer.
        /// Call in update function.
        /// </summary>
        /// <param name="layerMask">Specified layer.</param>
        /// <returns></returns>
        public static Vector3 GetMousePositionOnSpecifiedLayer(LayerMask layerMask)
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _maxRayDistance,layerMask)) { return hit.point; }
            return Vector3.zero;
        }

        /// <summary>
        /// Get Mouse Position on an object except specified layer.
        /// Call in update function.
        /// </summary>
        /// <param name="layerMask">Specified layer to ignore.</param>
        /// <returns></returns>
        public static Vector3 GetMousePositionExpectSpecifiedLayer(LayerMask layerMask)
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _maxRayDistance,~layerMask)) { return hit.point; }
            return Vector3.zero;
        }

        /// <summary>
        /// Select object by mouse for 2D objects.
        /// </summary>
        /// <returns></returns>
        public static GameObject Select2DObjectOnClick()
        {
            Vector3 worldPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero, 0);
            if (hit && Input.GetMouseButtonDown(0)) return hit.transform.gameObject;
            return null;
        }
        
        /// <summary>
        /// Select object by mouse with layer mask for 2D objects.
        /// </summary>
        /// <param name="layerMask">Specified layer.</param>
        /// <returns></returns>
        public static GameObject Select2DObjectOnClick(LayerMask layerMask)
        {
            Vector3 worldPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero, 0,layerMask);
            if (hit && Input.GetMouseButtonDown(0)) return hit.transform.gameObject;
            return null;
        }

        /// <summary>
        /// Select object by mouse for 3D objects.
        /// </summary>
        /// <returns></returns>
        public static GameObject Select3DObjectOnClick()
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _maxRayDistance) && Input.GetMouseButtonDown(0)) return hit.transform.gameObject;
            return null;
        }
        
        /// <summary>
        /// Select object by mouse with layer mask for 3D objects.
        /// </summary>
        /// <param name="layerMask">Specified layer.</param>
        /// <returns></returns>
        public static GameObject Select3DObjectOnClick(LayerMask layerMask)
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _maxRayDistance,layerMask) && Input.GetMouseButtonDown(0)) return hit.transform.gameObject;
            return null;
        }
        
    }
}