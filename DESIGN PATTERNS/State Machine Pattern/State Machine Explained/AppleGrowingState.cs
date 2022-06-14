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
public class AppleGrowingState : AppleBaseState
{
    private Vector3 _startingAppleSize = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 _growAppleScalar = new Vector3(0.1f, 0.1f, 0.1f);
    
    public override void EnterState(AppleStateManager apple)
    {
        Debug.Log("Hello from the Growing State");
        apple.transform.localScale = _startingAppleSize;
    }

    // Here we can use ".SwitchState", we have access to the context because,
    // context is passed as an argument to the update state function.
    public override void UpdateState(AppleStateManager apple)
    {
        if (apple.transform.localScale.x < 1)
        {
            apple.transform.localScale += _growAppleScalar * Time.deltaTime;
        }
        else if (Random.value >0.5f)
        {
            apple.SwitchState(apple.WholeState);
        }
        else
        {
            apple.SwitchState(apple.SuperState);
        }
    }

    public override void OnCollisionEnter(AppleStateManager apple, Collision collision)
    {
        
    }

}
