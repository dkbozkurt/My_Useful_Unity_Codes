// Uncomment this line if you need to store the world position of the hit
//#define STORE_POSITION

using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This stores information about a scene point on a mesh. This is usually generated from a <b>RaycastHit</b>, but it can also be filled manually.</summary>
	public struct P3dHit
	{
		public P3dHit(RaycastHit hit)
		{
			Root   = hit.collider.transform;
			First  = hit.textureCoord;
			Second = hit.textureCoord2;
#if STORE_POSITION
			Position = hit.point;
#endif
		}

		/// <summary>The <b>Transform</b> that was hit.</summary>
		public Transform Root;

		/// <summary>The first UV coord that was hit.</summary>
		public Vector2 First;

		/// <summary>The second UV coord that was hit.</summary>
		public Vector2 Second;

#if STORE_POSITION
		/// <summary>The world position that was hit.</summary>
		public Vector3 Position;
#endif
	}
}