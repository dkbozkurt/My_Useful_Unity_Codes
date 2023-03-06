using System;
using CpiTemplate.Game.Playable.Scripts.Managers;
using UnityEngine;

namespace CpiTemplate.Game.Playable.Scripts.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovementController : MonoBehaviour
    {
        [Header("Movement Properties")] 
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _turnSmoothTime = 0.05f;
        [SerializeField] private float _slowerMultiplier = 0.06f;

        private CharacterController _characterController;
        private CharacterAnimationController _characterAnimationController;

        private float _inputX;
        private float _inputZ;
        private float _turnSmoothVelocity;

        private bool _isGrounded;

        public bool PlayerIsMoving { get; set; }

        private float _playerInitSpeed;

        private bool _isGodModeEnabled = false;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _characterAnimationController = GetComponent<CharacterAnimationController>();
            _playerInitSpeed = _speed;
        }

        private void OnEnable()
        {
            PlayerInteractionManager.OnPlayerGetCargo += SetPlayerSpeedSlower;
            PlayerInteractionManager.OnHouseObjectPlaced += SetPlayerSpeedNormal;
        }

        private void OnDisable()
        {
            PlayerInteractionManager.OnPlayerGetCargo -= SetPlayerSpeedSlower;
            PlayerInteractionManager.OnHouseObjectPlaced -= SetPlayerSpeedNormal;
        }

        private void SetPlayerSpeedNormal()
        {
            _speed = _playerInitSpeed;
        }

        private void SetPlayerSpeedSlower()
        {
            _speed = _playerInitSpeed * _slowerMultiplier;
        }


        private void Update()
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                GodModeToggle();
            }
        }

        public void SetGrounded(bool status)
        {
            _isGrounded = status;
        }

        private void GodModeToggle()
        {
            _isGodModeEnabled = !_isGodModeEnabled;

            if (_isGodModeEnabled)
            {
                _speed = _speed * 10;
            }
            else
            {
                _speed = _playerInitSpeed;
            }
        }

        private void Move()
        {
            _inputX = JoystickManager.Instance.InputHorizontal();
            _inputZ = JoystickManager.Instance.InputVertical();
            
            var gravityMultiplier = _isGrounded ? 0 : -1;
            Vector3 direction = new Vector3(_inputX, gravityMultiplier, _inputZ).normalized;

            if (_inputX != 0 || _inputZ != 0)
            {
                Rotate(direction);
                _characterController.Move(direction * _speed * Time.deltaTime);
                PlayerIsMoving = true;
                _characterAnimationController.BoolAnimSetter("Move", true);

                return;
            }
            else if (_inputX == 0 && _inputZ == 0 && direction.y == -1)
            {
                _characterController.Move(new Vector3(0, direction.y, 0) * _speed * Time.deltaTime);
            }

            _characterAnimationController.BoolAnimSetter("Move", false);

            PlayerIsMoving = false;
        }

        private void Rotate(Vector3 direction)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Adding smooth turning
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                _turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}