// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ref : https://www.youtube.com/watch?v=2LA3BLqOw9g
/// </summary>

public class Interfaces_Lamp : MonoBehaviour,IInteractable
{
    private void Switch()
    {
        Debug.Log("Switch get triggered!");
    }
    public void Interact()
    {
        Switch();
    }
    
    



}
