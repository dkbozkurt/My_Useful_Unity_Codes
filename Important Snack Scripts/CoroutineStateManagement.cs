// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Coroutine State Management
///
/// Stops the currently active coroutine and starts a new one from 0.
/// 
/// Ref: https://www.youtube.com/watch?v=KRq0-0KY6bU&ab_channel=JasonWeimann
/// </summary>

// Basic Timer Counter
public class CoroutineStateManagement : MonoBehaviour
{
    // Coroutine holds the currently active coroutine.
    private Coroutine _coroutine;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Write a delayed message without a new coroutine");
    }

    private IEnumerator WaitAMinute()
    {
        for (int i = 0; i < 60; i++)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log($"Waited {i} seconds");
        }
        
        Debug.Log("Waited");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // If we dont do that we will lose the reference + missed ref coroutine wont stop again. 
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            
            _coroutine = StartCoroutine(WaitAMinute());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Coroutine Stopped!");
            StopCoroutine(_coroutine);
        }
    }
}
