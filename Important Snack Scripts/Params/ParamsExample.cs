// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace ImportantSnackScripts.Params
{
    /// <summary>
    /// By using the params keyword, you can specify a method parameter that takes a variable number of arguments. The parameter type must be a single-dimensional array.
    /// 
    /// Ref : https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params
    /// </summary>
    public class ParamsExample : MonoBehaviour
    {
        public void Awake()
        {
            UseParams(1,2,3,4);

            int[] myIntArray = new[] {5, 6, 7, 8, 9};
            UseParams(myIntArray);
            
            object[] myObjArray = { 2, 'b', "test", "again" };
            UseParams(myObjArray);
            
            // The following call does not cause an error, but the entire
            // integer array becomes the first element of the params array.
            UseParams(myIntArray);
        }

        public static void UseIntParams(params int[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                Debug.Log($"list[{i}]");
            }
        }
        
        public static void UseParams(params object[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                Debug.Log($"list[{i}]");
            }
        }
        
    }
}
