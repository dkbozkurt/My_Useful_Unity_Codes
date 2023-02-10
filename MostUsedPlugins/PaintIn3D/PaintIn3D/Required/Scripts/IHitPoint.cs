using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This interface allows you to make components that can paint 3D points with a specified orientation.</summary>
	public interface IHitPoint : IHit
	{
		void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation);
	}
}