using System;

namespace LaserReflection3D.Scripts
{
	public class PlayerController_LaserReflect : CharacterBase_LaserReflect
	{
		public static event Action OnPlayerDie;
		
		protected override void Die()
		{
			base.Die();
			OnPlayerDie?.Invoke();
		}
	}
}
