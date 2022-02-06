// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic movement in 3D by using keyboard.
/// Attach this script to gameObject that will be moved.
/// </summary>

public class SuperBasicMovement : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    
    void Update()
    {
        if (Input.GetKey (KeyCode.D))
        {
            ForwardingAngle(90);
        }
        if (Input.GetKey (KeyCode.A)) {
            ForwardingAngle(-90);
        }
        if (Input.GetKey (KeyCode.W)) {
            ForwardingAngle(0);
        }
        if (Input.GetKey (KeyCode.S)) {
            ForwardingAngle(180);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            ForwardingAngle(45);
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            ForwardingAngle(-45);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            ForwardingAngle(135);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            ForwardingAngle(-135);
        }

        if (Input.anyKey)
        {
            Movement();
        }
    }

    private void Movement()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void ForwardingAngle(float angleValue)
    {
        transform.eulerAngles = new Vector3(0,angleValue,0);
        
    }
}
