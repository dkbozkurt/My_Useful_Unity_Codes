// Dogukan Kaan Bozkurt
//      githbub.com/dkbozkurt

using UnityEngine;

namespace Game_Mechanics.BasicCarSimulator.Scripts.Controllers
{
    /// <summary>
    ///
    /// Transform Based Car Movement
    /// 
    /// Ref : https://github.com/13ozkan/CarSimulator
    /// </summary>
    public class CarController : MonoBehaviour
    {
        [Header("Car Properties")] 
        [SerializeField] private float _moveSpeed = 50f;
        [SerializeField] private float _maxSpeed = 15f;
        [SerializeField] private float _drag = 0.98f;
        [SerializeField] private float _steerAngle = 20f;
        [SerializeField] private float _traction = 1f;

        private Vector3 _moveForce;
    
        private void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            _moveForce += transform.forward * _moveSpeed * InputController.Instance.GetVerticalInput() * _steerAngle * Time.deltaTime;
            transform.position += _moveForce * Time.deltaTime;
        }

        private void Rotate()
        {
            float steerInput = InputController.Instance.GetHorizontalInput();
            transform.Rotate(Vector3.up * steerInput * _moveForce.magnitude * _steerAngle * Time.deltaTime);

            _moveForce *= _drag;
            _moveForce = Vector3.ClampMagnitude(_moveForce, _maxSpeed);

            Debug.DrawRay(transform.position, _moveForce.normalized * 3);
            Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);

            _moveForce = Vector3.Lerp(_moveForce.normalized, transform.forward, _traction * Time.deltaTime) * _moveForce.magnitude;
        }
    }
}
