// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Jump.Scripts
{
    [Serializable]
    public enum JumpType
    {
        Standard,
        Floaty,
    }
    
    /// <summary>
    /// 
    /// Ref : https://www.youtube.com/watch?v=c9kxUvCKhwQ&ab_channel=GameDevBeginner
    /// </summary>
    [RequireComponent(typeof(Collider),typeof(Rigidbody))]
    public class PhysicsJump : MonoBehaviour
    {
        private delegate void JumpDelegate();

        public JumpType _jumpType;
        
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] [Range(-100f,0f)]private float _updatedGravityForce = -25f;
        
        private JumpDelegate _jumpDelegate;
        private Rigidbody _rigidbody;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            DecideJumpType();
        }

        private void OnDisable()
        {
            _jumpDelegate = null;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpDelegate();
            }
        }

        private void DecideJumpType()
        {
            switch (_jumpType)
            {
                case JumpType.Standard:
                    _jumpDelegate += StandardImpulseJump;
                    break;
                
                case JumpType.Floaty:
                    _jumpDelegate += FloatyImpulseJump;
                    Physics.gravity = new Vector2(0,_updatedGravityForce);
                    // While working in 2D you can change 2D rigidbody's gravity scale instead of in game gravity force !!!
                    // _rigidbody2D.gravityScale = _updatedGravityForce;
                    ImprovePhysicChecks();
                    break;
            }
        }

        private void ImprovePhysicChecks()
        {
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        #region Jump types

        private void StandardImpulseJump()
        {
            // ForceMode.Impulse allows applying the force in one hit.
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        // This method will cause applying gravity force change all the object in the game, consider while using!
        private void FloatyImpulseJump()
        {
            StandardImpulseJump();
        }
        
        #endregion
    }
}
