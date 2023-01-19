// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Coroutine.CoroutinesInUnity
{
    /// <summary>
    ///     # Coroutines #
    ///
    /// Coroutines work kind of like a to-do list. It is executed just like Update method except that
    /// unlike update it's able to pause what is's doing one frame and pick up where it left off in
    /// the next. 
    ///
    /// yield : Allow to instruct the coroutine to stop what it's doing until the next frame for a while,
    /// after which the coroutine will continue where it left off.
    ///
    /// yield return null : Suspends execution of a coroutine until the next frame. Yield statement interrups
    /// the function and pauses it until the next frame. Logic is split over a number of frames instead of one
    /// frame. (In update it will happen in one frame)
    ///
    /// yield return new WaitForSecond(floatNumber) : you know...
    ///
    /// yield return new WaitForEndOfFrame() : It makes sure the frame is actually finished before doing something.
    ///
    /// yield return new WaitUnti(MethodName) l : Allow you to use a delegate function in place of a conditional check, after checking
    /// the condition it will operate the following line.
    /// 
    /// yield return new WaitWhile(MethodName)  : Allow you to use a delegate function in place of a conditional check, after checking
    /// the condition it will operate the following line.
    ///
    /// yield return new StartCoroutine(MethodName) : Wait for another coroutine to complete.
    ///
    /// yield break : From inside the coroutine will end it on that line, which can be useful for creating conditions
    /// within the coroutine function that could be used to exit out of it
    ///
    /// StopCoroutine() : Only works if you have a reference to the coroutine that's running and only if that reference has
    /// been set otherwise you ll get a null error.
    ///
    /// : Will stop any and all curry scenes that were triggered by the script that it is used on.
    /// 
    /// 
    /// Ref : https://www.youtube.com/watch?v=kUP6OK36nrM&ab_channel=GameDevBeginner
    /// </summary>
    public class Coroutines : MonoBehaviour
    {
        private int _fuel;
        private bool _stopAfterFive;

        private UnityEngine.Coroutine myTextCoroutine;
        private void Start()
        {
            myTextCoroutine = StartCoroutine(MyTestCoroutine());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                StartCoroutine(TakeScreenShot());
            }
        }

        IEnumerator SimpleCoroutineWithNull(int countLimit)
        {
            int i = 0;

            while ( i < countLimit)
            {
                i++;
                Debug.Log(i);
                yield return null;
            }
            Debug.Log("I'm done counting!");
        }
        
        IEnumerator TakeScreenShot()
        {
            yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshot("screenshot.png");
        }
        
        IEnumerator CheckFuel()
        {
            yield return new WaitUntil(IsEmpty);
            ScreenCapture.CaptureScreenshot("screenshot.png");
        }

        private bool IsEmpty()
        {
            if (_fuel > 0) return false;
            
            return true;
        }

        // ### START
        
        private void StartingACoroutine()
        {
            // Can be done by;
            StartCoroutine("SimpleCoroutineWithNull", 5);
        }
        
        IEnumerator MyTestCoroutine()
        {
            yield return null;
            Debug.Log("My test coroutine works here!");
        }
        
        // ### CANCEL AND STOP

        private void CancelACoroutine()
        {
            StartCoroutine(SimpleCoroutineWithCancel(10));
        }
        
        IEnumerator SimpleCoroutineWithCancel(int countLimit)
        {
            int i = 0;

            while ( i < countLimit)
            {
                i++;
                Debug.Log(i);

                if (i > 5 && _stopAfterFive)
                {
                    yield break;
                }
                yield return new WaitForSeconds(1);
            }
            Debug.Log("I'm done counting!");
        }
        
        private void StopACoroutine()
        {
            StopCoroutine(MyTestCoroutine());
        }
        
        private void StopMultipleCoroutinesOnTheScript()
        {
            StopAllCoroutines();
        }
        
    }
    
}
