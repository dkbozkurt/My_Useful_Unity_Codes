//  Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace DKBozkurt.Utils
{
    public static partial class DKBozkurtUtils
    {
        public static class Events
        {
            public static readonly Evt<int> OnScoreUpdated = new Evt<int>();
            
            public static readonly Evt OnGameOver = new Evt();
            public static readonly Evt OnGameStart = new Evt();
        }

        public class Evt<T>
        {
            private event Action<T> _action = delegate { };
            
            public void Invoke(T param) {_action.Invoke(param);}
            public void AddListener(Action<T> listener)
            {
                _action -= listener;
                _action += listener;
            }
            public void RemoveListener(Action<T> listener) { _action -= listener; }
        }
        
        public class Evt
        {
            private event Action _action = delegate { };

            public void Invoke() { _action.Invoke(); }
            public void AddListener(Action listener) { // Safety check
                _action -= listener;
                _action += listener;
            }
            public void RemoveListener(Action listener) { _action -= listener; }
        }
    }
}
