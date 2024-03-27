using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Controllers
{
    public class CameraOrbitController : MonoBehaviour
    {
        public float RotationSpeed = 0.5f;
        public float Deceleration = 15f; 

        private bool _isRotating;
        private Vector3 _lastMousePosition;
        private Vector3 _rotationVelocity;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isRotating = true;
                _lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isRotating = false;
            }

            if (_isRotating)
            {
                Vector3 currentMousePosition = Input.mousePosition;
                float deltaX = currentMousePosition.x - _lastMousePosition.x;
                float deltaY = currentMousePosition.y - _lastMousePosition.y;

                _rotationVelocity.x = deltaX * RotationSpeed;
                _rotationVelocity.y = -deltaY * RotationSpeed;

                transform.RotateAround(Vector3.zero, transform.up, _rotationVelocity.x);
                transform.RotateAround(Vector3.zero, transform.right, _rotationVelocity.y);

                _lastMousePosition = currentMousePosition;
            }
            else
            {
                _rotationVelocity *= 1.0f - Deceleration * Time.deltaTime;

                transform.RotateAround(Vector3.zero, transform.up, _rotationVelocity.x);
                transform.RotateAround(Vector3.zero, transform.right, _rotationVelocity.y);
            }
        }
    }
}