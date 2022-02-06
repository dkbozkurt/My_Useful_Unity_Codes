// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

/// <summary>
/// Attach this script to parent object oh the player.
///
/// NOTE: Parent object should have camera gameObject in its childs. (For FPS Look)
/// </summary>

public class FirstPersonMouseLook : MonoBehaviour
{
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private float mouseSensitivity = 1000f;

    // For LookAroundAlongY() method.
    private float xRotation = 0f;
    
    private void Start()
    {
        // Avoids cursor to look in the game scene.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        LookAround();
    }

    // Looks around and rotates body.
    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        LookAroundAlongY(mouseY);
        LookAroundAlongX(mouseX);
    }

    private void LookAroundAlongX(float mouseX)
    {
        transform.Rotate(Vector3.up * mouseX);
    }

    private void LookAroundAlongY(float mouseY)
    {
        xRotation -= mouseY;
        
        // To avoid player to look all around its body along y
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        fpsCamera.transform.localRotation =Quaternion.Euler(xRotation,0f,0f);
        
    }
    
    
}
