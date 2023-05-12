using Game.Scripts.Managers;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Properties")]
        [SerializeField] private float _speed = 25f;
        // [SerializeField] private float _turnSmoothTime = 0.05f;

        private Rigidbody2D _rigidbody;

        private float _inputX;
        private float _inputY;
        private float _turnSmoothVelocity;

        public bool IsPlayerMoving { get; set; }

        private bool _isAlreadyLookingRight = true;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            _inputX = JoystickManager.Instance.InputHorizontal();
            _inputY = JoystickManager.Instance.InputVertical();

            Vector2 direction = new Vector2(_inputX, _inputY).normalized;

            if (direction.magnitude >= 0.1f)
            {
                Rotate(_inputX >= 0);
                _rigidbody.velocity = direction * _speed * Time.fixedDeltaTime;
                IsPlayerMoving = true;
                return;
            }

            // AnimationBoolSetter("Move",false);
            _rigidbody.velocity = Vector2.zero;
            IsPlayerMoving = false;
        }

        private void Rotate(bool lookingAtRight)
        {
            if(lookingAtRight == _isAlreadyLookingRight) return;

            _isAlreadyLookingRight = lookingAtRight;

            var targetAngle = lookingAtRight ? 0 : -180f; 
            
            transform.rotation= Quaternion.Euler(0f,-targetAngle,0f);
        }

        // private void Rotate(Vector3 direction)
        // {
        //     float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //
        //     // Adding smooth turning
        //     float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
        //         _turnSmoothTime);
        //
        //     transform.rotation = Quaternion.Euler(0f, angle, 0f);
        // }
    }
}