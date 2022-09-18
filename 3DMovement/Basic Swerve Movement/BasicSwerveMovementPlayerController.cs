// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace _3DMovement.Basic_Swerve_Movement__Right_Left_
{
    /// <summary>
    /// Basic swerve movement
    ///
    /// Attach this script onto player.
    ///  
    /// Ref : https://www.youtube.com/watch?v=GYJMIDGLcdw
    /// </summary>

    // [RequireComponent(typeof(Rigidbody),typeof(Collider))]
    public class BasicSwerveMovementPlayerController : MonoBehaviour
    {
        [SerializeField] private float swerveSpeed=0.01f;
        
        private Vector3 _firstPosition;
        private Vector3 _endPosition;

        private void Update()
        {
            Swerve();    
        }

        private void Swerve()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _firstPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                _endPosition = Input.mousePosition;

                float differenceX = _endPosition.x - _firstPosition.x;
                
                transform.Translate(differenceX * Time.deltaTime * swerveSpeed,0,0);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _firstPosition = Vector3.zero;
                _endPosition = Vector3.zero;
            }
        }
        
        
    }
}
