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
public class AppleWholeState : AppleBaseState
{
    private float _rottenCountDown = 10.0f;
    public override void EnterState(AppleStateManager apple)
    {
        Debug.Log("Hello from the Whole State");
        apple.GetComponent<Rigidbody>().useGravity = true;
    }


    public override void UpdateState(AppleStateManager apple)
    {
        if (_rottenCountDown >= 0)
        {
            _rottenCountDown -= Time.deltaTime;
        }
        else
        {
            apple.SwitchState(apple.RottenState);
        }
    }


    public override void OnCollisionEnter(AppleStateManager apple, Collision collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("Player"))
        {
            //other.GetComponent<PlayerController>().addHealth();
        }
    }
}
