// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this script into the Main Object that would be effected by recursion.
///
/// Ref : https://www.youtube.com/watch?v=Z6VwROzpSZg&ab_channel=StuartSpence
/// </summary>

public class SimpleRecursion : MonoBehaviour
{
    
    // Distance vector of the object to be duplicated from the parent object
    [SerializeField] private Vector3 shiftingVector;

    [Header("Limit values")]
    [SerializeField] private float recursionStopValue=0.1f;
    [SerializeField] private float dividerStepValue=2f;
    
    private void Start()
    {
        Recursion();
    }

    private void Recursion()
    {
        if (transform.localScale.x > recursionStopValue)
        {
            GameObject newObject = Instantiate(gameObject) as GameObject;
            newObject.transform.localScale = transform.localScale / dividerStepValue;
            newObject.transform.position = new Vector3(
                transform.position.x + shiftingVector.x,
                transform.position.y + shiftingVector.y,
                transform.position.z + shiftingVector.z
                );
        }
        
    }
}
