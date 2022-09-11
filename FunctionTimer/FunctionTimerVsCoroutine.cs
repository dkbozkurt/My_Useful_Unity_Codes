// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using UnityEngine;

namespace FunctionTimer
{
    /// <summary>
    /// 
    /// Ref : https://youtu.be/KtY_5pDhqYY?t=291 4.51
    /// </summary>

    public class FunctionTimerVsCoroutine : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(CoroutineDelay(3,DoSomethingAfterTime));
        
            FunctionTimerDelay(3,DoSomethingAfterTime);
        }

        private IEnumerator CoroutineDelay(float delayTime,Action action)
        {
            yield return new WaitForSeconds(delayTime);
        
            action?.Invoke();
        }

        private void FunctionTimerDelay(float delayTime,Action action)
        {
            FunctionTimer.Create(()=>
            { 
                action?.Invoke();
            },delayTime);
        }

        private void DoSomethingAfterTime()
        {
            Debug.Log("Time is passed so party time!");
        }
        
    }
}
