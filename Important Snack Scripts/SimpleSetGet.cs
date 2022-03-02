// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class SimpleSetGet : MonoBehaviour
{
    // Assigning variables with an easy way.
    private bool _movementEnabled=false;
    public bool MovementEnabled { get => _movementEnabled; set => _movementEnabled = value; }
    
    // Only get version
    private int _health = 100;
    private int Health => _health;

    private void Start()
    {
        Debug.Log("Movement enabled: " + MovementEnabled);
        
        MovementEnabled = true;
        
        Debug.Log("Movement enabled: " + MovementEnabled);

        // Gives "has no setter" error
        //Health = 5;
        Debug.Log("Health amount: " + Health);
        
    }

}
