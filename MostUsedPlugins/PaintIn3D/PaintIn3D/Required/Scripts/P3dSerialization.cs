using UnityEngine;
using System.Collections.Generic;

namespace PaintIn3D
{
	/// <summary>This class handles the low level de/serialization of different paint in 3D objects to allow for things like networking.</summary>
	public static class P3dSerialization
	{
		/// <summary>This stores an association between a <b>Material</b> hash code and the <b>Material</b> instance, so it can be de/serialized.</summary>
		public static Dictionary<int, Material> HashToMaterial = new Dictionary<int, Material>();

		/// <summary>This stores an association between a <b>Material</b> instance and the <b>Material</b> hash code, so it can be de/serialized.</summary>
		public static Dictionary<Material, int> MaterialToHash = new Dictionary<Material, int>();

		/// <summary>This stores an association between a <b>P3dModel</b> hash code and the <b>P3dModel</b> instance, so it can be de/serialized.</summary>
		public static Dictionary<P3dHash, P3dModel> HashToModel = new Dictionary<P3dHash, P3dModel>();

		/// <summary>This stores an association between a <b>P3dModel</b> instance and the <b>P3dModel</b> hash code, so it can be de/serialized.</summary>
		public static Dictionary<P3dModel, P3dHash> ModelToHash = new Dictionary<P3dModel, P3dHash>();

		/// <summary>This stores an association between a <b>Texture</b> hash code and the <b>Texture</b> instance, so it can be de/serialized.</summary>
		public static Dictionary<P3dHash, Texture> HashToTexture = new Dictionary<P3dHash, Texture>();

		/// <summary>This stores an association between a <b>Texture</b> instance and the <b>Texture</b> hash code, so it can be de/serialized.</summary>
		public static Dictionary<Texture, P3dHash> TextureToHash = new Dictionary<Texture, P3dHash>();

		/// <summary>This stores an association between a <b>P3dModel</b> hash code and the <b>P3dModel</b> instance, so it can be de/serialized.</summary>
		public static Dictionary<P3dHash, P3dPaintableTexture> HashToPaintableTexture = new Dictionary<P3dHash, P3dPaintableTexture>();

		/// <summary>This stores an association between a <b>P3dModel</b> instance and the <b>P3dModel</b> hash code, so it can be de/serialized.</summary>
		public static Dictionary<P3dPaintableTexture, P3dHash> PaintableTextureToHash = new Dictionary<P3dPaintableTexture, P3dHash>();

		public static void TryRegister(P3dPaintableTexture paintableTexture, P3dHash hash)
		{
			TryRegister(paintableTexture, hash, HashToPaintableTexture, PaintableTextureToHash);
		}

		public static void TryRegister(P3dModel model, P3dHash hash)
		{
			TryRegister(model, model.Hash, HashToModel, ModelToHash);
		}

		public static void TryRegister(Texture texture, P3dHash hash)
		{
			TryRegister(texture, hash, HashToTexture, TextureToHash);
		}

		public static void TryRegister<T>(T obj, P3dHash hash, Dictionary<P3dHash, T> hashToObj, Dictionary<T, P3dHash> objToHash)
			where T : Object
		{
			P3dHash existingHash;

			if (objToHash.TryGetValue(obj, out existingHash) == true)
			{
				// Already up to date
				if (existingHash == hash)
				{
					return;
				}

				// Remove old hash
				objToHash.Remove(obj);
				hashToObj.Remove(existingHash);
			}

			// Register new
			if (hash != default(P3dHash))
			{
				objToHash.Add(obj, hash);
				hashToObj.Add(hash, obj);
			}
		}

		public static int TryRegister(Material material)
		{
			var hash = GetStableStringHash(material.name);

			if (HashToMaterial.ContainsKey(hash) == true)
			{
				throw new System.Exception("You're trying to register the " + material + " Material, but you've already registered the " + HashToMaterial[hash] + " Material with the same hash.");
			}

			MaterialToHash.Add(material, hash);
			HashToMaterial.Add(hash, material);

			return hash;
		}

		private static int GetStableStringHash(string s)
		{
			var hash = 23;

			foreach (var c in s)
			{
				hash = hash * 31 + c;
			}

			return hash;
		}
	}
}