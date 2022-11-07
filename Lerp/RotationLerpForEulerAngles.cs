// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace Lerp
{
    /// <summary>
    /// Ref : https://answers.unity.com/questions/643141/how-can-i-lerp-an-objects-rotation.html
    /// </summary>
    public class RotationLerpForEulerAngles : MonoBehaviour
    {
        [SerializeField] private Vector3 _targetAngle;
        [SerializeField] private float _rotationSpeed = 1f;
        private Vector3 _currentAngle;

        void Start()
        {
            _currentAngle = transform.eulerAngles;
        }
        
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RotateByUsingEulerAngles();
            }
        }
        
        private void RotateByUsingEulerAngles()
        {
            _currentAngle = new Vector3(
                Mathf.LerpAngle(_currentAngle.x, _targetAngle.x, Time.deltaTime * _rotationSpeed),
                Mathf.LerpAngle(_currentAngle.y, _targetAngle.y, Time.deltaTime * _rotationSpeed),
                Mathf.LerpAngle(_currentAngle.z, _targetAngle.z, Time.deltaTime * _rotationSpeed));

            transform.eulerAngles = _currentAngle;
        }
    }
}
