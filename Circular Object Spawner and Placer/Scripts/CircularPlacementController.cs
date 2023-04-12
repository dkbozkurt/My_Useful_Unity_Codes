// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Circular_Object_Spawner_and_Placer.Scripts
{
    /// <summary>
    /// Ref : https://answers.unity.com/questions/1068513/place-8-objects-around-a-target-gameobject.html
    /// </summary>
    public class CircularPlacementController : MonoBehaviour
    {
        [Header("Test Properties")]
        [SerializeField] private GameObject _objectToSpawnPrefab;
        [SerializeField] private int _objectToSpawnCount = 6;
        [SerializeField] private float _circleRadius = 2f;
        [SerializeField] private float _drawDegree = 360f;
        
        private List<GameObject> _spawnedObjects = new List<GameObject>();

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Test();
            }
        }
        
        private void Test()
        {
            CircularSpawnNOrder(_objectToSpawnPrefab,_objectToSpawnCount,_drawDegree,_circleRadius);
        }

        public void CircularSpawnNOrder<T>(T objectClass , int objectAmount, float drawDegree = 360f, float circleRadius = 2f) where T : MonoBehaviour
        {
            CircularSpawnNOrder(objectClass.gameObject, objectAmount, circleRadius, drawDegree);
        }
        
        public void CircularSpawnNOrder(Transform objectPrefabTransform,int objectAmount, float drawDegree = 360f,float circleRadius = 2f)
        {
            CircularSpawnNOrder(objectPrefabTransform.gameObject, objectAmount, circleRadius, drawDegree);
        }

        public void CircularSpawnNOrder(GameObject objectPrefab,int objectAmount, float drawDegree = 360f,float circleRadius = 2f)
        {
            RemoveSpawnedObjects();
            
            for (int i = 0; i < objectAmount; i++)
            {
                float angle = (i * Mathf.PI * (drawDegree/180f)) / objectAmount;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * circleRadius, 0, Mathf.Sin(angle)*circleRadius);
                GameObject go = Instantiate(objectPrefab, newPos, Quaternion.identity);
                _spawnedObjects.Add(go);
            }
        }
        
        public void CircularOrder<T>(T[] objectsToOrder, float drawDegree = 360f,
            float circleRadius = 2f) where T : MonoBehaviour
        {
            Transform[] objectsArray = objectsToOrder.Select(obj => obj.transform).ToArray();
            
            CircularOrder(objectsArray,drawDegree,circleRadius);
        }
        
        public void CircularOrder<T>(List<T> objectsToOrder, float drawDegree = 360f,
            float circleRadius = 2f) where T : MonoBehaviour
        {
            Transform[] objectsArray = objectsToOrder.Select(obj => obj.transform).ToArray();
            
            CircularOrder(objectsArray,drawDegree,circleRadius);
        }

        public void CircularOrder(List<GameObject> objectsToOrder, float drawDegree = 360f,
            float circleRadius = 2f)
        {
            Transform[] objectsArray = objectsToOrder.Select(obj => obj.transform).ToArray();
            
            CircularOrder(objectsArray,drawDegree,circleRadius);
        }
        
        public void CircularOrder(GameObject[] objectsToOrder, float drawDegree = 360f,
            float circleRadius = 2f)
        {
            Transform[] objectsArray = objectsToOrder.Select(obj => obj.transform).ToArray();
            
            CircularOrder(objectsArray,drawDegree,circleRadius);
        }

        public void CircularOrder(List<Transform> objectsToOrder, float drawDegree = 360f,float circleRadius = 2f)
        {
            CircularOrder(objectsToOrder.ToArray(),drawDegree,circleRadius);
        }
        
        public void CircularOrder(Transform[] objectsToOrder, float drawDegree = 360f, float circleRadius = 2f)
        {
            for (int i = 0; i < objectsToOrder.Length; i++)
            {
                float angle = (i * Mathf.PI * (drawDegree/180f)) / objectsToOrder.Length;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * circleRadius, 1, Mathf.Sin(angle)*circleRadius);
                objectsToOrder[i].position = newPos;
            }
        }
        
        private void RemoveSpawnedObjects()
        {
            foreach (GameObject child in _spawnedObjects)
            {
                Destroy(child.gameObject);
            }
            
            _spawnedObjects.Clear();
        }
    }
}
