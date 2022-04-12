// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NewMovementPanel
///
/// Ref: https://www.youtube.com/watch?v=hnxzYdnjH1U
/// </summary>

// Subject
public class NewMovementPanel : MonoBehaviour
{
    
    // Without input values
    public static event Action OnForwardButtonClicked;
    public static event Action OnBackButtonClicked;
    public static event Action OnRightButtonClicked;
    public static event Action OnLeftButtonClicked;
    
    // With input values
    public static event Action<Vector3> OnButtonClicked;

    
    // Without input values
    // public void ForwardOnClick()
    // {
    //     OnForwardButtonClicked?.Invoke();   
    // }
    //
    // public void BackOnClick()
    // {
    //     OnBackButtonClicked?.Invoke();
    // }
    //
    // public void RightOnCLick()
    // {
    //     OnRightButtonClicked?.Invoke();
    // }
    //
    // public void LeftOnClick()
    // {
    //     OnLeftButtonClicked?.Invoke();
    // }

    // with input values
    
    public void ForwardOnClick()
    {
        OnButtonClicked?.Invoke(Vector3.forward);
    }

    public void BackOnClick()
    {
        OnButtonClicked?.Invoke(Vector3.back);
    }

    public void RightOnCLick()
    {
        OnButtonClicked?.Invoke(Vector3.right);
    }

    public void LeftOnClick()
    {
        OnButtonClicked?.Invoke(Vector3.left);
    }
}
