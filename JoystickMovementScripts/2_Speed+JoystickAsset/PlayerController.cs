// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type 2

/// <summary>
/// Movement with Joystick and speed(from rigidbody component)
/// 
/// Attach this script to main player.
///
/// Use this script with "Joystick Pack" asset and,"_joystick" variable must refer
/// the script that attached to the Joystick prefab coming from the asset.
/// 
/// Note: Freeze Position for X,Y,Z should be unchecked.
/// </summary>

[RequireComponent(typeof(Rigidbody),typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRB;
    [SerializeField]private FixedJoystick _joystick;

    public float _moveSpeed;

    private void Start()
    {
        _playerRB = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _playerRB.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _playerRB.velocity.y,
            _joystick.Vertical * _moveSpeed);

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_playerRB.velocity);
            // Animation will come here
        }
    }

}
