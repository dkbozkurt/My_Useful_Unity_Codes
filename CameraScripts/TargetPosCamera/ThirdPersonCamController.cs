// Doğukan Kaan Bozkurt
//  github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Attach this script to FreeLookCamera Component in the scene, use this script with "ThirdPersonCameraController" script's attached GameObject.
/// 
/// </summary>
 
[RequireComponent(typeof(CinemachineFreeLook))]
public class ThirdPersonCamController : MonoBehaviour
{
    private bool _freeLookActive;

    void Start()
    {
        CinemachineCore.GetInputAxis = GetInputAxis;
    }

    void Update()
    {
        _freeLookActive = Input.GetMouseButton(1);
    }

    private float GetInputAxis(string axisName)
    {
        return !_freeLookActive ? 0 : Input.GetAxis(axisName == "Mouse Y" ? "Mouse Y" : "Mouse X");
    }
}