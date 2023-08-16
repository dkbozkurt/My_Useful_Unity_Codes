using System.Collections;
using UnityEngine;

namespace LaserReflection3D.Scripts
{
	[RequireComponent(typeof(Rigidbody))]
	public class LaserGunBullet : MonoBehaviour, IDamageSource
	{
		[SerializeField] private int _numberOfBouncesLimit = 3;
		[SerializeField] private float _damageAmount = 100;

		[SerializeField] private TrailRenderer _trailRenderer;

		private Rigidbody _rigidbody;
		private Vector3 _lastVelocity;

		private float _currentSpeed;
		private Vector3 _direction;
		private int _currentBounces = 0;

		public Rigidbody GetRigidbody => _rigidbody;
		
		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void LateUpdate()
		{
			_lastVelocity = _rigidbody.velocity;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (_currentBounces >= _numberOfBouncesLimit)
			{
				Deactivate();
				return;
			}
			
			Bounce(collision);
		}

		public float DamageAmount()
		{
			return _damageAmount;
		}

		public void Touch()
		{
			Deactivate();
		}

		public void Deactivate()
		{
			StartCoroutine(DeactivateBulletCoroutine());
		}

		private IEnumerator DeactivateBulletCoroutine()
		{
			_currentBounces = 0;
			
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			transform.rotation = Quaternion.Euler(0,0,0);
			
			_trailRenderer.Clear();
			_trailRenderer.emitting = true;
			
			yield return null;

			gameObject.SetActive(false);
		}

		private void Bounce(Collision collision)
		{
			_currentSpeed = _lastVelocity.magnitude;
			_direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);

			_rigidbody.velocity = _direction * Mathf.Max(_currentSpeed, 0);
			
			transform.rotation = Quaternion.LookRotation(_direction,transform.up);
			
			// AudioManager.Instance.PlaySound(AudioName.AmmoBounce);
			_currentBounces++;
		}

		
	}
}
