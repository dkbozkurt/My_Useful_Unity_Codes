// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type 3

/// <summary>
/// Attach this script to parent object of the player.
/// 
/// This script works with "ManagerJoystick.cs" script that attached to the joystick that is handmade(not from the store).
/// 
///  Important Note: Freeze position and Freeze rotations X,Y,Z components should be checked for the child object(player),
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class CharacterControlManager : MonoBehaviour
{
    //
    private Vector3 v_movement;
    private CharacterController _characterController;
    public float moveSpeed;
    
    //
    private Transform meshPlayer;
    private float gravity;
    
    //
    [SerializeField]private ManagerJoystick _managerJoystick;
    private float inputX;
    private float inputZ;
    
    //
    public float rotationSpeed;

    private void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        moveSpeed = 0.1f;
        gravity = 0.5f;
        rotationSpeed = 1f;

        meshPlayer = gameObject.transform.GetChild(0);
    }

    private void Update()
    {
        //inputX = Input.GetAxis("Horizontal");
        //inputZ = Input.GetAxis("Vertical");
        
        inputX  =_managerJoystick.inputHorizontal();
        inputZ = _managerJoystick.inputVertical();

        
    }

    private void FixedUpdate()
    {
        // gravity
        if (_characterController.isGrounded)
        {
            v_movement.y = 0f;
        }
        else
        {
            v_movement.y -= gravity * Time.deltaTime;
        }
        
        // Movement
        v_movement = new Vector3(inputX*moveSpeed, 0, inputZ*moveSpeed);
        _characterController.Move(v_movement);
        
        // mesh rotate
        if (inputX != 0 || inputZ != 0)
        {
            Vector3 lookDir = new Vector3(v_movement.x*rotationSpeed, v_movement.y, v_movement.z*rotationSpeed);
            meshPlayer.rotation = Quaternion.LookRotation(lookDir);    
        }
        
        
        
    }
}
