using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This class allows you to read the contents of a <b>RenderTexture</b> immediately or async.</summary>
	[System.Serializable]
	public class P3dReader
	{
		[SerializeField]
		private AsyncGPUReadbackRequest request;

		[SerializeField]
		private bool dirty;

		[SerializeField]
		private bool requested;

		[SerializeField]
		private RenderTexture buffer;

		[SerializeField]
		private Vector2Int originalSize;

		[SerializeField]
		private Vector2Int downsampledSize;

		[SerializeField]
		private int downsampleSteps;

		[SerializeField]
		private int downsampleBoost;

		[SerializeField]
		private Texture2D tempTexture;

		public event System.Action<NativeArray<Color32>> OnComplete;

		public bool Dirty
		{
			get
			{
				return dirty;
			}
		}

		public bool Requested
		{
			get
			{
				return requested;
			}
		}

		public Vector2Int OriginalSize
		{
			get
			{
				return originalSize;
			}
		}

		public int DownsampleSteps
		{
			get
			{
				return downsampleSteps;
			}
		}

		public Vector2Int DownsampledSize
		{
			get
			{
				return downsampledSize;
			}
		}

		public int DownsampleBoost
		{
			get
			{
				return downsampleBoost;
			}
		}

		public void MarkAsDirty()
		{
			dirty = true;
		}

		public void UpdateRequest()
		{
			if (requested == true)
			{
				if (request.hasError == true)
				{
					requested = false;

					CompleteDirectly();
				}
				else if (request.done == true)
				{
					requested = false;

					buffer = P3dCommon.ReleaseRenderTexture(buffer);

					OnComplete(request.GetData<Color32>());
				}
			}
		}

		public static bool NeedsUpdating<T>(P3dReader reader, NativeArray<T> array, RenderTexture texture, int downsampleSteps)
			where T : struct
		{
			if (array.IsCreated == false || reader.dirty == true || reader.DownsampledSize.x * reader.DownsampledSize.y != array.Length)
			{
				return true;
			}

			var originalSize    = Vector2Int.zero;
			var downsampledSize = Vector2Int.zero;

			originalSize.x = downsampledSize.x = texture.width;
			originalSize.y = downsampledSize.y = texture.height;

			for (var i = 0; i < downsampleSteps; i++)
			{
				if (downsampledSize.x > 2)
				{
					downsampledSize.x /= 2;
				}

				if (downsampledSize.y > 2)
				{
					downsampledSize.y /= 2;
				}
			}

			return reader.OriginalSize != originalSize || reader.DownsampledSize != downsampledSize;
		}

		public void Request(RenderTexture texture, int downsample, bool async)
		{
			if (texture == null)
			{
				Debug.LogError("Texture null."); return;
			}

			if (requested == true)
			{
				Debug.LogError("Already requested."); return;
			}

			if (buffer != null)
			{
				Debug.LogError("Buffer exists."); return;
			}

			originalSize.x = downsampledSize.x = texture.width;
			originalSize.y = downsampledSize.y = texture.height;

			for (var i = 0; i < downsample; i++)
			{
				if (downsampledSize.x > 2)
				{
					downsampledSize.x /= 2;
				}

				if (downsampledSize.y > 2)
				{
					downsampledSize.y /= 2;
				}
			}

			downsampleSteps = downsample;
			downsampleBoost = (originalSize.x / downsampledSize.x) * (originalSize.y / downsampledSize.y);

			var desc = texture.descriptor;

			desc.useMipMap = false;
			desc.width     = downsampledSize.x;
			desc.height    = downsampledSize.y;

			buffer = P3dCommon.GetRenderTexture(desc);

			P3dCommandReplace.Blit(buffer, texture, Color.white);

			if (async == true && SystemInfo.supportsAsyncGPUReadback == true)
			{
				request   = AsyncGPUReadback.Request(buffer, 0, TextureFormat.RGBA32);
				requested = true;

				UpdateRequest();
			}
			else
			{
				CompleteDirectly();
			}
		}

		public void Release()
		{
			buffer = P3dCommon.ReleaseRenderTexture(buffer);

			tempTexture = CwHelper.Destroy(tempTexture);
		}

		private void CompleteDirectly()
		{
			if (tempTexture != null && (tempTexture.width != buffer.width || tempTexture.height != buffer.height))
			{
				tempTexture = CwHelper.Destroy(tempTexture);
			}

			if (tempTexture == null)
			{
				tempTexture = new Texture2D(buffer.width, buffer.height, TextureFormat.RGBA32, false);
			}

			CwHelper.BeginActive(buffer);

			tempTexture.ReadPixels(new Rect(0, 0, buffer.width, buffer.height), 0, 0);

			CwHelper.EndActive();

			buffer = P3dCommon.ReleaseRenderTexture(buffer);

			tempTexture.Apply();

			OnComplete(tempTexture.GetRawTextureData<Color32>());
		}
	}
}