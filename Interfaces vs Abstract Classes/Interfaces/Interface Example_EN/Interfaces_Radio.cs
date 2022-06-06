// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ref : https://www.youtube.com/watch?v=2LA3BLqOw9g
///
/// </summary>

public class Interfaces_Radio : MonoBehaviour,IInteractable
{

    private void Toggle()
    {
        Debug.Log("Toggle get trigerred!");
    }
    public void Interact()
    { 
        Toggle();   
    }
    



}
