// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// What are Events?
/// 
/// /// NOTE : The subscriber script that is expected to work,
/// must be attached to the GameObject that, the Publisher script is attached.
/// </summary>


// SUBSCRIBER SCRIPT
public class TestingEventSubscriber : MonoBehaviour
{
    // Version 1
    // void Start()
    // {
    //     TestingEvents testingEvents = GetComponent<TestingEvents>();
    //     testingEvents.OnSpacePressed += TestingEvents_OnSpacePressed;
    // }
    //
    // private void TestingEvents_OnSpacePressed(object sender, EventArgs e)
    // {
    //     Debug.Log("Space by subscriber");
    //     TestingEvents testingEvents = GetComponent<TestingEvents>();
    //     testingEvents.OnSpacePressed -= TestingEvents_OnSpacePressed;
    // }
    
    // Version 2
    void Start()
    {
        TestingEvents testingEvents = GetComponent<TestingEvents>();
        testingEvents.OnSpacePressed += TestingEvents_OnSpacePressed;

        testingEvents.OnFloatEvent += TestingEvents_OnFloatEvent;

        testingEvents.OnActionEvent += TestingEvents_OnActionEvent;
        
    }
    
    private void TestingEvents_OnSpacePressed(object sender, TestingEvents.OnSpacePressedEventArgs e)
    {
        Debug.Log("Space by subscriber" + e.spaceCount);
        TestingEvents testingEvents = GetComponent<TestingEvents>();
        testingEvents.OnSpacePressed -= TestingEvents_OnSpacePressed;
    }

    private void TestingEvents_OnFloatEvent(float f)
    {
        Debug.Log("Flott: "+ f);
    }

    private void TestingEvents_OnActionEvent(bool arg1, int arg2)
    {
        Debug.Log("Arg1: "+ arg1 +  " Arg2: "+ arg2);
    }

    public void TestingUnityEvent()
    {
        Debug.Log("TestingUnityEvent");
    }
}
