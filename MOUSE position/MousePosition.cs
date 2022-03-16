// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;using UnityEditor;
using UnityEngine;

/// <summary>
/// 
/// Getting the mouse position information in 2D and 3D games.
/// 
/// Attach this script to a gameObject will be teleported to mouse position
/// , that will be followed by the Camera.
/// 
/// </summary>

public enum CameraType {
    Perspective, 
    Orthographic,
}

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public CameraType cameraType;

    public bool withLayerMask=false;
    [SerializeField] private LayerMask mouseColiderForLayerMask;
    //public bool onlyRayGround=false; 

    private void Awake()
    {
        if (cameraType == CameraType.Perspective)
        {
            mainCamera.orthographic = false;
            MouseFollower3D();    
        }

        if (cameraType == CameraType.Orthographic)
        {
            mainCamera.orthographic = true;
            MouseFollower2D();
        }
        
    }

    void Update ()
    {
        if (cameraType == CameraType.Perspective)
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
        
        if (cameraType == CameraType.Orthographic)
        {
            MouseFollower2D();
        }
        
    }
    
    /// <summary>
    /// NOTE: CAMERA NEED TO BE ORTOGRAPHIC TO USE 2D
    /// </summary>
    private void MouseFollower2D()
    {
        // Debug.Log(Input.mousePosition);
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;

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
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue,mouseColiderForLayerMask) )
        {
            transform.position = raycastHit.point;
            //Debug.Log(transform.position);
        }
    }

    
}
