// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace Mechanics.Shoot.BasicDragAndShoot
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=99yIg-A5eCw&ab_channel=Devsplorer
    /// </summary>
    
    [RequireComponent(typeof(Collider),typeof(Rigidbody))]
    public class BasicDragAndShoot : MonoBehaviour
    {
        [SerializeField] private float _shootForceMultiplier = 3f;
        [SerializeField] private float _forwardSpeed = 5f;
        
        private Vector3 _mousePressDownPosition;
        private Vector3 _mouseReleasePosition;
        private Rigidbody _rigidbody;
        private bool _isShoot;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnMouseDown()
        {
            _mousePressDownPosition = Input.mousePosition;
        }

        private void OnMouseUp()
        {
            _mouseReleasePosition = Input.mousePosition;
            Shoot(_mouseReleasePosition - _mousePressDownPosition );
        }

        private void Shoot(Vector3 force)
        {
            if (_isShoot) return;

            _isShoot = true;
            _rigidbody.AddForce( new Vector3(force.x,force.y,_forwardSpeed) * _shootForceMultiplier);
        }
    }
}
