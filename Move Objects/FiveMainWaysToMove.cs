// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 5 ways to move Unity3D objects
///
/// Attach this script to empty gameObject and assign the target gameObject to be moved.
///
/// NOTES:
/// 
/// Deciding tips:
/// If you dont want to interact with the physics system (Rigidbody's is kinematic is true):
/// Rigidbody_SetVelocity and Rigidbody_SetVelocity are not gonna work. 
/// </summary>

public enum MovementType
{
    Transform_SetPosition,
    Transform_Translate,
    Rigidbody_AddForce,
    Rigidbody_MovePosition,
    Rigidbody_SetVelocity
    
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FiveMainWaysToMove : MonoBehaviour
{
    [Header("General Settings")] 
    public KeyCode MoveTriggerKeyCode= KeyCode.Space;
    [SerializeField] private MovementType _movementType= MovementType.Transform_SetPosition;
    
    
    [Header("GameObject Settings")]
    [SerializeField] private GameObject _gameObject; 
    [SerializeField] private float _speedMultiplier=10.0f;
    private Rigidbody objectRB;

    private void Awake()
    {
        objectRB=_gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(MoveTriggerKeyCode))
        {
            MovementTypeSelector();
        }
    }
    
    private void MovementTypeSelector()
    {
        switch (_movementType)
        {
            case MovementType.Transform_SetPosition:
                TransformSetPosition();
                break;
            case MovementType.Transform_Translate:
                TransformTranslate();
                break;
            case MovementType.Rigidbody_AddForce:
                RigidbodyAddForce();
                break;
            case MovementType.Rigidbody_MovePosition:
                RigidbodyMovePosition();
                break;
            
            case MovementType.Rigidbody_SetVelocity:
                RigidbodySetVelocity();
                break;
        }
    }
    
    #region Transform Based
    
    // Fav
    private void TransformSetPosition()
    {
        _gameObject.transform.position += transform.forward * _speedMultiplier * Time.deltaTime;
    }

    private void TransformTranslate()
    {
        _gameObject.transform.Translate(Vector3.forward * _speedMultiplier * Time.deltaTime); // ,Space.Self or Space.Word can define relativeTo
    }

    #endregion

    #region Rigidbody Based

    // Keep speeding up(adding force every frame) so it is the fastest.
    private void RigidbodyAddForce()
    {
        objectRB.AddForce(transform.forward * _speedMultiplier);
    }

    // Not recommended a lot.
    private void RigidbodyMovePosition()
    {
        Vector3 newPosition = _gameObject.transform.position + (transform.forward * _speedMultiplier * Time.deltaTime);
        objectRB.MovePosition(newPosition);
    }

    // Fav
    private void RigidbodySetVelocity()
    {
        objectRB.velocity = transform.forward * _speedMultiplier ;
    }

    #endregion
    
    
    
}
