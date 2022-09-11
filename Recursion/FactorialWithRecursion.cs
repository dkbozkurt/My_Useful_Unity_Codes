// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace Recursion.Scripts
{
    /// <summary>
    /// Recursion Example
    /// </summary>

    public class FactorialWithRecursion : MonoBehaviour
    {
        [SerializeField]
        private int factorialNumber;
        private void Start()
        {
            Debug.Log($"Answer :{Factorial(factorialNumber)}");
        }

        private int Factorial(int value)
        {
            if (value == 1)
                return 1;
            
            return value * Factorial(value - 1);
            
        }
        
    }
}
