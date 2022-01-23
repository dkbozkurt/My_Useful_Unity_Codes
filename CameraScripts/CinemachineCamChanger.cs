using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// CinemachineVirtualCamera Follow Offset Changer Script.
public class CinemachineCamChanger : MonoBehaviour
{
    private GameObject airplaneFollowerCam;
    private GameObject airplaneWaiterCam;
    private CinemachineVirtualCamera followerCamCinemachine;
    private CinemachineVirtualCamera waiterCamCinemachine;
    
    private int cameraCounter;

    //private Transform airplaneFollowerPoint;
    
    private void Awake()
    {
        airplaneFollowerCam = this.transform.GetChild(2).gameObject;
        followerCamCinemachine = airplaneFollowerCam.gameObject.GetComponent<CinemachineVirtualCamera>();
        cameraCounter = 0;
        

        airplaneWaiterCam = this.transform.GetChild(3).gameObject;
        waiterCamCinemachine = airplaneWaiterCam.gameObject.GetComponent<CinemachineVirtualCamera>();
        
        CameraView();
        //airplaneFollowerPoint = this.transform.GetChild(7).gameObject.transform;

    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (cameraCounter <4 )
                cameraCounter++;
            else
                cameraCounter = 0;
            
            CameraView();

            // var tranposer = followerCamCinemachine.GetCinemachineComponent<CinemachineTransposer>();
            // Debug.Log(tranposer.m_FollowOffset +" transformmmmm");
            
        }
        
    }

    private void CameraView()
    {
        var tranposer = followerCamCinemachine.GetCinemachineComponent<CinemachineTransposer>();

        var transposerW = waiterCamCinemachine.GetCinemachineComponent<CinemachineTransposer>();
        
        switch (cameraCounter)
        {
            case 0: // Default camera Angle
                //tranposer.m_FollowOffset = new Vector3(0.0f, 2.2f, -4.2f);
                tranposer.m_FollowOffset = new Vector3(0, 4.2681f, -9.2848f); //
                transposerW.m_FollowOffset = new Vector3(-0.8743575f,6.434238f,-23.48938f); //
                break;
            case 1:
                tranposer.m_FollowOffset = new Vector3(0.0f, 6.149832f, -0.6257756f);
                break;
            case 2:
                tranposer.m_FollowOffset = new Vector3(13.71006f, 4.964869f, -0.391f);
                break;
            case 3:
                tranposer.m_FollowOffset = new Vector3(0, 3.7514f, 1.5078f);
                break;
            case 4:
                //tranposer.m_FollowOffset = new Vector3(0, 4.2681f, -9.2848f);
                tranposer.m_FollowOffset = new Vector3(0.0f, 2.2f, -4.2f);  //
                transposerW.m_FollowOffset = new Vector3(-0.8743575f,4.144245f,-19.77936f); //
                break;
            default:
                goto case 0 ;

        }
        
        //ChangeLookPoint();

    }

    // private void ChangeLookPoint()
    // {
    //     if (cameraCounter==2)
    //         followerCamCinemachine.Follow = airplaneFollowerPoint;
    //     else
    //         followerCamCinemachine.Follow = defaultFollowPoint;
    // }
}
