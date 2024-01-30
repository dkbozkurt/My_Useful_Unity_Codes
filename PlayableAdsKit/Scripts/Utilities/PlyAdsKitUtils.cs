using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace PlayableAdsKit.Scripts.Utilities
{
    public static class PlyAdsKitUtils
    {
        private static Camera _camera;
        private static readonly int _maxRayDistance = 1000;
        
        #region Camera

        /// <summary>
        /// Getting main camera.
        /// </summary>
        public static Camera Camera
        {
            get
            {
                if(_camera == null) _camera = UnityEngine.Camera.main;
                return _camera;
            }    
        }

        #endregion

        #region Screen

        public static bool IsLandscape()
        {
            return Screen.width > Screen.height; 
        }

        public static float GetScreenWidth()
        {
            return Screen.width;
        }
        
        public static float GetScreenHeight()
        {
            return Screen.height;
        }

        #endregion

        #region MouseInfo

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
            if (Physics.Raycast(ray, out hit, _maxRayDistance)) return hit.transform.gameObject;
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
            if (Physics.Raycast(ray, out hit, _maxRayDistance,layerMask)) return hit.transform.gameObject;
            return null;
        }

        /// <summary>
        /// Detect UI elements by sending ray from the camera to game and return first detected ui element gameObject.
        /// </summary>
        /// <param name="layerMaskIndex">Layer mask index for UI</param>
        /// <returns>First Detected UI Element</returns>
        public static GameObject GetUIInfo(int layerMaskIndex = 5)
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Where(r => r.gameObject.layer == layerMaskIndex).Count() <= 0) return null;
            
            return results[0].gameObject;
        }
        #endregion

        #region Random

        /// <summary>
        /// Getting random int number
        /// </summary>
        /// <param name="min">Min value included</param>
        /// <param name="max">Max value excluded</param>
        public static int GetRandomInt(int min,int max)
        {
            return Random.Range(min, max);
        }
        
        /// <summary>
        /// Getting random float number
        /// </summary>
        /// <param name="min">Min value included</param>
        /// <param name="max">Max value excluded</param>
        /// <returns></returns>
        public static float GetRandomFloat(float min, float max)
        {
            return Random.Range(min, max);
        }
        
        /// <summary>
        /// Getting random element of an array
        /// </summary>
        /// <param name="array">Array to get random element.</param>
        /// <typeparam name="T">Array type.</typeparam>
        /// <returns>Random Elements belongs to input array. </returns>
        public static T GetRandomFromArray<T>(T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
        
        /// <summary>
        /// Get Random Color
        /// </summary>
        /// <returns> Random color.</returns>
        public static Color GetRandomColor() {
            return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
        }
        
        /// <summary>
        /// Returns an array with unique numbers between the given range and array size.
        /// </summary>
        /// <param name="returnArraySize">Return random numbers array length.</param>
        /// <param name="randomIndexRange">Assignable numbers range.</param>
        /// <param name="randomNumStartIndex">Assignable numbers starting value.</param>
        /// <returns></returns>
        public static int[] GetUniqueRandomIndexes(int returnArraySize,int randomIndexRange, int randomNumStartIndex =0)
        {
            if (randomIndexRange < returnArraySize)
            {
                Debug.LogError(" Random Indexn Range must be greater than Return Array Size!");
                return null;
            }
            HashSet<int> uniqueNumbers = new HashSet<int>();

            while (uniqueNumbers.Count < returnArraySize)
            {
                int randomNumber = UnityEngine.Random.Range(randomNumStartIndex, randomIndexRange);
                uniqueNumbers.Add(randomNumber);
            }

            int[] randomNumbersArray = new int[returnArraySize];
            uniqueNumbers.CopyTo(randomNumbersArray);

            return randomNumbersArray;
        }
        
        /// <summary>
        /// To get a random position between two circles difference on XY plane.
        /// </summary>
        /// <param name="smallerRadius"> Radius of smaller circle.</param>
        /// <param name="greaterRadius"> Radius of greater circle.</param>
        /// <returns></returns>
        public static Vector3 GetRandomSpawnPositionFromTwoCirclesDifferenceOnXY
            (float smallerRadius, float greaterRadius)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(smallerRadius, greaterRadius);

            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            float z = 0f;

            return new Vector3(x, y, z);
        }
        
        /// <summary>
        /// To get a random position between two circles difference on XZ plane.
        /// </summary>
        /// <param name="smallerRadius"> Radius of smaller circle.</param>
        /// <param name="greaterRadius"> Radius of greater circle.</param>
        /// <returns></returns>
        public static Vector3 GetRandomSpawnPositionFromTwoCirclesDifferenceOnXZ
            (float smallerRadius, float greaterRadius)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(smallerRadius, greaterRadius);

            float x = radius * Mathf.Cos(angle);
            float y = 0f;
            float z = radius * Mathf.Sin(angle);

            return new Vector3(x, y, z);
        }
        
        /// <summary>
        /// Returns a random point position on the sphere of the given radius.
        /// </summary>
        /// <param name="sphereRadius">Sphere radius</param>
        /// <returns></returns>
        public static Vector3 GetRandomPointOnSphere(float sphereRadius)
        {
            return Random.onUnitSphere * sphereRadius;
        }

        #endregion

        #region Array & List

        /// <summary>
        /// Shuffle array elements
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="iterations"></param>
        /// <typeparam name="T"></typeparam>
        public static void ShuffleArray<T>(T[] arr, int iterations) {
            for (int i = 0; i < iterations; i++) {
                int rnd = UnityEngine.Random.Range(0, arr.Length);
                T tmp = arr[rnd];
                arr[rnd] = arr[0];
                arr[0] = tmp;
            }
        }
        
        /// <summary>
        /// Shuffle list elements
        /// </summary>
        /// <param name="list"></param>
        /// <param name="iterations"></param>
        /// <typeparam name="T"></typeparam>
        public static void ShuffleList<T>(List<T> list, int iterations) {
            for (int i = 0; i < iterations; i++) {
                int rnd = UnityEngine.Random.Range(0, list.Count);
                T tmp = list[rnd];
                list[rnd] = list[0];
                list[0] = tmp;
            }
        }
        
        /// <summary>
        /// Remove duplicates from array
        /// </summary>
        /// <param name="arr"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] RemoveDuplicates<T>(T[] arr) {
            List<T> list = new List<T>();
            foreach (T t in arr) {
                if (!list.Contains(t)) {
                    list.Add(t);
                }
            }
            return list.ToArray();
        }
        
        /// <summary>
        /// Remove duplicates from list
        /// </summary>
        /// <param name="arr"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> RemoveDuplicates<T>(List<T> arr) {
            List<T> list = new List<T>();
            foreach (T t in arr) {
                if (!list.Contains(t)) {
                    list.Add(t);
                }
            }
            return list;
        }
        
        /// <summary>
        /// Converts transform array`s positions to Vector3 array and returns it.
        /// </summary>
        /// <param name="thisTransformArray">Target transform array to be converted to Vector3[] position array.</param>
        /// <returns>Vector3 position array.</returns>
        public static Vector3[] ConvertToPositionArray(this Transform[] thisTransformArray)
        {
            Vector3[] positionArray = new Vector3[thisTransformArray.Length];

            for (int i = 0; i < thisTransformArray.Length; i++)
            {
                positionArray[i] = thisTransformArray[i].position;
            }

            return positionArray;
        }
        
        #endregion

        #region Coroutines

        /// <summary>
        /// Waits for a while then executes the callback.
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator WaitForTimeCoroutine(float duration,Action callback = null)
        {
            yield return new WaitForSeconds(duration);
            callback?.Invoke();
        }

        /// <summary>
        /// Waits for a frame then executes the callback.
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator WaitForOneFrameCoroutine(Action callback = null)
        {
            yield return null;
            callback?.Invoke();
        }
        
        /// <summary>
        /// Waits for a while in real time then executes the callback.
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator WaitForRealTimeCoroutine(float duration, Action callback = null)
        {
            yield return new WaitForSecondsRealtime(duration);
            callback?.Invoke();
        }
        
        /// <summary>
        /// Coroutine execution waits until the predicate become true
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator WaitUntilTrueCoroutine(Func<bool> predicate,Action callback= null)
        {
            yield return new WaitUntil(predicate);
            callback?.Invoke();
        }
        
        /// <summary>
        /// Coroutine execution waits while the predicate is true
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator WaitWhileTrueCoroutine(Func<bool> predicate,Action callback= null)
        {
            yield return new WaitWhile(predicate);
            callback?.Invoke();
        }

        #endregion

        #region Objects

        /// <summary>
        /// Quickly destroy all child objects.
        /// </summary>
        /// <param name="t">Parent object's transform </param>
        public static void DeleteChildren(this Transform t)
        {
            foreach (Transform child in t) { Object.Destroy(child.gameObject); }
        }
        
        /// <summary>
        /// Resets the transform properties
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="isLocalTransform"> Identifies if operation going to be in local space</param>
        public static void Reset(this Transform transform, bool isLocalTransform = true )
        {
            if (isLocalTransform)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(Vector3.zero);
                transform.localScale = Vector3.one;
                return;    
            }
            
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
        }
        
        /// <summary>
        /// Makes the object's down vector point at the target transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="targetTransform"></param>
        public static void LookTargetWithDownVector(this Transform transform, Transform targetTransform)
        {
            transform.LookTargetWithDownVector(targetTransform.position);
        }
        
        /// <summary>
        /// Makes the object's down vector point at the target position
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="targetPosition"></param>
        public static void LookTargetWithDownVector(this Transform transform, Vector3 targetPosition)
        {
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            transform.rotation = Quaternion.FromToRotation(transform.up, -directionToTarget)
                                 * transform.rotation;

        }

        /// <summary>
        /// It gives the UI point corresponding to the point where a 3D and 2D object is located on the scene.
        /// </summary>
        /// <param name="targetTransform">Object to get corresponding UI point</param>
        /// <returns>UI location</returns>
        public static Vector3 GetScreenPositionOfObject(this Transform targetTransform)
        {
            return GetScreenPositionOfObject(targetTransform.position);
        }

        /// <summary>
        /// It gives the UI point corresponding to the point where a 3D and 2D object is located on the scene.
        /// </summary>
        /// <param name="targetTransform">Object to get corresponding UI point</param>
        /// <returns>UI location</returns>
        public static Vector3 GetScreenPositionOfObject(this Vector3 targetTransformPosition)
        {
            return Camera.WorldToScreenPoint(targetTransformPosition);
        }

        #endregion

    }
}
