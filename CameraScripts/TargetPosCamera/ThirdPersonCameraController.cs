// Doğukan Kaan Bozkurt
//  github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

/// <summary>
/// Script should attached to a new GameObject (example: FreelockCam Controller) in the scene that will be used in the game.
///
/// Assign required components through inspector.
/// 
/// If the game using CinemachineVirtualCamera instead of CinemachineFreelook camera, change the variable type
/// 
/// </summary>

public class ThirdPersonCameraController : MonoBehaviour
{
    #region Cameras
    
    // private CinemachineBrain _cinemachineBrain;
    [SerializeField] private CinemachineFreeLook _creativeCam;
    // [SerializeField] private CinemachineVirtualCamera _creativeCam;
    private CinemachineVirtualCamera[] allGameInCameras;

    #endregion

    #region Multipliers

    [SerializeField]private float sensitivity=0.5f;

    #endregion

    #region Checkers

    private bool camChangedtoCreativeCam;
    private bool disableGameInCameras;

    #endregion
    
    [SerializeField] private Transform _targetPoint;

    private void Start()
    {
        // _cinemachineBrain = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        
        _creativeCam.enabled = false;
        camChangedtoCreativeCam = false;
        disableGameInCameras = false;
        
        CreativeCamSensitivity();
        FreeCamFocusAdjust();

    }

    private void Update()
    {
        
        // To change camera view to free thirdperson camera
        if (Input.GetKeyDown(KeyCode.V))
        {
            camChangedtoCreativeCam = !camChangedtoCreativeCam;
            CamChanger(camChangedtoCreativeCam);
        }
        
        // If camera view is in free mode
        if (camChangedtoCreativeCam)
        {
            Zoom();
        }
        
        
        // To disable all gameincameras
        if (Input.GetKeyDown(KeyCode.B))
        {
            disableGameInCameras = !disableGameInCameras;
            DisableAllGameInCameras(disableGameInCameras);
        }
        
    }

    private void FreeCamFocusAdjust()
    {
        _creativeCam.Follow = _targetPoint;
        _creativeCam.LookAt = _targetPoint;
    }
    
    private void CamChanger(bool state)
    {
        if (state)
        {
            _creativeCam.enabled = true;
        }
        else
        {
            _creativeCam.enabled = false;
        }
        
    }
    
    private void CreativeCamSensitivity()
    {
        _creativeCam.m_XAxis.m_MaxSpeed = _creativeCam.m_XAxis.m_MaxSpeed * sensitivity;
        _creativeCam.m_YAxis.m_MaxSpeed = _creativeCam.m_YAxis.m_MaxSpeed * sensitivity;
    }
    private void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            _creativeCam.m_Lens.FieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 10 * sensitivity;
        }
    }

    private void DisableAllGameInCameras(bool state)
    {
        
        foreach (CinemachineVirtualCamera virtualCameras in allGameInCameras)
        {
            virtualCameras.enabled = state;
        }
    }


}
