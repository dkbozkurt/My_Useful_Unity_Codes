// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionBehaviour : MonoBehaviour
{

    [SerializeField] private LayerMask mouseColliderForLayerMask;
    [SerializeField] private Vector3 offSetValue;
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    
    void Update ()
    {
        if(Input.GetMouseButton(0)) MouseFollower3DwithLayer();
    }

    private void MouseFollower3DwithLayer()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue,mouseColliderForLayerMask) )
        {
            transform.position = raycastHit.point + offSetValue;
        }
    }
}
