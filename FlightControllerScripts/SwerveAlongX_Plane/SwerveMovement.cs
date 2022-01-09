// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type 1

/// <summary>
/// This script prepared for swerve mechanism with a basic flight(plane).
/// Swerves along x-axis and continuously moves along y-axis.
/// 
/// This script need to be used with "SwerveInputSystem.cs" script.
/// Attach this script to gameObject.
///
/// This script receives input information and depends on the input this script,
/// swerves along x-axis, moves in the y axis, rotates depends on the
/// MoveFactorX (Inputs X info) with a changeable rotationWay checker.
/// 
/// </summary>
[RequireComponent(typeof(SwerveInputSystem))]
public class SwerveMovement : MonoBehaviour
{
    private SwerveInputSystem _swerveInputSystem;
    
    #region Swerve along x-axis

    private float swerveAmount;
    [SerializeField] private float swerveSpeed = 10f;
    [SerializeField] private float maxSwerveAmount = 1f;

    #endregion

    #region Movement in the y-axis

    [SerializeField] private float _forwardSpeed = 5f;
    private float _objectHeight;

    #endregion

    #region Rotation

    public float _rotationSpeed = 100f;
    public bool inverseRotationWay = false;
    private int _rotationMultiplier = 1;

    #endregion
    private void Awake()
    {
        _swerveInputSystem = GetComponent<SwerveInputSystem>();
        _objectHeight = transform.position.y;
        RotationLogicChanger();
    }

    private void Update()
    {
        MoveOnX();
        MoveOnY();
        ObjectRotation();
    }
    
    private void MoveOnX()
    {
        swerveAmount = Time.deltaTime* swerveSpeed* _swerveInputSystem.MoveFactorX;
        swerveAmount = Mathf.Clamp(swerveAmount,-maxSwerveAmount, maxSwerveAmount);
        transform.Translate(swerveAmount,0,0);
    }

    private void MoveOnY()
    {
        transform.position = new Vector3(transform.position.x, _objectHeight,
            transform.position.z + _forwardSpeed * Time.deltaTime);
    }

    private void ObjectRotation()
    {
        if (_swerveInputSystem.MoveFactorX > 0)
        {
            transform.Rotate(0,0, _rotationMultiplier*(_rotationSpeed* Time.deltaTime));
        }
        else if (_swerveInputSystem.MoveFactorX <0)
        {
            transform.Rotate(0,0,-_rotationMultiplier*( _rotationSpeed* Time.deltaTime));
        }
    
    }

    private void RotationLogicChanger()
    {
        if (inverseRotationWay)
            _rotationMultiplier=1;
        else
        {
            _rotationMultiplier=-1;
        }
        
    }
}
