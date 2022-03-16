// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Attach this script onto an empty GameObject.
///
/// Plane.Raycast method creates an invisible plane, gets precise word point, INFINITE RANGE, requires slightly more calculation.
/// 
/// Ref : https://www.youtube.com/watch?v=RGjojuhuk_s&ab_channel=BoardToBitsGames
/// </summary>

// Method 4 from the video
public class ClickPositionByPlaneRaycast : MonoBehaviour
{
    private Vector3 _clickPosition;
    private float _distanceToPlane;
    private Plane _plane;

    private void Start()
    {
        GeneratePlane();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) RaycastUsingPlane();
    }

    private void GeneratePlane()
    {
        _plane = new Plane(Vector3.up, 0f);
    }

    private void RaycastUsingPlane()
    {
        _clickPosition = -Vector3.one;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(ray, out _distanceToPlane))
        {
            _clickPosition = ray.GetPoint(_distanceToPlane);
        }

        Debug.Log("Click Position: " + _clickPosition);
    }
}
