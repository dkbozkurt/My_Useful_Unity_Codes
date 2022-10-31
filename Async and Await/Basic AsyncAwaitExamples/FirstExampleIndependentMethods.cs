// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Threading.Tasks;
using UnityEngine;

namespace Async_and_Await.Basic_AsyncAwaitExamples
{
    /// <summary>
    /// Async and Await
    ///
    /// Here we can clearly see Method1 and Method2 are not dependent on each other and are not waiting for
    /// each other and working asynchronous.
    ///
    /// While Method2 and Method3 are dependent on each other and Method3 is waiting for Method2 to completed.
    /// 
    /// Ref : https://www.c-sharpcorner.com/article/async-and-await-in-c-sharp/
    /// </summary>

    public class FirstExampleIndependentMethods : MonoBehaviour
    {
        private void Start()
        {
            Method1();
            Method2();
//            Method3();
        }

        private async Task Method1()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Debug.Log(" Method 1");
                    Task.Delay(100).Wait();
                }
            });
        }

        private void Method2()
        {
            for (int i = 0; i < 100; i++)
            {
                Debug.Log(" Method 2");
                Task.Delay(100).Wait();
            }
        }
        
        private void Method3()
        {
            for (int i = 0; i < 100; i++)
            {
                Debug.Log(" Method 3");
                Task.Delay(100).Wait();
            }
        }
    }
}
