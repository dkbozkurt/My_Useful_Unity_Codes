// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Async_and_Await.Tarodev
{
    /// <summary>
    /// Async / Await
    /// 
    /// Ref : https://www.youtube.com/watch?v=WY-mk-ZGAq8&ab_channel=Tarodev
    /// </summary>
    public class AsyncAwaitShape : MonoBehaviour
    {
        
        // Coroutine version
        // public IEnumerator RotateForSeconds(float duration)
        // {
        //     var end = Time.time * duration;
        //     while (Time.time < end)
        //     {
        //         transform.Rotate(new Vector3(1,1)* Time.deltaTime * 150);
        //         yield return null;
        //     }
        //
        // }
        
        // Async version
        
        public async Task RotateForSeconds(float duration)
        {
            var end = Time.time * duration;
            while (Time.time < end)
            {
                transform.Rotate(new Vector3(1,1)* Time.deltaTime * 150);
                await Task.Yield();
            }

        }
    }
}
