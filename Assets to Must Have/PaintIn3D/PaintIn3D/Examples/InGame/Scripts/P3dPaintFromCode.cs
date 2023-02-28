using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This component shows you how to paint from code.</summary>
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Paint From Code")]
	public class P3dPaintFromCode : MonoBehaviour
	{
		// The decal settings we want to use (this can be a prefab).
		public P3dPaintDecal MyDecal;

		protected virtual void Update()
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var hit = default(RaycastHit);

			if (Physics.Raycast(ray, out hit))
			{
				var preview  = !Input.GetKey(KeyCode.Mouse0);
				var priority = 0; // If you're painting multiple times per frame, or using 'live painting', then this can be used to sort the paint draw order. This should normally be set to 0.
				var pressure = 1.0f; // If you're using modifiers that use paint pressure (e.g. from a finger), then you can set it here. This should normally be set to 1.
				var seed     = 0; // If this paint uses modifiers that aren't marked as 'Unique', then this seed will be used. This should normally be set to 0.
				var rotation = Quaternion.LookRotation(-hit.normal); // Get the rotation of the paint. This should point TOWARD the surface we want to paint, so we use the inverse normal.

				MyDecal.HandleHitPoint(preview, priority, pressure, seed, hit.point, rotation);
			}
		}
	}
}