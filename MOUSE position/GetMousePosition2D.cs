// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// 
/// Getting the mouse position information in 2D games.
/// 
/// Attach this script to a gameObject will be teleported to mouse position
/// , that will be followed by the Camera.
/// 
/// NOTE: CAMERA NEED TO BE ORTOGRAPHIC TO USE 2D
///
/// Ref: https://www.youtube.com/watch?v=0jTPKz3ga4w
/// </summary>

public class GetMousePosition2D : MonoBehaviour
{
    [SerializeField] [CanBeNull]private Camera mainCamera;

    private void Awake()
    {
        mainCamera.orthographic = true;
        
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update ()
    {
        MouseFollower2D();
    }
    
    private void MouseFollower2D()
    {
        // Debug.Log(Input.mousePosition);
        // Debug.Log(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;

    }
    
}
