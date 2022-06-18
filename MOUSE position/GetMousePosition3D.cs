// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Getting the mouse position information in 2D games.
/// 
/// Attach this script to a gameObject will be teleported to mouse position
/// , that will be followed by the Camera.
///
/// NOTE: CAMERA NEED TO BE PERSPECTIVE TO USE 3D
///
/// Ref: https://www.youtube.com/watch?v=0jTPKz3ga4w
/// </summary>

public class GetMousePosition3D : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public bool withLayerMask=false;
    [SerializeField] private LayerMask mouseColliderForLayerMask;
    

    private void Awake()
    {
        mainCamera.orthographic = false;
        
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update ()
    {
        
        if (withLayerMask)
        {
            MouseFollower3DwithLayer();
        }
        else
        {
            MouseFollower3D();    
        } 
    }
    

    /// <summary>
    /// Camera sending ray to screen and getting location information at the first collider that it interacted.
    /// 
    /// Attach this script to parent-object of the child object to be moved in the game as a mouse.
    ///
    /// NOTE: TURN OFF THE COLLIDER COMPONENET OF THE CHILD OBJECT/ OBJECTS.
    /// </summary>
    private void MouseFollower3D()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit) )
        {
            transform.position = raycastHit.point;
            //Debug.Log(transform.position);
        }
    }
    
    /// <summary>
    /// Same as 3D follower but only works on a certain layer.
    /// 
    /// Attach this script to parent-object of the child object to be moved in the game as a mouse.
    ///
    /// NOTE: TURN OFF THE COLLIDER COMPONENET OF THE CHILD OBJECT/ OBJECTS.
    /// </summary>
    private void MouseFollower3DwithLayer()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue,mouseColliderForLayerMask) )
        {
            transform.position = raycastHit.point;
            //Debug.Log(transform.position);
        }
    }

    
}
