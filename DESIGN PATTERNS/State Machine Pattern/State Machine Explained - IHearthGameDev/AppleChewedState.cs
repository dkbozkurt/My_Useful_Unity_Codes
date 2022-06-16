// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

/// <summary>
/// State Machine Pattern
/// 
/// 
/// Ref : https://youtu.be/Vt8aZDPzRjI
/// </summary>

// # Concrete State #
public class AppleChewedState : AppleBaseState
{
    private float _destroyCountdown = 5.0f;
    
    public override void EnterState(AppleStateManager apple)
    {
        Animator animator = apple.GetComponent<Animator>();
        animator.Play("Base Layer.eat",0,0);
    }

    public override void UpdateState(AppleStateManager apple)
    {
        if (_destroyCountdown > 0)
        {
            _destroyCountdown -= Time.deltaTime;
        }
        else
        {
            Object.Destroy(apple.gameObject);
        }
    }


    public override void OnCollisionEnter(AppleStateManager apple, Collision collision)
    {

    }
}
