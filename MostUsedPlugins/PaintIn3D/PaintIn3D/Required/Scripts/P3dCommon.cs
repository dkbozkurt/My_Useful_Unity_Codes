using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This class contains some useful methods used by this asset.</summary>
	public static partial class P3dCommon
	{
		public const string HelpUrlPrefix = "https://carloswilkes.com/Documentation/PaintIn3D#";

		public const string ComponentMenuPrefix = "Paint in 3D/P3D ";

		public const string ComponentHitMenuPrefix = "Paint in 3D/Hit/P3D ";

		public static System.Action<Camera> OnCameraDraw;

		private static int _Coord = Shader.PropertyToID("_Coord");

		static P3dCommon()
		{
			Camera.onPreCull += (camera) =>
				{
					if (OnCameraDraw != null) OnCameraDraw(camera);
				};

			UnityEngine.Rendering.RenderPipelineManager.beginCameraRendering += (context, camera) =>
				{
					if (OnCameraDraw != null) OnCameraDraw(camera);
				};
		}

		public static float RatioToPercentage(float ratio01, int decimalPlaces)
		{
			var percentage = Mathf.Clamp01(ratio01) * 100.0;
			var multiplier = 1.0;

			if (decimalPlaces >= 0)
			{
				multiplier = System.Math.Pow(10.0, decimalPlaces);
			}

			return (float)(System.Math.Truncate(percentage * multiplier) / multiplier);
		}

		public static RenderTexture GetRenderTexture(RenderTexture other)
		{
			return GetRenderTexture(other.descriptor, other);
		}

		public static RenderTexture GetRenderTexture(RenderTextureDescriptor desc, RenderTexture other)
		{
			var renderTexture = GetRenderTexture(desc);

			renderTexture.filterMode = other.filterMode;
			renderTexture.anisoLevel = other.anisoLevel;
			renderTexture.wrapModeU  = other.wrapModeU;
			renderTexture.wrapModeV  = other.wrapModeV;

			return renderTexture;
		}

		public static RenderTexture GetRenderTexture(RenderTextureDescriptor desc)
		{
			return GetRenderTexture(desc, QualitySettings.activeColorSpace == ColorSpace.Gamma);
		}

		public static RenderTexture GetRenderTexture(RenderTextureDescriptor desc, bool sRGB)
		{
			desc.sRGB = sRGB;

			var renderTexture = RenderTexture.GetTemporary(desc);

			// TODO: For some reason RenderTexture.GetTemporary ignores the useMipMap flag?!
			if (renderTexture.useMipMap != desc.useMipMap)
			{
				renderTexture.Release();

				renderTexture.descriptor = desc;

				renderTexture.Create();
			}

			renderTexture.DiscardContents();

			return renderTexture;
		}

		public static RenderTexture ReleaseRenderTexture(RenderTexture renderTexture)
		{
			RenderTexture.ReleaseTemporary(renderTexture);

			return null;
		}

		public static Quaternion NormalToCameraRotation(Vector3 normal, Camera optionalCamera = null)
		{
			var up     = Vector3.up;
			var camera = CwHelper.GetCamera(optionalCamera);

			if (camera != null)
			{
				up = camera.transform.up;
			}

			return Quaternion.LookRotation(-normal, up);
		}

		public static Vector3 GetCameraUp(Camera camera = null)
		{
			camera = CwHelper.GetCamera(camera);

			return camera != null ? camera.transform.up : Vector3.up;
		}
		
		public static bool CanReadPixels(TextureFormat format)
		{
			if (format == TextureFormat.RGBA32 || format == TextureFormat.ARGB32 || format == TextureFormat.RGB24 || format == TextureFormat.RGBAFloat || format == TextureFormat.RGBAHalf)
			{
				return true;
			}

			return false;
		}

		public static void ReadPixelsLinearGamma(Texture2D texture2D, RenderTexture renderTexture)
		{
			if (renderTexture != null)
			{
				CwHelper.BeginActive(renderTexture);

				var buffer = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false, true);

				buffer.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

				CwHelper.EndActive();

				var pixels = buffer.GetPixels();

				for (var i = pixels.Length - 1; i >= 0; i--)
				{
					pixels[0] = pixels[0].gamma;
				}

				Object.DestroyImmediate(buffer);

				texture2D.SetPixels(pixels);
				texture2D.Apply();
			}
		}

		public static void ReadPixels(Texture2D texture2D, RenderTexture renderTexture)
		{
			if (renderTexture != null)
			{
				CwHelper.BeginActive(renderTexture);

				if (CanReadPixels(texture2D.format) == true)
				{
					texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

					CwHelper.EndActive();

					texture2D.Apply();
				}
				else
				{
					var buffer = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);

					buffer.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

					CwHelper.EndActive();

					var pixels = buffer.GetPixels32();

					Object.DestroyImmediate(buffer);

					texture2D.SetPixels32(pixels);
					texture2D.Apply();
				}
			}
		}

		public static bool Downsample(RenderTexture renderTexture, int steps, ref RenderTexture temporary)
		{
			if (steps > 0 && renderTexture != null)
			{
				// Perform initial downsample to get buffer
				var oldActive         = RenderTexture.active;
				var desc              = new RenderTextureDescriptor(renderTexture.width / 2, renderTexture.height / 2, renderTexture.format, 0);
				var halfRenderTexture = GetRenderTexture(desc);

				P3dCommandReplace.BlitFast(halfRenderTexture, renderTexture, Color.white);

				// Ping-pong downsample
				for (var i = 1; i < steps; i++)
				{
					desc.width       /= 2;
					desc.height      /= 2;
					renderTexture     = halfRenderTexture;
					halfRenderTexture = GetRenderTexture(desc);

					Graphics.Blit(renderTexture, halfRenderTexture);

					ReleaseRenderTexture(renderTexture);
				}

				temporary = halfRenderTexture;

				RenderTexture.active = oldActive;

				return true;
			}

			return false;
		}

		public static bool HasMipMaps(Texture texture)
		{
			if (texture != null)
			{
				var texture2D = texture as Texture2D;

				if (texture2D != null)
				{
					return texture2D.mipmapCount > 0;
				}

				var textureRT = texture as RenderTexture;

				if (textureRT != null)
				{
					return textureRT.useMipMap;
				}
			}

			return false;
		}

		private static Mesh sphereMesh;
		private static bool sphereMeshSet;

		public static Mesh GetSphereMesh()
		{
			if (sphereMeshSet == false)
			{
				var gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

				sphereMeshSet = true;
				sphereMesh    = gameObject.GetComponent<MeshFilter>().sharedMesh;

				Object.DestroyImmediate(gameObject);
			}

			return sphereMesh;
		}

		private static Mesh quadMesh;
		private static bool quadMeshSet;

		public static Mesh GetMesh(GameObject root, Mesh mesh = null)
		{
			if (mesh == null)
			{
				var meshFilter = root.GetComponent<MeshFilter>();

				if (meshFilter != null)
				{
					return meshFilter.sharedMesh;
				}
				else
				{
					var skinnedMeshRenderer = root.GetComponent<SkinnedMeshRenderer>();

					if (skinnedMeshRenderer != null)
					{
						return skinnedMeshRenderer.sharedMesh;
					}
				}
			}

			return mesh;
		}

		public static Mesh GetQuadMesh()
		{
			if (quadMeshSet == false)
			{
				var gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);

				quadMeshSet = true;
				quadMesh    = gameObject.GetComponent<MeshFilter>().sharedMesh;

				Object.DestroyImmediate(gameObject);
			}

			return quadMesh;
		}

		private static Texture2D tempReadTexture;

		public static Color GetPixel(RenderTexture renderTexture, Vector2 uv, bool mipMaps = false)
		{
			if (renderTexture != null)
			{
				if (tempReadTexture == null)
				{
					tempReadTexture = new Texture2D(2, 2, TextureFormat.ARGB32, mipMaps, QualitySettings.activeColorSpace == ColorSpace.Linear);
				}

				if (SystemInfo.graphicsUVStartsAtTop == true && SystemInfo.graphicsDeviceType != UnityEngine.Rendering.GraphicsDeviceType.Metal) // Metal gives the wrong value for graphicsUVStartsAtTop?!
				{
					uv.y = 1.0f - uv.y;
				}

				var x = uv.x * renderTexture.width;
				var y = uv.y * renderTexture.height;

				CwHelper.BeginActive(renderTexture);
					tempReadTexture.ReadPixels(new Rect(x, y, 1, 1), 0, 0);
				CwHelper.EndActive();

				tempReadTexture.Apply();

				return CwHelper.ToGamma(tempReadTexture.GetPixel(0, 0));
			}

			return default(Color);
		}

		public static Texture2D GetReadableCopy(Texture texture, TextureFormat format = TextureFormat.ARGB32, bool mipMaps = false, int width = 0, int height = 0)
		{
			var newTexture = default(Texture2D);

			if (texture != null)
			{
				if (width <= 0)
				{
					width = texture.width;
				}

				if (height <= 0)
				{
					height = texture.height;
				}

				if (CanReadPixels(format) == true)
				{
					var desc          = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 0);
					var renderTexture = GetRenderTexture(desc, true);

					newTexture = new Texture2D(width, height, format, mipMaps, false);

					CwHelper.BeginActive(renderTexture);
						Graphics.Blit(texture, renderTexture);

						newTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
					CwHelper.EndActive();

					ReleaseRenderTexture(renderTexture);

					newTexture.Apply();
				}
			}

			return newTexture;
		}

		/// <summary>This method allows you to save a byte array to PlayerPrefs, and is used by the texture saving system.
		/// If you want to save to files instead then just modify this.</summary>
		public static void SaveBytes(string saveName, byte[] data, bool save = true)
		{
			var base64 = default(string);

			if (data != null)
			{
				base64 = System.Convert.ToBase64String(data);
			}

			PlayerPrefs.SetString(saveName, base64);

			if (save == true)
			{
				PlayerPrefs.Save();
			}
		}

		/// <summary>This method allows you to load a byte array from PlayerPrefs, and is used by the texture loading system.
		/// If you want to save to files instead then just modify this.</summary>
		public static byte[] LoadBytes(string saveName)
		{
			var base64 = PlayerPrefs.GetString(saveName);

			if (string.IsNullOrEmpty(base64) == false)
			{
				return System.Convert.FromBase64String(base64);
			}

			return null;
		}

		/// <summary>This method tells if you if there exists save data at the specified save name.</summary>
		public static bool SaveExists(string saveName)
		{
			return PlayerPrefs.HasKey(saveName);
		}

		/// <summary>This method allows you to clear save data at the specified save name.</summary>
		public static void ClearSave(string saveName, bool save = true)
		{
			if (PlayerPrefs.HasKey(saveName) == true)
			{
				PlayerPrefs.DeleteKey(saveName);

				if (save == true)
				{
					PlayerPrefs.Save();
				}
			}
		}

		public static Vector3 GetPosition(Vector3 position, Vector3 endPosition)
		{
			return (position + endPosition) / 2.0f;
		}

		public static Vector3 GetPosition(Vector3 positionA, Vector3 positionB, Vector3 positionC)
		{
			return (positionA + positionB + positionC) / 3.0f;
		}

		public static Vector3 GetPosition(Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2)
		{
			return (position + position2 + endPosition + endPosition2) / 4.0f;
		}

		public static float GetRadius(Vector3 size)
		{
			return Mathf.Sqrt(Vector3.Dot(size, size));
		}

		public static float GetRadius(Vector3 size, Vector3 position, Vector3 endPosition)
		{
			size.x = System.Math.Abs(size.x) + System.Math.Abs(endPosition.x - position.x);
			size.y = System.Math.Abs(size.y) + System.Math.Abs(endPosition.y - position.y);
			size.z = System.Math.Abs(size.z) + System.Math.Abs(endPosition.z - position.z);

			return GetRadius(size);
		}

		public static float GetRadius(Vector3 size, Vector3 positionA, Vector3 positionB, Vector3 positionC)
		{
			var minX = System.Math.Min(System.Math.Min(positionA.x, positionB.x), positionC.x);
			var maxX = System.Math.Max(System.Math.Max(positionA.x, positionB.x), positionC.x);
			var minY = System.Math.Min(System.Math.Min(positionA.y, positionB.y), positionC.y);
			var maxY = System.Math.Max(System.Math.Max(positionA.y, positionB.y), positionC.y);
			var minZ = System.Math.Min(System.Math.Min(positionA.z, positionB.z), positionC.z);
			var maxZ = System.Math.Max(System.Math.Max(positionA.z, positionB.z), positionC.z);

			size.x = System.Math.Abs(size.x) + System.Math.Abs(maxX - minX);
			size.y = System.Math.Abs(size.y) + System.Math.Abs(maxY - minY);
			size.z = System.Math.Abs(size.z) + System.Math.Abs(maxZ - minZ);

			return GetRadius(size);
		}

		public static float GetRadius(Vector3 size, Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2)
		{
			var minX = System.Math.Min(System.Math.Min(position.x, position2.x), System.Math.Min(endPosition.x, endPosition2.x));
			var maxX = System.Math.Max(System.Math.Max(position.x, position2.x), System.Math.Max(endPosition.x, endPosition2.x));
			var minY = System.Math.Min(System.Math.Min(position.y, position2.y), System.Math.Min(endPosition.y, endPosition2.y));
			var maxY = System.Math.Max(System.Math.Max(position.y, position2.y), System.Math.Max(endPosition.y, endPosition2.y));
			var minZ = System.Math.Min(System.Math.Min(position.z, position2.z), System.Math.Min(endPosition.z, endPosition2.z));
			var maxZ = System.Math.Max(System.Math.Max(position.z, position2.z), System.Math.Max(endPosition.z, endPosition2.z));

			size.x = System.Math.Abs(size.x) + System.Math.Abs(maxX - minX);
			size.y = System.Math.Abs(size.y) + System.Math.Abs(maxY - minY);
			size.z = System.Math.Abs(size.z) + System.Math.Abs(maxZ - minZ);

			return GetRadius(size);
		}

		public static Vector3 ScaleAspect(Vector3 size, float aspect)
		{
			if (aspect > 1.0f)
			{
				size.y /= aspect;
			}
			else
			{
				size.x *= aspect;
			}

			return size;
		}

		public static float GetAspect(Texture textureA, Texture textureB = null)
		{
			if (textureA != null)
			{
				return textureA.width / (float)textureA.height;
			}

			if (textureB != null)
			{
				return textureB.width / (float)textureB.height;
			}

			return 1.0f;
		}

		public static void Blit(RenderTexture renderTexture, Texture other)
		{
			var oldActive = RenderTexture.active;

			Graphics.Blit(other, renderTexture);

			RenderTexture.active = oldActive;
		}

		public static void Blit(RenderTexture renderTexture, Material material, int pass)
		{
			CwHelper.BeginActive(renderTexture);

			Draw(material, pass);
			//Graphics.Blit(default(Texture), renderTexture, material);

			CwHelper.EndActive();
		}

		public static Vector4 IndexToVector(int index)
		{
			switch (index)
			{
				case 0: return new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
				case 1: return new Vector4(0.0f, 1.0f, 0.0f, 0.0f);
				case 2: return new Vector4(0.0f, 0.0f, 1.0f, 0.0f);
				case 3: return new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
			}

			return default(Vector4);
		}

		public static void Draw(Material material, int pass, Mesh mesh, Matrix4x4 matrix, int subMesh, P3dCoord coord)
		{
			material.SetVector(_Coord, IndexToVector((int)coord));

			if (material.SetPass(pass) == true)
			{
				Graphics.DrawMeshNow(mesh, matrix, subMesh);
			}
		}

		public static void Draw(Material material, int pass)
		{
			if (material.SetPass(pass) == true)
			{
				Graphics.DrawMeshNow(GetQuadMesh(), Matrix4x4.identity, 0);
			}
		}

		public static Texture2D CreateTexture(int width, int height, TextureFormat format, bool mipMaps)
		{
			if (width > 0 && height > 0)
			{
				return new Texture2D(width, height, format, mipMaps);
			}

			return null;
		}

		// This method allows you to easily find a Material attached to a GameObject
		public static Material GetMaterial(Renderer renderer, int materialIndex = 0)
		{
			if (renderer != null && materialIndex >= 0)
			{
				var materials = renderer.sharedMaterials;

				if (materialIndex < materials.Length)
				{
					return materials[materialIndex];
				}
			}

			return null;
		}

		// This method allows you to easily duplicate a Material attached to a GameObject
		public static Material CloneMaterial(GameObject gameObject, int materialIndex = 0)
		{
			if (gameObject != null && materialIndex >= 0)
			{
				var renderer = gameObject.GetComponent<Renderer>();

				if (renderer != null)
				{
					var materials = renderer.sharedMaterials;

					if (materialIndex < materials.Length)
					{
						// Get existing material
						var material = materials[materialIndex];

						// Clone it
						material = Object.Instantiate(material);

						// Update array
						materials[materialIndex] = material;

						// Update materials
						renderer.sharedMaterials = materials;

						return material;
					}
				}
			}

			return null;
		}

		// This method allows you to add a material (layer) to a renderer at the specified material index, or -1 for the end (top)
		public static Material AddMaterial(Renderer renderer, Shader shader, int materialIndex = -1)
		{
			if (renderer != null)
			{
				var newMaterials = new List<Material>(renderer.sharedMaterials);
				var newMaterial  = new Material(shader);

				if (materialIndex <= 0)
				{
					materialIndex = newMaterials.Count;
				}

				newMaterials.Insert(materialIndex, newMaterial);

				renderer.sharedMaterials = newMaterials.ToArray();

				return newMaterial;
			}

			return null;
		}

		public static Shader LoadShader(string shaderName)
		{
			var shader = Shader.Find(shaderName);

			if (shader == null)
			{
				throw new System.Exception("Failed to find shader called: " + shaderName);
			}

			return shader;
		}

		public static Material BuildMaterial(Shader shader)
		{
			var material = new Material(shader);
#if UNITY_EDITOR
			material.hideFlags = HideFlags.DontSave;
#endif
			return material;
		}

		public static Material BuildMaterial(string shaderName, string keyword = null)
		{
			var shader   = LoadShader(shaderName);
			var material = BuildMaterial(shader);

			material.name = shaderName + keyword;

			if (string.IsNullOrEmpty(keyword) == false)
			{
				material.EnableKeyword(keyword);
			}

			return material;
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;

	public static partial class P3dCommon
	{
		public static string FindScriptGUID<T>()
			where T : MonoBehaviour
		{
			var guids = AssetDatabase.FindAssets("t:Script " + typeof(T).Name);

			foreach (var guid in guids)
			{
				if (System.IO.Path.GetFileName(AssetDatabase.GUIDToAssetPath(guid)) == typeof(T).Name + ".cs")
				{
					return guid;
				}
			}

			return null;
		}

		public static T LoadPrefabIfItContainsScriptGUID<T>(string prefabGuid, string scriptGuid)
			where T : MonoBehaviour
		{
			var prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuid);

			foreach (var line in System.IO.File.ReadLines(prefabPath))
			{
				if (line.Contains(scriptGuid) == true)
				{
					var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

					if (prefab != null)
					{
						var component = prefab.GetComponent<T>();

						if (component != null)
						{
							return component;
						}
					}

					break;
				}
			}

			return null;
		}

		public static void ClearControl()
		{
			GUIUtility.keyboardControl = -1;
			GUIUtility.hotControl      = -1;
		}

		public static bool TexEnvNameExists(Shader shader, string name)
		{
			if (shader != null)
			{
				var count = ShaderUtil.GetPropertyCount(shader);
				
				for (var i = 0; i < count; i++)
				{
					if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
					{
						if (ShaderUtil.GetPropertyName(shader, i) == name)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public struct TexEnv
		{
			public string Name;
			public string Desc;

			public string Title
			{
				get
				{
					return Desc + " (" + Name + ")";
				}
			}
		}

		private static List<TexEnv> texEnvNames = new List<TexEnv>();

		public static List<TexEnv> GetTexEnvs(Shader shader)
		{
			texEnvNames.Clear();

			if (shader != null)
			{
				var count = ShaderUtil.GetPropertyCount(shader);

				for (var i = 0; i < count; i++)
				{
					if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
					{
						var texEnv = default(TexEnv);

						texEnv.Name = ShaderUtil.GetPropertyName(shader, i);
						texEnv.Desc = ShaderUtil.GetPropertyDescription(shader, i);

						texEnvNames.Add(texEnv);
					}
				}
			}

			return texEnvNames;
		}
	}
}
#endif