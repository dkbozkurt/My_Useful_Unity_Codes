// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Attach this script into the object that will rotate.
/// 
/// Quaternion.Slerp(a,b,t)
/// Spherically interpolates between a and b by t. The parameter t is clamped to the range [0,1].
/// 
/// </summary>

public class ObjectRotationBasics : MonoBehaviour
{
    [Header(".Rotate")] 
    [SerializeField] private bool controlByKeyboard;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _speed=100f;
    
    [Header("Slerp")]
    [SerializeField] private Quaternion targetAngle= Quaternion.Euler(0,0,90);
    [SerializeField] private float intervalDivider =0.2f;

    private void Update()
    {
        DoRotate();
        //DoSlerp();
    }

    // Favourite one.
    private void DoRotate()
    {
        if (controlByKeyboard)
        {
            if (Input.GetKey(KeyCode.W)) _rotation = Vector3.forward;
            else if (Input.GetKey(KeyCode.S)) _rotation = Vector3.back;
            else if (Input.GetKey(KeyCode.A)) _rotation = Vector3.right;
            else if (Input.GetKey(KeyCode.D))_rotation = Vector3.left;
            else _rotation = Vector3.zero;
        }
        
        transform.Rotate(_rotation * _speed * Time.deltaTime);
    }
    
    private void DoSlerp()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,targetAngle, intervalDivider);
    }
    
    
}
