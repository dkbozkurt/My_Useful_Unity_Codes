using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This interface allows you to make components that can paint points defined by UV coordinates.
	/// NOTE: The <b>rotation</b> argument is in world space, where <b>Quaternion.identity</b> means the paint faces forward on the +Z axis, and up is +Y.</summary>
	public interface IHitCoord : IHit
	{
		void HandleHitCoord(bool preview, int priority, float pressure, int seed, P3dHit hit, Quaternion rotation);
	}
}