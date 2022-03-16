// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this script onto an object that you want it to be react to clicks on it.
///
/// Note: Clickable object must have a Collider on it.
/// Ref : https://www.youtube.com/watch?v=RGjojuhuk_s&ab_channel=BoardToBitsGames
/// </summary>

[RequireComponent(typeof(Collider))]
public class UnitClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log(name + " was clicked.\n And its center location is " + transform.position);
    }
}
