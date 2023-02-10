using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This interface allows you to make components that can paint lines defined by two points.</summary>
	public interface IHitLine : IHit
	{
		void HandleHitLine(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip);
	}
}