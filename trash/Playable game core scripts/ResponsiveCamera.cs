// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using JetBrains.Annotations;
using UnityEngine;

public class ResponsiveCamera : MonoBehaviour
{
    [Header("Cameras")] 
    [CanBeNull][SerializeField] private Camera landscapeCamera;
    [CanBeNull][SerializeField] private Camera portraitCamera;
    
    private float screenRatio = (Screen.width / Screen.height);
    
    private void Start()
    {
        CamTypeController();
    }

    private void CamTypeController()
    {
        if (screenRatio >= 1)
        {
            // Landscape Layout

            landscapeCamera.enabled = true;
            landscapeCamera.GetComponent<AudioListener>().enabled = true;
        }
        else
        {
            // Portrait Layout

            portraitCamera.enabled = true;
            portraitCamera.GetComponent<AudioListener>().enabled = true;
        }
        
        Debug.Log("LandScape Camera: " + landscapeCamera.enabled + "\nPortrait Camera: " + portraitCamera.enabled);
    }
    
}
