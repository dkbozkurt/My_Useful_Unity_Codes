// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type 3

/// <summary>
/// Add this script the plane's parent object it must have charactercontroller component.
///
/// 
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    private CharacterController _controller;

    #region Speed Variables

    public float baseSpeed = 10.0f;
    public float rotSpeedX = 3.0f;
    public float rotSpeedY = 1.5f;

    #endregion

    #region Movement Variables

    private Vector3 moveVector;
    private Vector3 inputs;
    private Vector3 yaw, pitch, directon;
    private float maxX;
    
    #endregion

    private GameObject _parentPlane;
    private void Awake()
    {
        // If we use RequireComponent dont need to use the following lines
        // if (gameObject.GetComponent<CharacterController>() == null)
        //     gameObject.AddComponent(typeof(CharacterController));
        
        // CreateNewGameobject();
    }

    private void Start()
    {
         _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
      PlayersInfo();
      DeltaDirection();
      LimitFunction();
      MoveObject();
    }

    // Add a parent gameObject to the main gameObject. 
    private void CreativeNewGameobject()
    {
        _parentPlane = new GameObject("ParentPlane");
        _parentPlane.transform.position = gameObject.transform.position;
        _parentPlane.AddComponent<CharacterController>();
        gameObject.transform.SetParent(_parentPlane.transform);
    }
    
    // Give the player forward velocity and gather the player's input.
    private void PlayersInfo()
    {
        moveVector = transform.forward * baseSpeed;
        inputs = Manager.Instance.GetPlayerInput();

    }
    // Get the delta direction
    private void DeltaDirection()
    {
        yaw = inputs.x * transform.right * rotSpeedX * Time.deltaTime;
        pitch = inputs.y * transform.up * rotSpeedY * Time.deltaTime;
        directon = yaw + pitch;
    }
    
    // Make sure to limit the player doing loop so adjusting max values
    private void LimitFunction()
    {
        maxX = Quaternion.LookRotation(moveVector + directon).eulerAngles.x;

        if (maxX < 90 && maxX > 70 || maxX > 270 && maxX < 290)
        { 
            // Do nothing.
        }
        else
        {
            // Add the direction to the current move
            moveVector += directon;
            
            // Have the player face where he is going
            transform.rotation = Quaternion.LookRotation(moveVector);
        }
    }

    private void MoveObject()
    {
        _controller.Move(moveVector * Time.deltaTime);
    }
}
