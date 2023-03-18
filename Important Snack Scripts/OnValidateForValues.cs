// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace ImportantSnackScripts
{
    /// <summary>
    /// OnValidate() function will be called when a value has updated from inspector
    /// either in edit and game mode.
    /// </summary>

    public class OnValidateForValues : MonoBehaviour
    {

        public float MyValue = 5f;

        private void OnValidate()
        {
            Debug.Log(MyValue);
        }
    }
}
