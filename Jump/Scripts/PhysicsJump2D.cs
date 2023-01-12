// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Jump.Scripts
{
    [Serializable]
    public enum JumpType2D
    {
        Standard,
        Floaty,
        Curvy,
        Controlled
    }
    
    /// <summary>
    /// 
    /// Ref : https://www.youtube.com/watch?v=c9kxUvCKhwQ&ab_channel=GameDevBeginner
    /// </summary>
    [RequireComponent(typeof(Collider2D),typeof(Rigidbody2D))]
    public class PhysicsJump2D : MonoBehaviour
    {
        private delegate void JumpDelegate();

        public JumpType2D _jumpType;
        
        [Header("JumpForce For Standard and Floaty")]
        [SerializeField] private float _jumpForce = 10f;
        
        [Space]
        [SerializeField] private float _gravityScale = 5f;
        [SerializeField] private float _fallGravityScale = 15f;

        [Header("JumpHeight For Curvy")]
        [SerializeField] private float _jumpHeight = 5f;

        [Header("Properties for Controlled")]
        [SerializeField] private float _buttonPresWindow=2f;
        [SerializeField] private float _cancelRate=5f;
        private float _buttonPressedTime;
        private bool _jumping;
        private bool _jumpCancalled;
        
        private JumpDelegate _jumpDelegate;
        private Rigidbody2D _rigidbody;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            DecideJumpType();
        }

        private void OnDisable()
        {
            _jumpDelegate = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpDelegate();
                _jumping = true;
                _buttonPressedTime = 0;
                _jumpCancalled = false;

            }

            if (_jumpType == JumpType2D.Curvy)
            {
                if (_rigidbody.velocity.y > 0)
                {
                    _rigidbody.gravityScale = _gravityScale;
                }
                else
                {
                    _rigidbody.gravityScale = _fallGravityScale;
                }
            }

            if (_jumpType == JumpType2D.Controlled)
            {
                if (_jumping)
                {
                    _buttonPressedTime += Time.deltaTime;

                    if (_buttonPressedTime < _buttonPresWindow && Input.GetKeyUp(KeyCode.Space))
                    {
                        //_rigidbody.gravityScale = _fallGravityScale;
                        _jumpCancalled = true;
                    }

                    if (_rigidbody.velocity.y < 0)
                    {
                        _rigidbody.gravityScale = _fallGravityScale;
                        _jumping = false;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (_jumpType == JumpType2D.Controlled)
            {
                if (_jumpCancalled && _jumping && _rigidbody.velocity.y > 0)
                {
                    _rigidbody.AddForce(Vector2.down * _cancelRate);
                }
            }
        }

        private void DecideJumpType()
        {
            switch (_jumpType)
            {
                case JumpType2D.Standard:
                    _jumpDelegate += StandardImpulseJump;
                    break;
                
                case JumpType2D.Floaty:
                    _jumpDelegate += FloatyImpulseJump;
                    ImprovePhysicChecks();
                    _rigidbody.gravityScale = _gravityScale;
                    break;
                
                case JumpType2D.Curvy:
                    _jumpDelegate += CurvyJump;
                    ImprovePhysicChecks();
                    break;
                
                case JumpType2D.Controlled:
                    _jumpDelegate += ControlledJump;
                    
                    break;
            }
        }

        private void ImprovePhysicChecks()
        {
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        #region Jump types

        private void StandardImpulseJump()
        {
            // ForceMode.Impulse allows applying the force in one hit.
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        
        private void FloatyImpulseJump()
        {
            StandardImpulseJump();
        }

        private void CurvyJump()
        {
            _rigidbody.gravityScale = _gravityScale;
            float jumpForce = Mathf.Sqrt(_jumpHeight * (Physics.gravity.y * _rigidbody.gravityScale) * -2f) * _rigidbody.mass;
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void ControlledJump()
        {
            CurvyJump();
            _jumping = true;
            _buttonPressedTime = 0f;
        }

        #endregion
        
    }
}
