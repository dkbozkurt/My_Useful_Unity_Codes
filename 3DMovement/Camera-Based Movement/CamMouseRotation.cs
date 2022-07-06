// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mouse Input Rotation
///
/// Depends on the mouse location on the screen, camera will rotate.
///
/// Attach this script into Camera.
/// 
/// Ref : https://www.youtube.com/watch?v=CxI2OBdhLno&ab_channel=RoyalSkies
/// </summary>

public class CamMouseRotation : MonoBehaviour
{
    [SerializeField] private bool cursorStatus=true;
    [SerializeField] private float sensitivity = 2f;

    public Vector2 turnValues;

    private void Start()
    {
        if(!cursorStatus) HideCursor();
    }

    private void Update()
    {
        CamRotationRespectToMouse();
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CamRotationRespectToMouse()
    {
        turnValues.x += Input.GetAxis("Mouse X") * sensitivity;
        turnValues.y += Input.GetAxis("Mouse Y") * sensitivity;
        transform.localRotation = Quaternion.Euler(-turnValues.y,turnValues.x,0);
        
    }
}
