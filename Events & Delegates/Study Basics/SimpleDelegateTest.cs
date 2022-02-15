// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// - - - Delegate - - - 
/// 
/// Delegate is a container for a function that can be used as a variable.
/// Delegates are used to perform tasks assigned to different methods with one call.
///
/// Note: Return type of the func. and input parameter types should be same with the delegate type's.
/// 
/// </summary>

public class SimpleDelegateTest : MonoBehaviour
{
    // Assigning delegate
    public delegate void DogukanTestDelegate(int value0);
    
    // Creating object of Delegate
    public DogukanTestDelegate dogukanTestDelegate;

    private void Start()
    {
        Print();
        
    }

    private void SingleCastDelegate()
    {
        // Giving a func to delegate
        dogukanTestDelegate = NumberTest1;

        // Calling the delegate
        dogukanTestDelegate(10);
        
        // Giving a func to delegate
        dogukanTestDelegate = NumberTest2;

        // Calling the delegate
        dogukanTestDelegate(5);
    }

    private void MultiCastDelegate()
    {
        // Unsubscribing to previous functions (otherwise we would see assigned function's results too)
        dogukanTestDelegate -= NumberTest1;
        dogukanTestDelegate -= NumberTest2;
        
        // Subscribing to delegate
        dogukanTestDelegate += NumberTest1;
        
        // Subscribing to delegate
        dogukanTestDelegate += NumberTest2;

        // Calling the delegate
        dogukanTestDelegate(6);
    }

    private void Print()
    {
        Debug.Log("SingleCastDelegate Called:\n");
        SingleCastDelegate();
        Debug.Log("MultiCastDelegate Called:\n");
        MultiCastDelegate();
    }
    
    private void NumberTest1(int value1)
    {
        Debug.Log("Value is: " + value1);
    }

    // Functions can be static.
    public static void NumberTest2(int value2)
    {
        value2 *= value2;
        Debug.Log("Value times value is:" + value2);
    }
}
