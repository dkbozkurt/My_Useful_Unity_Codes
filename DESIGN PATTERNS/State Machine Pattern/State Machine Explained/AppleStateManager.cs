// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State Machine Pattern
///
/// The context passes data to our states.
/// 
/// This script is the Context of the state machine.
///
/// Add this script to apple prefab that you want to use in game.
///  
/// Ref : https://youtu.be/Vt8aZDPzRjI
/// </summary>

// # Context # 
public class AppleStateManager : MonoBehaviour
{
    // currentState holds a reference to the active state in a state machine
    // Remember, state machines can only be in one state at a time.
    private AppleBaseState currentState;

    // Instantiating our four concrete states each will use their type
    public AppleGrowingState GrowingState = new AppleGrowingState();
    public AppleWholeState WholeState = new AppleWholeState();
    public AppleChewedState ChewedState = new AppleChewedState();
    public AppleRottenState RottenState = new AppleRottenState();
    public AppleSuperState SuperState = new AppleSuperState();

    private void Start()
    {
        currentState = GrowingState;

        // "this" is a reference to the context. (this EXACT MonoBehaviour script)
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this,collision);
    }

    // The method has the ability to transition from one state to another.
    public void SwitchState(AppleBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}