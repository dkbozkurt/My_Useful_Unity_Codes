// Dgoukan Kaan Bozkurt
//       github/dkbozkurt.com

using System;
using UnityEngine;

namespace _3DMovement.CharacterMovementBasics
{
    /// <summary>
    /// Dont forget to set;
    ///     Rigidbody's interpolate property to "Interpolate".
    ///     Freeze all Rotations from Constraints.
    /// 
    /// 
    /// Ref : https://www.youtube.com/watch?v=ELz_EG-s0jU&ab_channel=Tarodev
    /// </summary>
    [RequireComponent(typeof(Collider),typeof(Rigidbody))]
    public class CharacterMovementBasics : MonoBehaviour
    {
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _jumpForce = 300;

        private Rigidbody _rigidbody;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        private void Update() {
            var vel = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * _speed;
            vel.y = _rigidbody.velocity.y;
            _rigidbody.velocity = vel;
 
            if(Input.GetKeyDown(KeyCode.Space)) _rigidbody.AddForce(Vector3.up * _jumpForce);
        }
    }
}