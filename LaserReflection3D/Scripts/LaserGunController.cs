using System;
using DESIGN_PATTERNS.Object_Pool_Pattern;
using UnityEngine;
using UnityEngine.Serialization;

namespace LaserReflection3D.Scripts
{
	public class LaserGunController : MonoBehaviour
	{
		public static event Action OnFire; 
		public static event Action OnMouseReleased;
		
		[SerializeField] private LaserReflectionBehaviour _laserReflectionBehaviour;
		[SerializeField] private Transform _firePoint;
		[SerializeField] private float _fireForce = 300f;
		[SerializeField] private float _fireRate = 1f;
		[Space]
		[SerializeField] private Transform _objectBaseToRotate;
		[SerializeField] private float _rotationSpeed = 50f;
		[SerializeField] private float _sensitivity = 10.0f;
		[SerializeField] private Vector2 _rotationLimit = new Vector2(-90, 90);
		
		private float _time;
		private float _nextTimeFire;
		private bool _haveAmmo = true;
		private bool _canFire = true;
		
		//
		private bool _isAimControllable;
		private bool _isMousePressed;
		private float _currentRotation = 0.0f;
		private float _targetRotation = 0.0f;
		
		private void Start()
		{
			SetLaser(true);
			SetHaveAmmo(true);
			SetAimControllable(true);
		}

		private void Update()
		{
			if (!_canFire)
			{
				WaitForFire();
			}

			if (_isAimControllable)
			{
				if (Input.GetMouseButtonDown(0))
				{
					_isMousePressed = true;
				}
				else if (Input.GetMouseButtonUp(0))
				{
					_isMousePressed = false;
					Fire();
					OnMouseReleased?.Invoke();
				}

				if (_isMousePressed)
				{
					CalculateAim();
				}
			}
		}

		private void WaitForFire()
		{
			_time += Time.deltaTime;
			_nextTimeFire = 1 / _fireRate;

			if (_time >= _nextTimeFire)
			{
				_canFire = true;
				_time = 0f;
			}
		}

		public void SetLaser(bool status)
		{
			_laserReflectionBehaviour.gameObject.SetActive(status);	
		}
		
		private void SetHaveAmmo(bool status)
		{
			_haveAmmo = status;
		}
		
		private void CalculateAim()
		{
			float mouseX = Input.GetAxis("Mouse X");

			float angle = mouseX * _sensitivity;

			_targetRotation += angle;

			_targetRotation = Mathf.Clamp(_targetRotation, _rotationLimit.x, _rotationLimit.y);

			Quaternion targetRotationQuaternion = Quaternion.Euler(0, _targetRotation, 0);
			_objectBaseToRotate.rotation = Quaternion.Lerp(_objectBaseToRotate.rotation, targetRotationQuaternion, _rotationSpeed * Time.deltaTime);
		}

		private void SetAimControllable(bool status)
		{
			_isAimControllable = status;
		}
		
		private void Fire()
		{
			if(!_canFire || !_haveAmmo) return;
			
			Debug.Log("Fired");
			
			// LaserGunBullet bullet = 
			// 	ObjectPoolManager.Instance.GetPooledObject(ObjectName.LaserGunBullet,_firePoint.transform.position,Quaternion.identity)
			// 		.GetComponent<LaserGunBullet>();
			//
			// bullet.transform.rotation = Quaternion.LookRotation(_objectBaseToRotate.transform.forward, transform.up);
			// bullet.GetRigidbody.AddForce(_firePoint.forward * _fireForce);
			
			// AudioManager.Instance.PlaySound(AudioName.Fire);
			OnFire?.Invoke();
			_canFire = false;
		}
	}
}
