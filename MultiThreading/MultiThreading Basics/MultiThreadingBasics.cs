// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Threading;
using UnityEngine;

namespace MultiThreading.MultiThreading_Basics
{
    /// <summary>
    /// Ref : https://www.tutorialspoint.com/csharp/csharp_multithreading.htm
    /// </summary>
    public class MultiThreadingBasics : MonoBehaviour
    {
        private void Start()
        {
            //MainThread();
            //CreateThread();
            //ManagingThreads();
            //DestroyingThreads();
        }

        private void MainThread()
        {
            Thread thread = Thread.CurrentThread;
            thread.Name = "MainThread";
            
            Debug.Log($"This is {thread.Name}.");
        }
        
        #region Creating Threads

        private void CreateThread()
        {
            ThreadStart childRef = new ThreadStart(CallToChildThread);
            Debug.Log("In Main: Creating the Child thread.");
            Thread childThread = new Thread(childRef);
            childThread.Start();
        }

        private void CallToChildThread()
        {
            Debug.Log("Child thread starts.");
        }

        #endregion

        #region Managing Threads

        private void ManagingThreads()
        {
            ThreadStart childRef = new ThreadStart(CallToChildThreadForManaging);
            Debug.Log("In Main: Creating the Child thread.");
            Thread childThread = new Thread(childRef);
            childThread.Start();
        }
        
        private void CallToChildThreadForManaging()
        {
            Debug.Log("Child thread starts.");

            int sleepFor = 5000;
            Debug.Log($"Child Thread Paused for {sleepFor /1000} seconds.");
            Thread.Sleep(sleepFor);
            Debug.Log("Child thread resumes.");
        }
        #endregion

        #region Destorying Threads

        private void DestroyingThreads()
        {
            ThreadStart childRef = new ThreadStart(CallToChildThreadForDestroying);
            Debug.Log("In Main: Creating the Child thread.");
            
            Thread childThread = new Thread(childRef);
            childThread.Start();
            
            // stop the main thread for some time
            Thread.Sleep(2000);
            
            // now abort the child.
            Debug.Log("In Main: Aborting the child thread");
            
            childThread.Abort();
        }
        
        private void CallToChildThreadForDestroying()
        {
            try
            {
                Debug.Log("Child thread starts.");

                // do some work, like counting to 10
                for (int i = 0; i <= 10; i++)
                {
                    Thread.Sleep(500);
                    Debug.Log(i);
                }

                Debug.Log("Child thread completed");
            }
            catch (ThreadAbortException e)
            {
                Debug.LogError("Thread Abort Exception");
            }
            finally
            {
                Debug.Log("Couldn't catch the Thread Exception");
            }
        }

        #endregion

    }
}
