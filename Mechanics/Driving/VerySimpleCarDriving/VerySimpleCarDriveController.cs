// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace Mechanics.Driving.VerySimpleCarDriving
{
    
    /// <summary>
    /// Simple Car Driving
    /// Ref : https://docs.unity3d.com/ScriptReference/Input.GetAxis.html#:~:text=To%20set%20up%20your%20input,list%20of%20your%20current%20inputs.
    /// </summary>
    public class VerySimpleCarDriveController : MonoBehaviour
    {
        [Header("Car Properties")]
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _rotationSpeed = 100.0f;

        private void Update()
        {
            Drive();
        }

        private void Drive()
        {
            float translation = Input.GetAxis("Vertical") * _speed;
            float rotation = Input.GetAxis("Horizontal") * _rotationSpeed;
            
            // Make it move 10 meters per second instead of 10 meters per frame
            translation *= Time.deltaTime;
            rotation *= Time.deltaTime;
            
            // Move translation along the object's z-axis
            transform.Translate(0,0,translation);
            
            // Rotate around our y-axis
            transform.Rotate(0,rotation,0);
        }
    }
}
