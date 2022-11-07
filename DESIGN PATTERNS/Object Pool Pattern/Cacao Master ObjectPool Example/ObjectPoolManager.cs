using System;
using System.Collections.Generic;
using CpiTemplate.Game.Playable.Scripts.Helpers;
using UnityEngine;

namespace CpiTemplate.Game.Playable.Scripts.Managers
{
    public enum ObjectName
    {
        None,
        Cacao,
        Chocolate,
        Money
    }

    [Serializable]
    public struct ObjectToSpawn
    {
        public GameObject objectPrefab;
        public ObjectName objectName;
        [HideInInspector] public List<GameObject> pooledObjects;
    }
    
    public class ObjectPoolManager : SingletonBehaviour<ObjectPoolManager>
    {
        [SerializeField] private List<ObjectToSpawn> objectsToSpawnList;
        [SerializeField] private int amountToPool = 10;

        private Dictionary<ObjectName, ObjectToSpawn> _objectDict = new Dictionary<ObjectName, ObjectToSpawn>();
        
        protected override void OnAwake() { }

        private void Start()
        {
            GeneratePool();
        }

        public GameObject GetReadyPooledObjectAtTransform(ObjectName objectName,Vector3 position)
        {
            if (objectName == ObjectName.None) return null;
            
            // var objectToSpawn = GetDesiredObjectStruct(objectName);
            var objectToSpawn = _objectDict[objectName];
            GameObject obj = GetPooledObject(objectToSpawn);

            if (obj != null)
            {
                obj.transform.position = position;
                obj.SetActive(true);
                return obj;
            }

            return null;
        }
        
        public GameObject GetPooledObject(ObjectToSpawn objectToSpawn)
        {
            if (objectToSpawn.objectName == ObjectName.None) return null;
            
            for (int i = 0; i < objectToSpawn.pooledObjects.Count; i++)
            {
                if (!objectToSpawn.pooledObjects[i].activeInHierarchy)
                {
                    return objectToSpawn.pooledObjects[i];
                }
            }

            return GenerateNewObject(objectToSpawn);
        }

        public void DeactivatePooledObject(ObjectName objectName, GameObject pooledObject)
        {
            if(!_objectDict[objectName].pooledObjects.Contains(pooledObject))
            {
                Debug.Log("You are trying to disable an object that is not in the pooled list.");
                return; 
            }

            pooledObject.SetActive(false);
        }

        public void AddObjectBackToPool(GameObject obj)
        {
            obj.transform.parent = transform;
            obj.SetActive(false);
        }
        
        private void GeneratePool()
        {
            foreach (var child in objectsToSpawnList)
            {
                for (int i = 0; i < amountToPool; i++)
                {
                    GenerateObject(child);
                }    
            }
            
            GenerateObjectDict();
        }

        private void GenerateObjectDict()
        {
            for (int i = 0; i < objectsToSpawnList.Count; i++)
            {
                _objectDict.Add(objectsToSpawnList[i].objectName,objectsToSpawnList[i]);
            }
        }
        
        private void GenerateObject(ObjectToSpawn objectToSpawn)
        {
            GameObject obj = Instantiate(objectToSpawn.objectPrefab, transform);
            obj.SetActive(false);
            objectToSpawn.pooledObjects.Add(obj);
        }

        private GameObject GenerateNewObject(ObjectToSpawn objectToSpawn)
        {
            GameObject obj = Instantiate(objectToSpawn.objectPrefab, transform);
            obj.SetActive(false);
            objectToSpawn.pooledObjects.Add(obj);
            return obj;
        }

        private ObjectToSpawn GetDesiredObjectStruct(ObjectName objectName)
        {
            foreach (var child in objectsToSpawnList)
            {
                if (child.objectName == objectName) return child;
            }

            return new ObjectToSpawn();
        }
        
    }
}
