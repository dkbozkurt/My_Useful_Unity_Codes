// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// What are Delegates
///
/// Delegates basically variable type that contains functions in it.
///
/// NOTE:  (Operation 6) When using lambda expressions; if we dont
/// have unique reference,  for lambda expression, we cant unsub it,
/// after than we subbed.
/// </summary>
public class TestingDelegates : MonoBehaviour
{
    // Defining delegates
    public delegate void TestDelegate();

    public delegate bool TestBoolDelegate(int value);

    // Creating delegate type parameters.
    private TestDelegate testDelegateFunction;
    private TestBoolDelegate testBoolDelegateFunction;

    // Defining Action
    private Action testAction;
    private Action<int, float> testIntFloatAction;
    
    // Defining Func<x,y,...n> (n is return type of Func
    // (always the last type is return type)
    private Func<bool> testFunc;
    private Func<int, bool> testIntBoolFunc;


    private void Start()
    {
        // FirstOperation();

        // SecondOperation();

        // ThirdOperation();

        // FourthOperation();

        // FifthOperation();

        // SixthOperation();
        
        // SeventhOperation();

        // EighthOperation();
    }

    // Update is called once per frame
    void Update()
    {
    }

    #region Operations

    private void FirstOperation()
    {
        testDelegateFunction += MyTestDelegateFunction;
        testDelegateFunction += MySecondTestDelegateFunction;
        testDelegateFunction();

        testDelegateFunction -= MySecondTestDelegateFunction;
        testDelegateFunction();
    }

    private void SecondOperation()
    {
        testBoolDelegateFunction += MyBoolDelegateFunction;

        Debug.Log(testBoolDelegateFunction(1));
    }

    private void ThirdOperation()
    {
        testDelegateFunction = delegate () { Debug.Log("Anonymous method! "); };
        testDelegateFunction();
    }

    private void FourthOperation()
    {
        testDelegateFunction = () => { Debug.Log("Lambda expression !"); };
        testDelegateFunction();
    }
    
    private void FifthOperation()
    {
        testBoolDelegateFunction = (int i) => { return i < 5; };
        Debug.Log(testBoolDelegateFunction(1));
    }

    private void SixthOperation()
    {
        testDelegateFunction += () => { Debug.Log("Lambda expression"); };
        testDelegateFunction += () => { Debug.Log("Second Lambda expression"); };
        testDelegateFunction();
    }

    private void SeventhOperation()
    {
        testIntFloatAction = (int i, float f) => { Debug.Log("Test int float action!"); };
        testIntFloatAction(1, 2f);
    }

    private void EighthOperation()
    {
        testFunc = () => false;
        Debug.Log(testFunc());

        testIntBoolFunc = (int i) => { return i < 5; };
        Debug.Log(testIntBoolFunc(1));

    }

    #endregion

    
    private void MyTestDelegateFunction()
    {
        Debug.Log("MyTestDelegateFunction is called !");
    }

    private void MySecondTestDelegateFunction()
    {
        Debug.Log("MySecondTestDelegateFunction is called !");
    }

    private bool MyBoolDelegateFunction(int number)
    {
        return number < 5;
    }
}