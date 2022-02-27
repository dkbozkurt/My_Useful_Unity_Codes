// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Attach this script into the gameObject to be moved.
///  
/// Ref : https://www.youtube.com/watch?v=UyHxhXtE3wU
/// </summary>

public class MovingToMousePosition : MonoBehaviour
{
    private Vector3 cursorPosition;
    
    // Distance to move current position per call.
    [SerializeField] private float maxDistanceDelta = 0.2f;

    private void Update()
    {
        MoveToMousePosition();
    }

    private void MoveToMousePosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // z value is the distance between the camera and the object to be moved.
            // Edit depend on which surfaces ur working on.
            cursorPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));    
        }

        transform.position = Vector3.MoveTowards(transform.position, cursorPosition, maxDistanceDelta);

    }
}
