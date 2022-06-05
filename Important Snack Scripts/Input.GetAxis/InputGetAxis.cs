// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Input.GetAxis(string axisName)
///
/// Returns the value of the virtual axis identified by axisName.
/// The value will be in the range [-1,1] for keyboard and joystick input devices.
///  
/// The meaning of this value depends on the type of input control, for example with
/// a joystick's horizontal axis a value of 1 means the stick is pushed all the way
/// to the right and a value of -1 means it's all the way to the left; a value of 0
/// means the joystick is in its neutral position.
///
/// Options can be changed for axisName from Edit > Project Settings > Input Manager.
///
/// NOTE : THE HORIZONTAL AND VERTICAL RANGER CHANGE FROM 0 TO +1 OR -1 WITH INCREASE/DECREASE
/// IN 0.05F STEPS. GETAXISRAX HAS CHANGES FROM 0 TO 1 OR -1 IMMEDIATELY, SO WITH NO STEPS.
/// 
/// Ref: https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
/// </summary>


// A very simplistic car driving on the x-z plane.
public class InputGetAxis : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float rotationSpeed = 100.0f;

    private void Update()
    {
        CarController();
        //PerformMouseLook();
    }

    private void CarController()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The values in in range -1 to 1

        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        
        // Make it move 10 meters per second instead of 10 meters per frame...

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        
        // Mode translation along the object's z-axis
        transform.Translate(0,0,translation);
        
        // Rotate around our y axis.
        transform.Rotate(0,rotation,0);
    }
    
    //////
    
    // Performs a mouse look.

    // [SerializeField] private float horizontalSpeed=2.0f;
    // [SerializeField] private float verticalSpeed=2.0f;
    //
    // private void PerformMouseLook()
    // {
    //     // Get the mouse delta. This is not in the range -1...1
    //     float h = horizontalSpeed * Input.GetAxis("Mouse X");
    //     float v = verticalSpeed * Input.GetAxis("Mouse Y");
    //
    //     transform.Rotate(v, h, 0);
    // }
    //

}
