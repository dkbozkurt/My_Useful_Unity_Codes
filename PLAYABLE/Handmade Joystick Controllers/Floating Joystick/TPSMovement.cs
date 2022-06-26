using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class TPSMovement : MonoBehaviour
{
    private CharacterController _characterController;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;

    [SerializeField] private JoystickManager _joystickManager;

    private float inputX;
    private float inputZ;

    public static bool playerIsMoving= false;

    [SerializeField] private Animator _playerAnimator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        inputX = _joystickManager.inputHorizontal();
        inputZ = _joystickManager.inputVertical();
        
        Vector3 direction = new Vector3(inputX, 0f, inputZ).normalized;
        
        if (direction.magnitude >= 0.1f)
        {
            Rotate(direction);
            _characterController.Move(direction * (speed * Time.deltaTime));
            playerIsMoving = true;
            _playerAnimator.SetBool("Run",true);
            
            return;
        }
        
        _playerAnimator.SetBool("Run",false);
        playerIsMoving = false;

    }

    private void Rotate(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        
        // Adding smooth turning
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        
        transform.rotation = Quaternion.Euler(0f,angle,0f);
    }
}
