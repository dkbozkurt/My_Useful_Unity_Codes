using System;

namespace LaserReflection3D.Scripts
{
	public class EnemyController_LaserReflect : CharacterBase_LaserReflect
	{
		public static event Action OnEnemyDie;
		
		protected override void Die()
		{
			base.Die();
			OnEnemyDie?.Invoke();
		}
	}
}
