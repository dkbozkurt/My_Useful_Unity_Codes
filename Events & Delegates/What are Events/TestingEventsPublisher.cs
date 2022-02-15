// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// What are Events?
/// 
/// NOTE : The subscriber script that is expected to work,
/// must be attached to the GameObject that, the Publisher script is attached.
/// referance: https://www.youtube.com/watch?v=OuZrhykVytg
/// </summary>


// PUBLISHER SCRIPT
public class TestingEventsPublisher : MonoBehaviour
{
    // Version 1
    // public event EventHandler OnSpacePressed; // Event's format generally start with On.    
    //
    // private void Start()
    // {
    //     
    // }
    //
    // private void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.Space))
    //     {
    //         // Space pressed!
    //         // if (OnSpacePressed != null)
    //         // {
    //         //     OnSpacePressed(this,EventArgs.Empty);
    //         // }
    //         
    //         // Short way to do.
    //         OnSpacePressed?.Invoke(this,EventArgs.Empty);
    //
    //     }
    // }
    
    // Version 2
    
    // Event with parameters. (Classical usage)
    public event EventHandler<OnSpacePressedEventArgs> OnSpacePressed;
    public class OnSpacePressedEventArgs : EventArgs
    {
        public int spaceCount;
    }
    
    // Delegate
    public delegate void TestEventDelegate(float f);
    public event TestEventDelegate OnFloatEvent;
    
    // Action
    public event Action<bool, int> OnActionEvent;
    
    // UnityEvent
    public UnityEvent OnUnityEvent;
    
    
    
    private int spaceCount;
    
    private void Start()
    {
        
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
            OnSpacePressed?.Invoke(this,new OnSpacePressedEventArgs{ spaceCount = spaceCount });
            
            OnFloatEvent?.Invoke(5.5f);
            
            OnActionEvent?.Invoke(true,56);
            
            // Must be assigned through inspector.
            OnUnityEvent?.Invoke();
    
        }
    }

    
}
