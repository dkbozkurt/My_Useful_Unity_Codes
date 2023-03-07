// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;
using Random = System.Random;

namespace ImportantSnackScripts.Scripts
{
    public enum RandomEnum
    {
        Random1,
        Random2,
        Random3,
        Random4
    }
    
    public class GetRandomEnumElement : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Enum value : " + GetRandomEnum());    
            }
        }

        private RandomEnum GetRandomEnum()
        {
            Array values = Enum.GetValues(typeof(RandomEnum));
            Random random = new Random();
            RandomEnum randomEnum = (RandomEnum) values.GetValue(random.Next(values.Length));
            return randomEnum;
        }
    }
}
