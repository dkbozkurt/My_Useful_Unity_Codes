// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace StopWatch.BasicStopWatch
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicStopWatch : MonoBehaviour
    {
        private void Start()
        {
            CalculateStopWatchTime();
        }

        private void CalculateStopWatchTime()
        {
            var stopWatch = new Stopwatch();
            
            LongTimeTakerOperation();
            
            stopWatch.Stop();
            Debug.Log("Stop watch ended in : " + stopWatch.ElapsedMilliseconds + " milliseconds.");

        }

        private void LongTimeTakerOperation()
        {
            int a = 0;
            for (int i = 0; i < 100000000; i++)
            {
                 a += i;
            }
            Debug.Log("End value: " + a);
        }
    }
}
