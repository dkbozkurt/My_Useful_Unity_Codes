// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace _3DMovement.PhysicsBasedController.Scripts.Controllers
{
    /// <summary>
    /// Physics Based Character Controller with old input system.
    /// 
    /// Ref : https://www.youtube.com/watch?v=1LtePgzeqjQ&ab_channel=PotatoCode
    /// </summary>
    public class PhysicsBasedCharacterController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 4f;
        [SerializeField] private float _sensitivity = 0.1f;
        [SerializeField] private float _maxMoveForce = 1f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private Transform _childGameCamera;

        private Vector2 _keyboardInput;
        private Vector2 _mouseInput;
        private float _lookRotation;
        private bool _isGrounded;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            SetGrounded(true);
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void LateUpdate()
        {
            Rotate();
        }
        
        public void SetGrounded(bool status)
        {
            _isGrounded = status;
        }

        private void Move()
        {
            Vector3 currentVelocity = _rigidbody.velocity;
            Vector3 targetVelocity = new Vector3(GetKeyboardInputs().x, 0, GetKeyboardInputs().y);
            targetVelocity *= _moveSpeed;

            // Align direction
            targetVelocity = transform.TransformDirection(targetVelocity);

            Vector3 velocityChange = targetVelocity - currentVelocity;

            // Falling
            velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);

            // Limit force
            Vector3.ClampMagnitude(velocityChange, _maxMoveForce);
            _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        private void Rotate()
        {
            transform.Rotate(Vector3.up * GetMouseInputs().x * _sensitivity);

            _lookRotation += (-GetMouseInputs().y * _sensitivity);
            _lookRotation = Mathf.Clamp(_lookRotation, -90, 90);
            _childGameCamera.eulerAngles = new Vector3(_lookRotation, _childGameCamera.eulerAngles.y,
                _childGameCamera.eulerAngles.z);
        }

        private void Jump()
        {
            Vector3 jumpForces = Vector3.zero;

            if (!_isGrounded) return;

            jumpForces = Vector3.up * _jumpForce;
            _rigidbody.AddForce(jumpForces, ForceMode.VelocityChange);
        }
        
        private Vector2 GetKeyboardInputs()
        {
            _keyboardInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            return _keyboardInput;
        }

        private Vector3 GetMouseInputs()
        {
            _mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            return _mouseInput;
        }
    }
}