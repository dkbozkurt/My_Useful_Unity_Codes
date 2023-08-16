using UnityEngine;

namespace LaserReflection3D.Scripts
{
	public class CharacterBase_LaserReflect : MonoBehaviour
	{
		[SerializeField] protected Collider _characterCollider;
		[SerializeField] protected Animator _characterAnimator;
		[SerializeField] private float _health = 100;

		private bool _canReceiveDamage = true;
		private float _currentHealth;

		private void Awake()
		{
			_currentHealth = _health;
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if(!_canReceiveDamage ) return;
			
			if (other.TryGetComponent(out IDamageSource damageSource))
			{
				ReceiveDamage(damageSource.DamageAmount());
				damageSource.Touch();
			}
		}

		private void ReceiveDamage(float damageAmount)
		{
			_currentHealth -= damageAmount;

			if (_currentHealth <= 0)
			{
				Die();
			}
		}

		protected virtual void Die()
		{
			_characterCollider.enabled = false;
			SetCharacterAnimation("Die",true);
		}
		
		protected void SetCharacterAnimation(string animName,bool status, string animNameToSetFalse = "")
		{
			_characterAnimator.SetBool(animName,status);

			if (animNameToSetFalse != "")
			{
				_characterAnimator.SetBool(animNameToSetFalse,false);
			}
		}
	}
}
