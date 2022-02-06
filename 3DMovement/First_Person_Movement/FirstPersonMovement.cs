// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this script to parent object oh the player.
///
/// Add a child gameObject as a GroundChecker (The most bottom part of the player) to check, if the player is touching the ground.
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    private CharacterController _characterController;

    [Header("General Variables")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 currentVelocity;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float runMultuplier = 1.5f;
    [SerializeField] private float leanMultiplier = 0.5f;
    
    [Header("Ground Related Components")]
    [SerializeField] private Transform groundCheck; // The most bottom part of the player.
    [SerializeField] private float groundDistance = 0.4f; // Radius of the sphere to check isGrounded.
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;
    
    
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();
        Falling();
        
        Spirit();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        
        
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move =transform.right * x + transform.forward * z;
        _characterController.Move(move * speed * Time.deltaTime);
    }

    private void Falling()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && currentVelocity.y < 0)
        {
            currentVelocity.y = -2f;
        }
        
        currentVelocity.y += gravity * Time.deltaTime;
        _characterController.Move(currentVelocity * Time.deltaTime);
    }

    private void Jump()
    {
        currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void Spirit()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = speed * runMultuplier;
        }
        
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = speed / runMultuplier;
        }
    }
}
