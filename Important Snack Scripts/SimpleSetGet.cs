// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class SimpleSetGet : MonoBehaviour
{
    // Assigning variables with an easy way.
    private bool _movementEnabled=false;
    
    public bool MovementEnabled { get => _movementEnabled; set => _movementEnabled = value; }

    private void Start()
    {
        Debug.Log("Movement enabled: " + MovementEnabled);
        
        MovementEnabled = true;
        
        Debug.Log("Movement enabled: " + MovementEnabled);
    }

}
