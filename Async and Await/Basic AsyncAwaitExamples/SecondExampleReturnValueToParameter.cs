// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Threading.Tasks;
using UnityEngine;

namespace Async_and_Await.Basic_AsyncAwaitExamples
{
    /// <summary>
    ///
    /// Method1 is returning a value and we are passing a parameter in the Method3.
    /// We have to use await keyword before passing a parameter in Method3 and for it,
    /// we have to use the async keyword from the calling method.
    ///
    /// Method3 requires one parameter, which is the return type of Method1. Here,
    /// await keyword is playing a vital role for waiting of Method1 task completion.
    /// 
    /// Ref : https://www.c-sharpcorner.com/article/async-and-await-in-c-sharp/
    /// </summary>

    
    public class SecondExampleReturnValueToParameter : MonoBehaviour
    {
        #region Code sample for C# 7

        // private void Awake()
        // {
        //     CallMethod();
        // }
        //
        // private async void CallMethod()
        // {
        //     Task<int> task = Method1();
        //     Method2();
        //     int count = await task;
        //     Method3(count);
        // }
        //
        // private async Task<int> Method1()
        // {
        //     int count = 0;
        //     await Task.Run(() =>
        //     {
        //         for (int i = 0; i < 100; i++)
        //         {
        //             Debug.Log("Method 1");
        //             count += 1;
        //         }
        //     });
        //     return count;
        // }
        //
        // private void Method2()
        // {
        //     for (int i = 0; i < 25; i++)
        //     {
        //         Debug.Log("Method 2");
        //     }
        // }
        //
        // private void Method3(int count)
        // {
        //     Debug.Log("Total count is " + count);
        // }

        #endregion

        #region Code sample for C# 9

        private void Awake()
        {
            Main();
            
        }
        private async Task Main()
        {
            await CallMethod();
        }

        private async Task CallMethod()
        {
            Method2();
            int count = await Method1();
            Method3(count);
        }

        private async Task<int> Method1()
        {
            int count = 0;
            await Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Debug.Log("Method 1");
                    count += 1;
                }
            });
            return count;
        }
        
        private void Method2()
        {
            for (int i = 0; i < 25; i++)
            {
                Debug.Log("Method 2");
            }
        }
        
        private void Method3(int count)
        {
            Debug.Log("Total count is " + count);
        }

        #endregion
        
    }
}