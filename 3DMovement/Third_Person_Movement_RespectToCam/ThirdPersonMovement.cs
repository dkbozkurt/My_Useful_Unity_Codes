// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace _3DMovement.ThirdPersonMovement
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=4HpC--2iowE&ab_channel=Brackeys
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float turnSmoothTime = 0.1f;
        
        private CharacterController _characterController;
        private float _turnSmoothVelocity;
        private Transform _camTransform;         
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _camTransform = Camera.main.transform;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

            if (direction.magnitude >= 0.1f)
            {
                Rotate(direction,out Vector3 moveDir);
                
                _characterController.Move(moveDir.normalized * _speed * Time.deltaTime);
            }

        }

        private void Rotate(Vector3 direction,out Vector3 moveDirection)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camTransform.eulerAngles.y;
        
            // Adding smooth turning
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f,angle,0f);
            
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
    }
}
