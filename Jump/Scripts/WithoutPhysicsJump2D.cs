// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Jump.Scripts
{
    [Serializable]
    public enum JumpTypeForNonPhysics
    {
        Standard,
    }
    
    /// <summary>
    /// 
    /// Ref : https://www.youtube.com/watch?v=c9kxUvCKhwQ&ab_channel=GameDevBeginner
    /// </summary>
    public class WithoutPhysicsJump2D : MonoBehaviour
    {
        private delegate void JumpDelegate();

        public JumpTypeForNonPhysics _jumpType;
        
        [SerializeField] private float _jumpHeight = 0f;
        [SerializeField] private float _gravityScale = 5f;

        [Space] 
        [SerializeField] private float _floorHeight = 0.5f;
        [SerializeField] private Transform _feet;
        [SerializeField] private ContactFilter2D _filter;
        private bool _isGrounded;
        private Collider2D[] _results = new Collider2D[1];
        
        private JumpDelegate _jumpDelegate;
        private float _velocity;

        void Awake()
        {
            DecideJumpType();
        }

        private void OnDisable()
        {
            _jumpDelegate = null;
        }

        void Update()
        {
            ApplyAdjustableGravity();

            if (Physics2D.OverlapBox(_feet.position, _feet.localScale, 0, _filter, _results) > 0
                && _velocity < 0)
            {
                _velocity = 0;
                Vector2 surface = Physics2D.ClosestPoint(transform.position, _results[0]) + Vector2.up * _floorHeight;
                transform.position = new Vector3(transform.position.x, surface.y,transform.position.z);
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _jumpDelegate();
            }
            
            ExecuteJump();
        }

        private void DecideJumpType()
        {
            switch (_jumpType)
            {
                case JumpTypeForNonPhysics.Standard:
                    _jumpDelegate += StandardImpulseJump;
                    break;
            }
        }

        private void ApplyAdjustableGravity()
        {
            _velocity += Physics2D.gravity.y * _gravityScale * Time.deltaTime;
        }

        private void ExecuteJump()
        {
            transform.Translate(new Vector3(0,_velocity,0)* Time.deltaTime);
        }

        #region Jump types

        private void StandardImpulseJump()
        {
            _velocity = Mathf.Sqrt(_jumpHeight * -2 * (Physics2D.gravity.y * _gravityScale));
        }
        #endregion
    }
}
