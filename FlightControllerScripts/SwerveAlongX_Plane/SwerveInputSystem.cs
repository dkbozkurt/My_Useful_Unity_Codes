// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type 1

/// <summary>
/// This script need to be used with "SwerveMovement.cs" script.
/// Attach this script to gameObject.
///
/// This script controls the inputs from the user. Sends the x location of the input and sends it to "SwerveMovement.cs" script.
/// 
/// </summary>

public class SwerveInputSystem : MonoBehaviour
{
    
    private float _lastFrameFingerPositionX;
    private float _moveFactorX;
    
    // Get function of private _moveFactorX;
    public float MoveFactorX => _moveFactorX;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            _lastFrameFingerPositionX = Input.mousePosition.x;

        }
        else if (Input.GetMouseButton(0) || (Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
            // Debug.Log("movement:" + _moveFactorX);
            _lastFrameFingerPositionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0) || (Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            _moveFactorX = 0f;
        }

    }

    
}
