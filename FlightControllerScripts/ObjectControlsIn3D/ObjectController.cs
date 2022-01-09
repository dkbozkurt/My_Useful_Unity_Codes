// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type 2

/// <summary>
///
/// Moving the object in the 3D world by keyboard and mouse controls.
/// Better to use on as a spaceship kind objects.
///
/// Attach this script to a gameObject to be moved.
/// 
/// Important Note: You have to create new axis called "Hover" to use this script.
/// Edit> Project Settings> Input Manager> Vertical:Right Clicl"Duplicate Array element"> Then Re-name as Hover.
/// Change positive and negative buttons and remove alt neg button and alt pos button key codes.
/// </summary>

public class ObjectController : MonoBehaviour
{
    #region Speed Variables

    public float forwardSpeed = 25f;
    public float strafeSpeed = 20f;
    public float hoverSpeed = 5f;
    
    #endregion
    

    #region Activate Speed Variables


    private float activeForwardSpeed;
    private float activeStrafeSpeed;
    private float activeHoverSpeed;

    #endregion

    #region Acceleration Variables (For smooth movement)

    private float forwardAcceleration =2.5f;
    private float strafeAcceleration = 2f;
    private float hoverAcceleration = 2f;

    #endregion

    #region Rotation Variables

    public float lookRateSpeed = 90f;
    private Vector2 lookInput;
    private Vector2 screenCenter;
    private Vector2 mouseDistance;

    #endregion

    #region RoolVariables

    private float rollInput;
    public float rollSpeed = 90f;
    public float rollAcceleration = 3.5f;


    #endregion
    void Start()
    {
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        // Lock the cursor in the game scene.
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        VerticalMovement();
        HorizontalMovement();
        HoverMovement();
        RollMovement();
        RotationMovement();
    }

    // Movement on z-axis(forward axis)
    private void VerticalMovement() 
    {
        // Without acc
        //activeForwardSpeed = Input.GetAxisRaw("Vertical") * forwardSpeed;

        // With acc
        activeForwardSpeed = Mathf.Lerp (activeForwardSpeed,
            Input.GetAxisRaw("Vertical") * forwardSpeed,
            forwardAcceleration * Time.deltaTime);
        
        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
    }

    // Movement on x-axis
    private void HorizontalMovement()
    {
        // Without acc
        //activeStrafeSpeed = Input.GetAxisRaw("Horizontal") * strafeSpeed;
        
        // With acc
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed,
            Input.GetAxisRaw("Horizontal") * strafeSpeed,
            strafeAcceleration * Time.deltaTime);
        
        
        transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;

    }

    // Movement on y-axis (Space to increase & Left ctrl to decrease)
    private void HoverMovement()
    {
        // Without acc
        //activeHoverSpeed = Input.GetAxisRaw("Hover") * hoverSpeed;
        
        // With acc
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed,
            Input.GetAxisRaw("Hover") * hoverSpeed,
            hoverAcceleration * Time.deltaTime);
        
        transform.position += transform.up * activeHoverSpeed * Time.deltaTime;
    }

    // Rotation By using mouse respect to center of the screen
    private void RotationMovement()
    {
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);
        
        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, 
            mouseDistance.x * lookRateSpeed * Time.deltaTime, 
            rollInput * rollSpeed * Time.deltaTime, Space.Self);
        
    }

    // Rotation of the object around its z-axis.(e to positive side, q to negative side)
    private void RollMovement()
    {
        rollInput = Mathf.Lerp(rollInput,
            Input.GetAxisRaw("Roll"),
            rollAcceleration * Time.deltaTime);
    }
    
}
