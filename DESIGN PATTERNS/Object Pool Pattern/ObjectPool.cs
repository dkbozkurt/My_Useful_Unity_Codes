// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Pool
/// 
/// Instead of creating and destroying objects several times, we are creating only once and then using it whenever we need.
/// By that way, we are gaining performance.
///
/// Since the limit of the number of objects in the pool has been reached, we can call the first called object again.
///
/// Attach this script onto the parent object of the pooled objects.
/// 
/// Ref: https://www.youtube.com/watch?v=QgP_J9XHz1Y&ab_channel=Emirhan%C5%9Eenkal
/// </summary>

public class ObjectPool : MonoBehaviour
{
    [Serializable]
    public struct Pool
    {
        public Queue<GameObject> pooledObjects;
        public GameObject objectPrefab;
        public int poolSize;
    }

    [SerializeField] private Pool[] pools = null;
    
    private void Awake()
    {
        for (int j = 0; j < pools.Length; j++)
        {
            pools[j].pooledObjects = new Queue<GameObject>();
            
            for (int i = 0; i < pools[j].poolSize; i++)
            {
                GameObject obj = Instantiate(pools[j].objectPrefab);
                obj.SetActive(false);
                obj.transform.parent = gameObject.transform;
            
                pools[j].pooledObjects.Enqueue(obj);
            }
        }
    }

    public GameObject GetPooledObject(int objectType)
    {
        if (objectType >= pools.Length) return null;
        
        GameObject obj = pools[objectType].pooledObjects.Dequeue();
        obj.SetActive(true);
        pools[objectType].pooledObjects.Enqueue(obj);

        return obj;
    }
}
