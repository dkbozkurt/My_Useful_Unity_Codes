using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This interface allows you to make components that can paint quads defined by a pair of two points.</summary>
	public interface IHitQuad : IHit
	{
		void HandleHitQuad(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2, Quaternion rotation, bool clip);
	}
}