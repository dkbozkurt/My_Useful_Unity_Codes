using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This struct can be used to reference a <b>Material</b> by instance or hash for de/serialization.</summary>
	[System.Serializable]
	public struct P3dHashedMaterial
	{
		[System.NonSerialized]
		private Material instance;

		[SerializeField]
		private int hash;

		public P3dHashedMaterial(Material newInstance, int newHash)
		{
			instance = newInstance;
			hash     = newHash;
		}

		public bool TryGetInstance(out Material model)
		{
			if (instance != null)
			{
				model = instance;

				return true;
			}

			return P3dSerialization.HashToMaterial.TryGetValue(hash, out model);
		}
	}
}