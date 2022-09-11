// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;

namespace Recursion.Scripts
{
    /// <summary>
    /// Recursion
    /// 
    /// Ref : https://www.youtube.com/watch?v=x8WJruoR6hA
    /// </summary>

    public class LogicOfRecursion : MonoBehaviour
    {
        private void Start()
        {
            f(3);
        }

        private void f(int n)
        {
            if(n>1)
                f(n-1);
            Debug.Log("n: " + n);
        }
    }
}
