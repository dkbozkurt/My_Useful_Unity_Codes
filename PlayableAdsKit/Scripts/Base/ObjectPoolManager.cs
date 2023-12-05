using System;
using System.Collections.Generic;
using PlayableAdsKit.Scripts.Helpers;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Base
{
    public enum ObjectName
    {
        Type1,
        Type2,
        Type3
    }

    [Serializable]
    public class ObjectPoolItem
    {
        [SerializeField] private ObjectName _objectName;
        [SerializeField] private GameObject _objectToPool;
        [SerializeField] private int _amountToPool;

        public ObjectName ObjectName => _objectName;
        public GameObject ObjectToPool => _objectToPool;
        public int AmountToPool => _amountToPool;
        public List<GameObject> PooledObjects { get; set; } = new List<GameObject>();
    }
    
    public class ObjectPoolManager : MonoBehaviour
    {
        [SerializeField] private ObjectPoolItem[] _objectPoolItems;
        
        public static ObjectPoolManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            CreatePool();
        }

        private void CreatePool()
        {
            foreach (ObjectPoolItem item in _objectPoolItems)
            {
                for (int i = 0; i < item.AmountToPool; i++)
                {
                    GameObject objectToPool = Instantiate(item.ObjectToPool, transform);
                    objectToPool.SetActive(false);
                    item.PooledObjects.Add(objectToPool);
                }
            }
        }
        
        public GameObject GetPooledObject(ObjectName objectName) =>
            GetPooledObject(objectName, Vector3.zero, Quaternion.identity);

        public GameObject GetPooledObject(ObjectName objectName, Transform parent) =>
            GetPooledObject(objectName, parent.position, parent.rotation, parent);

        public GameObject GetPooledObject(ObjectName objectName, Vector3 objectPosition, Quaternion objectRotation, Transform parent = null, bool isLocalPos = false)
        {
            GameObject pooledObject = CallPooledObject(objectName);
            
            if (parent)
                pooledObject.transform.SetParent(parent);
            if (isLocalPos)
                pooledObject.transform.localPosition = objectPosition;
            else
                pooledObject.transform.position = objectPosition;
            
            pooledObject.transform.rotation = objectRotation;
            pooledObject.SetActive(true);

            return pooledObject;
        }
        
        public void DeactivateAllPooledObjects()
        {
            foreach (ObjectPoolItem objectPoolItem in _objectPoolItems)
                foreach (GameObject pooledObject in objectPoolItem.PooledObjects)
                {
                    pooledObject.SetActive(false);
                    pooledObject.transform.SetParent(transform);
                }
        }
        
        private GameObject CallPooledObject(ObjectName objectName)
        {
            foreach (ObjectPoolItem item in _objectPoolItems)
            {
                if (item.ObjectName == objectName)
                {
                    foreach (GameObject pooledObject in item.PooledObjects)
                    {
                        if (!pooledObject.activeSelf)
                            return pooledObject;
                    }

                    return GenerateNewObject(item);
                }
            }
            return null;
        }
        
        private GameObject GenerateNewObject(ObjectPoolItem item)
        {
            GameObject instantiatedItem = Instantiate(item.ObjectToPool, transform);
            instantiatedItem.SetActive(false);
            item.PooledObjects.Add(instantiatedItem);
            
            return instantiatedItem;
        }
        
    }
}