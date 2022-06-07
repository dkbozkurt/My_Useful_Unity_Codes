// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inheritance
/// Ref : https://www.youtube.com/watch?v=8TIkManpEu4
/// </summary>

public class Inheritance_PlayerCharacter : Inheritance_Character
{
    // This attribute always going to be after the base class properties.
    [SerializeField] private string playerClassName = "Bard";

    protected override void Update()
    {
        // Calls the base classes update function.
        base.Update();
        DoMovementFromInput();
    }

    private void DoMovementFromInput()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        transform.position += movement * Time.deltaTime * moveSpeed;
    }
}
