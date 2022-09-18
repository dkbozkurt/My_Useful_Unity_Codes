// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace _3DMovement.Camera_Based_Movement
{
    /// <summary>
    /// Attach script onto object that gonna be moved.
    ///
    /// Use with "CamMouseRotation.cs" to rotate camera with mouse movement.
    /// 
    /// Ref : https://www.youtube.com/watch?v=7kGCrq1cJew
    /// </summary>
    
    public class CameraBasedMovement : MonoBehaviour
    {
        private float _playerVerticalInput;
        private float _playerHorizontalInput;

        private Vector3 _forwardRelativeVerticalInput;
        private Vector3 _rightRelativeVerticalInput;
        private Vector3 cameraRelativeMovement;

        private Camera _mainCam;

        private void Awake()
        {
            _mainCam= Camera.main;
        }

        private void Update()
        {
            MovePlayerRelativeToCamera();
        }

        private void MovePlayerRelativeToCamera()
        {
            // Get Player Input
            _playerVerticalInput = Input.GetAxis("Vertical");
            _playerHorizontalInput = Input.GetAxis("Horizontal");
            
            // Get Camera Normalized Directional Vectors
            Vector3 forward = _mainCam.transform.forward;
            Vector3 right = _mainCam.transform.right;

            forward.y = 0;
            right.y = 0;
            forward = forward.normalized;
            right = right.normalized;
            
            //Create direction-relative-input vectors
            _forwardRelativeVerticalInput = _playerVerticalInput * forward;
            _rightRelativeVerticalInput = _playerHorizontalInput * right;
            
            // Create and apply camera relative movement
            cameraRelativeMovement = _forwardRelativeVerticalInput + _rightRelativeVerticalInput;
            this.transform.Translate(cameraRelativeMovement,Space.World);

        }
    }
}
