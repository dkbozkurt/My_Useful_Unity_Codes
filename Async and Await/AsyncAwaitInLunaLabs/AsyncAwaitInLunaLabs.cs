// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Async_and_Await.AsyncAwaitInLunaLabs
{
    /// <summary>
    /// 
    /// Ref : https://docs.lunalabs.io/docs/playable/code/unity-cs-features/async-await
    /// </summary>
    public class AsyncAwaitInLunaLabs : MonoBehaviour
    {
        private void Start()
        {
            // await DoWorkParallel();

            // await DoWorkSequential();

            // Debug.Log(await TickTockTracker());

            // await CustomAwaiter();

            StartCoroutine(RunTask());
        }

        #region Parallel calls

        private async Task DoWorkParallel()
        {
            Debug.Log("Parallel calls");
            TickParallel();
            TockParallel();
            Debug.Log("Parallel calls are completed!");
        }

        // Task method will work but not if you are not about to wait for it keep it void
        private async Task TickParallel()
        {
            Debug.Log("Parallel Tick Waiting 0.5f seconds ...");
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
            Debug.Log("Parallel Tick Done!");
        }

        // Void as return value
        private async void TockParallel()
        {
            Debug.Log("Parallel Tock Waiting 0.5f seconds ...");
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
            Debug.Log("Parallel Tock Done!");
        }

        #endregion

        #region Sequential Calls

        // Calling method have to be marked as async

        private async Task DoWorkSequential()
        {
            Debug.Log("Sequential calls");
            await TickSequential();
            await TockSequential();
            Debug.Log("Sequential calls are completed!");
        }

        // void method
        private async Task TickSequential()
        {
            Debug.Log("Sequential Tick Waiting 1 second ...");
            await Task.Delay(TimeSpan.FromSeconds(1));
            Debug.Log("Sequential Tick Done!");
        }

        // In order to wait for result return value should be Task
        private async Task TockSequential()
        {
            Debug.Log("Sequential Tock Waiting 1 second ...");
            await Task.Delay(TimeSpan.FromSeconds(1));
            Debug.Log("Sequential Tock Done!");
        }

        #endregion

        #region Getting data from task (using Task<T>)

        // To use it Debug.Log(await TickTockTracker());

        private async Task<string> TickTockTracker()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            return "Waited for 2 seconds and return result sample";
        }

        #endregion

        #region Customer Awaiter

        // //call for it
        // await  CustomAwaiter();
        // //extenstion for TimeSpan
        // public static class AwaitExtensions
        // {
        //     public static TaskAwaiter GetAwaiter( this TimeSpan timeSpan )
        //     {
        //         return Task.Delay( timeSpan ).GetAwaiter();
        //     }
        // }
        #endregion

        #region Calling from Coroutine

        private IEnumerator RunTask()
        {
            yield return RunTaskAsync().AsIEnumerator();
        }

        private async Task RunTaskAsync()
        {
            Debug.Log("RunTaskAsync Start");
            await Task.Delay(TimeSpan.FromSeconds(1));
            Debug.Log("RunTaskAsync Finish");
        }

        #endregion
        
        #region Example for running method in a new thread

        private bool isRunning;
        
        private async void Awake()
        {
            // Runs the method in the main thread
            await Loop();
            
            // Runs the method in a new thread
            await Task.Run(Loop);
        }
        
        private async Task Loop()
        {
            while (isRunning)
            {
                await Task.Delay(3000);
            }
        }

        #endregion
    }
    
    public static class AwaitExtensions
    {
        
        public static IEnumerator AsIEnumerator(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (!task.IsFaulted) yield break;
            if (task.Exception != null) throw task.Exception;
        }
    }
}