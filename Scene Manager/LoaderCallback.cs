// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add this script into an empty gameObject in the "Loading" scene.
///
/// Note: Dont forget to add this "Loading" scene to Build Settings > Scenes In Build
/// </summary>

public class LoaderCallback : MonoBehaviour
{

    private bool isFirstUpdate = true;

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
