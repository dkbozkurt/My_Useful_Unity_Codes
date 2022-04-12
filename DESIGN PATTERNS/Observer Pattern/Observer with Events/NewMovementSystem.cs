// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NewMovementSystem
/// 
/// Ref: https://www.youtube.com/watch?v=hnxzYdnjH1U
/// </summary>

// Observer
public class NewMovementSystem : MonoBehaviour
{
    // Without input values
    // private void OnEnable()
    // {
    //     NewMovementPanel.OnForwardButtonClicked += MoveForward;
    //     NewMovementPanel.OnBackButtonClicked += MoveBack;
    //     NewMovementPanel.OnLeftButtonClicked += MoveLeft;
    //     NewMovementPanel.OnRightButtonClicked += MoveRight;
    // }
    //
    // private void OnDestroy()
    // {
    //     NewMovementPanel.OnForwardButtonClicked -= MoveForward;
    //     NewMovementPanel.OnBackButtonClicked -= MoveBack;
    //     NewMovementPanel.OnLeftButtonClicked -= MoveLeft;
    //     NewMovementPanel.OnRightButtonClicked -= MoveRight;
    // }
    
    // With input values

    private void OnEnable()
    {
        NewMovementPanel.OnButtonClicked += Move;
    }

    private void OnDestroy()
    {
        NewMovementPanel.OnButtonClicked -= Move;
    }

    // Without input values
    public void MoveForward() => transform.Translate(Vector3.forward);
    public void MoveBack() => transform.Translate(Vector3.back);
    public void MoveLeft() => transform.Translate(Vector3.left);
    public void MoveRight() => transform.Translate(Vector3.right);
    
    // With input values
    public void Move(Vector3 direction) => transform.Translate(direction);
}
