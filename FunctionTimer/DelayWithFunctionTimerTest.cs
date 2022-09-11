// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;

namespace FunctionTimer
{
    /// <summary>
    /// Use this script with FunctionTimer.cs
    /// https://www.youtube.com/watch?v=1hsppNzx7_0
    /// </summary>

    public class DelayWithFunctionTimerTest : MonoBehaviour
    {
        private void Start()
        {
            FunctionTimer.Create(TestingAction, 3f, "Timer");
            FunctionTimer.Create(TestingAction_2, 4f, "Timer_2");
            
            FunctionTimer.StopTimer("Timer");
            
        }

        private void TestingAction()
        {
            Debug.Log("Testing!");
        }
    
        private void TestingAction_2()
        {
            Debug.Log("Testing 2!");
        }
    }
}
