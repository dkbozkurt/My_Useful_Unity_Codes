using UnityEngine;
using Unity.Collections;

namespace PaintIn3D
{
	/// <summary>This base class allows you to quickly create components that listen for changes to the specified P3dPaintableTexture.</summary>
	public abstract class P3dPaintableTextureMonitorMask : P3dPaintableTextureMonitor
	{
		/// <summary>If you want this component to accurately count pixels relative to a mask mesh, then specify it here.
		/// NOTE: For best results this should be the original mesh, NOT the seam-fixed version.</summary>
		public Mesh MaskMesh { set { maskMesh = value; } get { return maskMesh; } } [UnityEngine.Serialization.FormerlySerializedAs("mesh")] [SerializeField] private Mesh maskMesh;

		/// <summary>If you have a <b>MaskMesh</b> set, then this allows you to choose which submesh of it will be used for the mask.</summary>
		public int MaskSubmesh { set { maskSubmesh = value; } get { return maskSubmesh; } } [SerializeField] private int maskSubmesh;

		/// <summary>If you want this component to accurately count pixels relative to a mask texture, then specify it here.</summary>
		public Texture MaskTexture { set { maskTexture = value; } get { return maskTexture; } } [SerializeField] private Texture maskTexture;

		/// <summary>This allows you to specify which channel of the <b>MaskTexture</b> will be used to define the mask.</summary>
		public P3dChannel MaskChannel { set { maskChannel = value; } get { return maskChannel; } } [SerializeField] private P3dChannel maskChannel = P3dChannel.Alpha;

		/// <summary>The previously counted total amount of pixels.</summary>
		public int Total { get { return total; } } [SerializeField] protected int total;

		[SerializeField]
		private P3dReader maskReader;

		[SerializeField]
		protected NativeArray<byte> maskPixels;

		public P3dReader MaskReader
		{
			get
			{
				return maskReader;
			}
		}

		public void MarkMaskReaderAsDirty()
		{
			if (maskReader != null)
			{
				maskReader.MarkAsDirty();
			}
		}

		private void HandleCompleteMask(NativeArray<Color32> pixels)
		{
			if (maskPixels.IsCreated == true && maskPixels.Length != pixels.Length)
			{
				maskPixels.Dispose();
			}

			if (maskPixels.IsCreated == false)
			{
				maskPixels = new NativeArray<byte>(pixels.Length, Allocator.Persistent);
			}

			if (maskTexture != null)
			{
				switch (maskChannel)
				{
					case P3dChannel.Red  : for (var i = 0; i < pixels.Length; i++) maskPixels[i] = pixels[i].r; break;
					case P3dChannel.Green: for (var i = 0; i < pixels.Length; i++) maskPixels[i] = pixels[i].g; break;
					case P3dChannel.Blue : for (var i = 0; i < pixels.Length; i++) maskPixels[i] = pixels[i].b; break;
					case P3dChannel.Alpha: for (var i = 0; i < pixels.Length; i++) maskPixels[i] = pixels[i].a; break;
				}
			}
			else
			{
				for (var i = 0; i < pixels.Length; i++) maskPixels[i] = pixels[i].r;
			}

			HandleComplete(maskReader.DownsampleBoost);
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			if (maskReader != null)
			{
				maskReader.OnComplete += HandleCompleteMask;
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (maskReader != null)
			{
				maskReader.OnComplete -= HandleCompleteMask;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (maskReader != null)
			{
				maskReader.Release();
			}

			if (maskPixels.IsCreated == true)
			{
				maskPixels.Dispose();
			}
		}

		protected override void Update()
		{
			base.Update();

			if (maskReader == null)
			{
				maskReader = new P3dReader();

				maskReader.OnComplete += HandleCompleteMask;
			}

			if (maskReader.Requested == false && registeredPaintableTexture != null && registeredPaintableTexture.Activated == true)
			{
				if (P3dReader.NeedsUpdating(maskReader, maskPixels, registeredPaintableTexture.Current, downsampleSteps) == true)
				{
					var desc          = registeredPaintableTexture.Current.descriptor; desc.useMipMap = false;
					var renderTexture = P3dCommon.GetRenderTexture(desc);

					if (maskTexture != null)
					{
						P3dBlit.Texture(renderTexture, P3dCommon.GetQuadMesh(), 0, maskTexture, P3dCoord.First);
					}
					else
					{
						P3dBlit.White(renderTexture, maskMesh, maskSubmesh, registeredPaintableTexture.Coord);
					}

					// Request new mask
					maskReader.Request(renderTexture, DownsampleSteps, Async);

					P3dCommon.ReleaseRenderTexture(renderTexture);
				}
			}

			maskReader.UpdateRequest();
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dPaintableTextureMonitorMask;

	[CustomEditor(typeof(TARGET))]
	public class P3dPaintableTextureMonitorMask_Editor : P3dPaintableTextureMonitor_Editor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			base.OnInspector();

			var markAsDirty = false;

			BeginError(Any(tgts, t => t.MaskMesh != null && t.MaskTexture != null));
				Draw("maskMesh", "If you want this component to accurately count pixels relative to a mask mesh, then specify it here.\n\nNOTE: For best results this should be the original mesh, NOT the seam-fixed version.");
				Draw("maskSubmesh", "If you have a <b>MaskMesh</b> set, then this allows you to choose which submesh of it will be used for the mask.");

				EditorGUILayout.BeginHorizontal();
				Draw("maskTexture", "If you want this component to accurately count pixels relative to a mask texture, then specify it here.");
				EditorGUILayout.PropertyField(serializedObject.FindProperty("maskChannel"), GUIContent.none, GUILayout.Width(50));
			EditorGUILayout.EndHorizontal();
			EndError();

			if (markAsDirty == true)
			{
				Each(tgts, t => t.MarkMaskReaderAsDirty(), true, true);
			}
		}
	}
}
#endif