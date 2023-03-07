using System.Threading;
using UnityEngine;

namespace MultiThreading.MultiThreadingBasics
{
    [DisallowMultipleComponent]
    public class ThreadingBasics : MonoBehaviour
    {
        private Thread _thread;
        private int _interval;
        private bool _isActive;
        
        private void OnEnable()
        {
            _thread = new Thread(Work);
            _interval = 3;
            _isActive = true;
            _thread.Start();
        }

        private void OnDisable()
        {
            EndThread();
        }

        private void Work()
        {
            while (_isActive)
            {
                Thread.Sleep(_interval * 1000);
                Debug.Log("This works in a new thread!");
            }
        }
        
        private void EndThread()
        {
            _thread = null;
            _isActive = false;
        }
    }
}