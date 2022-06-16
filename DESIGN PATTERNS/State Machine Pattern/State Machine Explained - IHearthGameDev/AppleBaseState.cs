// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

/// <summary>
/// State Machine Pattern
///
/// Adding abstract to the method means, we are required to define their functionality
/// in each our our concrete states(derived classes).
/// And in the derived classes we used "override" in method names because we are going
/// to define the functionality differently in each file to reflect that concrete state.
/// 
/// 
/// This script will serve as Abstract state. 
/// Ref : https://youtu.be/Vt8aZDPzRjI
/// </summary>

// # Abstract State # 
public abstract class AppleBaseState
{
    public abstract void EnterState(AppleStateManager apple);

    public abstract void UpdateState(AppleStateManager apple);

    public abstract void OnCollisionEnter(AppleStateManager apple,Collision collision);

}

