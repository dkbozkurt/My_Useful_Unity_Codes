using UnityEngine;

namespace LaserReflection3D.Scripts
{
	public interface IDamageSource
	{
		float DamageAmount();

		void Touch();
	}
}
