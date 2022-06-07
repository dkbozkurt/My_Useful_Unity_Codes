// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inheritance
/// Ref : https://www.youtube.com/watch?v=8TIkManpEu4
/// </summary>

public class Inheritance_NPC : Inheritance_Character
{
    protected override void Update()
    {
        // Calls the base classes update function.
        base.Update();
        DoSmeAIMovement();
    }

    private void DoSmeAIMovement()
    {
        Debug.Log("AI movement is happening!");
    }
}