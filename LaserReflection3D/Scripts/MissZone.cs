using System;
using UnityEngine;

namespace LaserReflection3D.Scripts
{
	public class MissZone : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out LaserGunBullet laserGunBullet))
			{
				laserGunBullet.Deactivate();
			}
		}
	}
}
