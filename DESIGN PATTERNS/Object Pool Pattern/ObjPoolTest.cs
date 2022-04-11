// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Pool Test Script
///
/// Use this script with "ObjectPool.cs" script
/// 
/// Ref: https://www.youtube.com/watch?v=QgP_J9XHz1Y&ab_channel=Emirhan%C5%9Eenkal
/// </summary>
public class ObjPoolTest : MonoBehaviour
{
    
    [SerializeField] public float spawnInterval = 1;
    [SerializeField] public ObjectPool objectPool = null;

    void Start()
    {
        StartCoroutine(nameof(SpawnRoutine));
    }
    
    private IEnumerator SpawnRoutine()
    {
        int counter = 0;
        while (true)
        {
            GameObject obj = objectPool.GetPooledObject(counter++ % 2);
            
            obj.transform.position = Vector3.zero;

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
}