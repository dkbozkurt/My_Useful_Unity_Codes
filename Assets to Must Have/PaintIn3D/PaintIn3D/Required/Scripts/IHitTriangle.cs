using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This interface allows you to make components that can paint triangles defined by three points.</summary>
	public interface IHitTriangle : IHit
	{
		void HandleHitTriangle(bool preview, int priority, float pressure, int seed, Vector3 positionA, Vector3 positionB, Vector3 positionC, Quaternion rotation);
	}
}