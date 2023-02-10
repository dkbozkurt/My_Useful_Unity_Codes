using UnityEngine;
using Unity.Collections;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This base class allows you to quickly create components that listen for changes to the specified <b>P3dPaintableTexture</b>.</summary>
	public abstract class P3dPaintableTextureMonitor : MonoBehaviour
	{
		/// <summary>This is the paintable texture whose pixels we will count.</summary>
		public P3dPaintableTexture PaintableTexture { set { paintableTexture = value; Register(); } get { return paintableTexture; } } [SerializeField] private P3dPaintableTexture paintableTexture;

		/// <summary>This allows you to specify the minimum delay between each texture read in seconds.
		/// 0 = Instant.
		/// 1 = Once a second.</summary>
		public float Interval { set { interval = value; } get { return interval; } } [SerializeField] private float interval;

		/// <summary>If you disable this, then the texture will be updated immediately, which may cause slowdown.
		/// NOTE: This isn't supported on all devices.</summary>
		public bool Async { set { async = value; } get { return async; } } [SerializeField] private bool async = true;

		/// <summary>Testing all the pixels of a texture can be slow, so you can pick how many times the texture is downsampled. One downsample = half width & height or 1/4 of the pixels.
		/// NOTE: The pixel totals will be multiplied to account for this downsampling.</summary>
		public int DownsampleSteps { set { downsampleSteps = value; } get { return downsampleSteps; } } [SerializeField] protected int downsampleSteps = 3;

		/// <summary>This event is invoked each time this texture monitor updates its pixel counts.</summary>
		public event System.Action OnUpdated;

		[SerializeField]
		protected P3dPaintableTexture registeredPaintableTexture;

		[SerializeField]
		private float cooldown;

		[SerializeField]
		private P3dReader currentReader;

		[SerializeField]
		protected NativeArray<Color32> currentPixels;

		/// <summary>This will be true after Register is successfully called.</summary>
		public bool Registered
		{
			get
			{
				return registeredPaintableTexture != null;
			}
		}

		public P3dReader CurrentReader
		{
			get
			{
				return currentReader;
			}
		}

		public void MarkCurrentReaderAsDirty()
		{
			if (currentReader != null)
			{
				currentReader.MarkAsDirty();
			}
		}

		/// <summary>This forces the specified P3dPaintableTexture to be registered.</summary>
		[ContextMenu("Register")]
		public void Register()
		{
			Unregister();

			if (paintableTexture != null)
			{
				paintableTexture.OnModified += HandleModified;

				registeredPaintableTexture = paintableTexture;
			}
		}

		/// <summary>This forces the specified P3dPaintableTexture to be unregistered.</summary>
		[ContextMenu("Unregister")]
		public void Unregister()
		{
			if (registeredPaintableTexture != null)
			{
				registeredPaintableTexture.OnModified -= HandleModified;

				registeredPaintableTexture = null;
			}
		}

		protected void InvokeOnUpdated()
		{
			if (OnUpdated != null)
			{
				OnUpdated.Invoke();
			}
		}

		protected virtual void OnEnable()
		{
			Register();

			if (currentReader != null)
			{
				currentReader.OnComplete += HandleCompleteCurrent;
			}
		}

		protected virtual void OnDisable()
		{
			Unregister();

			if (currentReader != null)
			{
				currentReader.OnComplete -= HandleCompleteCurrent;
			}
		}

		protected virtual void OnDestroy()
		{
			if (currentReader != null)
			{
				currentReader.Release();
			}

			if (currentPixels.IsCreated == true)
			{
				currentPixels.Dispose();
			}
		}

		protected virtual void Update()
		{
			if (currentReader == null)
			{
				currentReader = new P3dReader();

				currentReader.OnComplete += HandleCompleteCurrent;
			}

			cooldown -= Time.deltaTime;

			if (currentReader.Dirty)

			if (cooldown <= 0.0f && currentReader.Requested == false && registeredPaintableTexture != null && registeredPaintableTexture.Activated == true)
			{
				if (P3dReader.NeedsUpdating(currentReader, currentPixels, registeredPaintableTexture.Current, downsampleSteps) == true)
				{
					cooldown = interval;

					currentReader.Request(registeredPaintableTexture.Current, downsampleSteps, async);
				}
			}

			currentReader.UpdateRequest();
		}

		private void HandleCompleteCurrent(NativeArray<Color32> pixels)
		{
			if (currentPixels.IsCreated == true && currentPixels.Length != pixels.Length)
			{
				currentPixels.Dispose();
			}

			if (currentPixels.IsCreated == true)
			{
				NativeArray<Color32>.Copy(pixels, currentPixels);
			}
			else
			{
				currentPixels = new NativeArray<Color32>(pixels, Allocator.Persistent);
			}

			HandleComplete(currentReader.DownsampleBoost);
		}

		private void HandleModified(bool preview)
		{
			if (preview == false)
			{
				MarkCurrentReaderAsDirty();
			}
		}

		protected abstract void HandleComplete(int boost);
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dPaintableTextureMonitor;

	[CustomEditor(typeof(TARGET))]
	public class P3dPaintableTextureMonitor_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			BeginError(Any(tgts, t => t.PaintableTexture == null));
				if (Draw("paintableTexture", "This is the paintable texture whose pixels we will count.") == true)
				{
					Each(tgts, t =>
						{
							if (t.Registered == true)
							{
								t.Register();
							}
						}, true);
				}
			EndError();
			Draw("interval", "This allows you to specify the maximum time between each texture read in seconds.\n\n0 = Instant.\n\n1 = Once a second.");
			Draw("async", "If you disable this, then the texture will be updated immediately, which may cause slowdown.\n\nNOTE: This isn't supported on all devices.");
			BeginError(Any(tgts, t => t.DownsampleSteps < 0));
				Draw("downsampleSteps", "Counting all the pixels of a texture can be slow, so you can pick how many times the texture is downsampled before it gets counted. One downsample = half width & height or 1/4 of the pixels. NOTE: The pixel totals will be multiplied to account for this downsampling.");
			EndError();
		}
	}
}
#endif