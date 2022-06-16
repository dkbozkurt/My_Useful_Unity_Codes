// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

/// <summary>
/// State Machine Pattern
/// 
/// Lastly added extra concrete state
/// Ref : https://youtu.be/Vt8aZDPzRjI
/// </summary>

// # Concrete State # 
public class AppleSuperState : AppleBaseState
{
    public override void EnterState(AppleStateManager apple)
    {
        apple.GetComponent<Rigidbody>().useGravity = true;
        Animator animator = apple.GetComponent<Animator>();
        animator.Play("Base Layer.super", 0, 0);
    }

    public override void UpdateState(AppleStateManager apple)
    {
    }

    public override void OnCollisionEnter(AppleStateManager apple, Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            // other.GetComponent<PlayerController>().addHealth();
            // other.GetComponent<PlayerController>().addExtraHealth();
            
            apple.SwitchState(apple.ChewedState);
        }
    }
}