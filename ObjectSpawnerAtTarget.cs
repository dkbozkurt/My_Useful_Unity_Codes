// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Basic object spawner at assigned target position. (Use with mousePosition script as a target)
///
/// </summary>,

public class ObjectSpawnerAtTarget : MonoBehaviour
{
    private float timer; // = 0.01f;
    [Header("General Settings")]
    [SerializeField]private float delayValue = 0.04f;
    [SerializeField] private GameObject target;
    
    [Header("Object Prefabs")]
    [SerializeField] private GameObject[] _objectsToThrow;
    private int colorIndex;
    
    [Header("Object Scale Settings")]
    public float maxObjectScale=0.6f;
    public float minObjectScale=0.2f;

    private void Start()
    {
        colorIndex = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Throw();
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (colorIndex < _objectsToThrow.Length)
                colorIndex++;
            else
                colorIndex = 0;
        }
    }

    private void Throw()
    {
        Instantiate(EditedObject(), target.transform.position, target.transform.rotation);
        timer += delayValue;
    }
    
    private GameObject EditedObject()
    {
        return RandomScale(ChageColor());
    }

    private GameObject ChageColor()
    {
        return _objectsToThrow[colorIndex];
    }

    private GameObject RandomScale(GameObject obj)
    {
        float scaleIndex = Random.Range(minObjectScale,maxObjectScale);
        //Debug.Log("Scale index is: " + scaleIndex);
        obj.GetComponent<Transform>().localScale = new Vector3(scaleIndex, scaleIndex, scaleIndex);
        return obj;
    }

}
