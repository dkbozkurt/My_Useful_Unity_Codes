// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt


using System;
using UnityEngine;

namespace DebugGizmos.Unity_Gizmos
{
    /// <summary>
    /// 
    /// Ref : https://www.youtube.com/watch?v=4Osn0g_Y24k&ab_channel=Tarodev
    /// </summary>
    public class GrounderByGizmos : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _jumpForce = 10;

        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Vector3 _grounderOffset;
        private readonly Collider[] _groundHits = new Collider[1];

        private bool _isGrounded =>
            Physics.OverlapSphereNonAlloc(transform.position + _grounderOffset, 0.5f, _groundHits, _groundMask) > 0;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _animator.SetTrigger($"Jump");
                _rigidbody.AddForce(Vector3.up * _jumpForce);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + _grounderOffset,0.5f);
        }
    }
}
