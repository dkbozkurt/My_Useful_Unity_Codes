// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Async_and_Await.Tarodev
{
    /// <summary>
    /// Async / Await
    /// 
    /// NOTE : The most powerful thing of the async workflow over a coroutine workflow is async functions can return
    /// data which you cannot do in coroutines.
    ///
    ///  In tasks, Random.Range would return in milisecond !!!
    ///
    /// Define as , var tasks = new Task[shapes.Length]; and then you can access useful
    /// .IsCancelled and .IsCompleted bool values 
    /// 
    /// Ref : https://www.youtube.com/watch?v=WY-mk-ZGAq8&ab_channel=Tarodev
    /// </summary>
    public class AsyncAwaitShapeManager : MonoBehaviour
    {
        [SerializeField] private AsyncAwaitShape[] shapes;

        // Coroutine version
        // public void BeginTest()
        // {
        //     for (int i = 0; i < shapes.Length; i++)
        //     {
        //         StartCoroutine(shapes[i].RotateForSeconds(1 + 1 * i));
        //     }
        // }
        
        // Async, running functions sequentially 
        // public async void BeginTest()
        // {
        //     for (int i = 0; i < shapes.Length; i++)
        //     {
        //         // Will wait until the previous one ended. 
        //         await shapes[i].RotateForSeconds(1 + 1 * i);
        //     }
        // }
        
        //Async method waiting for synchronous tasks to complete
        // public async void BeginTest()
        // {
        //     Debug.Log("Beginned!");
        //
        //     var tasks = new Task[shapes.Length];
        //     // Also can be done by using List
        //     //var tasks = new List<Task>();
        //     for (int i = 0; i < shapes.Length; i++)
        //     {
        //         tasks[i] =  shapes[i].RotateForSeconds(1 + 1 * i);
        //         // Also can be done by using List
        //         // tasks.Add(shapes[i].RotateForSeconds(1 + 1 * i));
        //     }
        //
        //     await Task.WhenAll(tasks);
        //
        //     Debug.Log("All Tasks Finished!");
        // }
        
        // Async method mixing sequential and synchronous tasks
        // public async void BeginTest()
        // {
        //     Debug.Log("Beginned!");
        //
        //     await shapes[0].RotateForSeconds(1 + 1 * 0);
        //
        //     var tasks = new List<Task>();
        //     for (int i = 1; i < shapes.Length; i++)
        //     {
        //         tasks.Add(shapes[i].RotateForSeconds(1 + 1 * i));
        //     }
        //
        //     await Task.WhenAll(tasks);
        //     
        //     Debug.Log("All Tasks Finished!");
        // }
        
        // Async method returning data from an async function
        /*
        public async void BeginTest()
        {
            var randomNumber = await GetRandomNumber();
            print(randomNumber);
        }
        private async Task<int> GetRandomNumber()
        {
            // In tasks, Random.Range would return in milisecond !!!
            var randomNumber = Random.Range(100, 300);
            await System.Threading.Tasks.Task.Delay(randomNumber);
            return randomNumber;
        }
        */
        
        // Calling an async function from a non-async function 
        
        // Non async function
        /*
        public void BeginTest()
        {
            // You can call async function, by adding .GetAwaiter().GetResult(); at the end of the async method you called
            var randomNumber = GetRandomNumber().GetAwaiter().GetResult();
            
            // Another Way is using .Result but dont use it !!! It causese some problems when you use it with try-catch.
            // var randomNumber = GetRandomNumber().Result;

            Debug.Log("Random number: " + randomNumber);
            
        }

        async Task<int> GetRandomNumber()
        {

            var randomNumber = Random.Range(100, 300);
            await System.Threading.Tasks.Task.Delay(randomNumber);
            return randomNumber;

        }
        */
    }
}