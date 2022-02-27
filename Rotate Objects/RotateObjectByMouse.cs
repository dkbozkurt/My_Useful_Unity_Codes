// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotator
///
/// To rotate 3d object by using mouse.
///
/// Attach this script into the gameObject.
/// 
/// Ref : https://www.youtube.com/watch?v=HRYodomxd-E
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class RotateObjectByMouse : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;
    private bool dragging = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseDrag()
    {
        dragging = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
    }

    private void FixedUpdate()
    {
        if (dragging)
        {
            Rotator();
        }
    }

    private void Rotator()
    {
        float x = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
        float y = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;
        
        rb.AddTorque(Vector3.down * x);
        rb.AddTorque(Vector3.right * y);
    }
}
