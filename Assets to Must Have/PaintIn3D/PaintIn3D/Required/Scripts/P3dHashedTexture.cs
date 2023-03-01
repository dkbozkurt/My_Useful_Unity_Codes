using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This struct can be used to reference a <b>Texture</b> by instance or hash for de/serialization.
	/// NOTE: For the de/serialization to work you must call the <b>P3dSerialization.TryRegister/TryUnregister</b> methods on your textures.</summary>
	[System.Serializable]
	public struct P3dHashedTexture
	{
		[System.NonSerialized]
		private Texture instance;

		[SerializeField]
		private P3dHash hash;

		public static implicit operator P3dHashedTexture(Texture newInstance)
		{
			P3dHashedTexture hashed;

			hashed.instance = newInstance;

			if (newInstance != null)
			{
				P3dSerialization.TextureToHash.TryGetValue(newInstance, out hashed.hash);
			}
			else
			{
				hashed.hash = 0;
			}

			return hashed;
		}

		public static implicit operator Texture(P3dHashedTexture hashed)
		{
			Texture texture;

			hashed.TryGetInstance(out texture);

			return texture;
		}

		public bool TryGetInstance(out Texture texture)
		{
			if (instance != null)
			{
				texture = instance;

				return true;
			}

			return P3dSerialization.HashToTexture.TryGetValue(hash, out texture);
		}
	}
}