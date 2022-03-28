// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basically, if we add ? after the variable type it means nullable.
///
/// Can be used with types: int, bool, Vector3
/// </summary>

public class NullableType : MonoBehaviour
{
    
    private Vector3? positionToMove; // Nullable!
    private Vector3 positionToBegin; // Not nullable, syntax error

    private bool? c;
    private int? i;
    private void Start()
    {
        if (positionToMove != null)
        {
            Debug.Log("Destination location set!");
        }
        else
        {
            Debug.Log("Destination location empty!");
        }
    }
}

