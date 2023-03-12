// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;

namespace Game_Mechanics.BasicFlightSimulator.Scripts
{
    /// <summary>
    /// Ref : https://github.com/13ozkan/FlightSimulator/blob/main/Assets/Scripts/AirplaneController.cs
    /// </summary>

    public class FlightSimulatorController : MonoBehaviour
    {
        [SerializeField] private float _flySpeed = 5f;

        [SerializeField] private float _yawAmount = 120f;

        private float _yaw;

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.position += transform.forward * _flySpeed * Time.deltaTime;

            float hortizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            _yaw += hortizontalInput * _yawAmount * Time.deltaTime;
            float pitch = Mathf.Lerp(0, 20, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
            float roll = Mathf.Lerp(0, 30, Mathf.Abs(hortizontalInput)) * -Mathf.Sign(hortizontalInput);

            transform.localRotation = Quaternion.Euler(Vector3.up * _yaw);
            transform.localRotation = Quaternion.Euler(Vector3.up * _yaw + Vector3.right * pitch);
        }
    }
}
