using UnityEngine;
using UnityEngine.Events;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to read the paint color at a hit point. A hit point can be found using a companion component like: P3dHitScreen, P3dHitBetween.
	/// NOTE: This component only works when you hit a non-convex MeshCollider that has UV data.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dReadColor")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Read Color")]
	public class P3dReadColor : MonoBehaviour, IHitCoord
	{
		[System.Serializable] public class ColorEvent : UnityEvent<Color> {}

		/// <summary>Only the <b>P3dPaintableTexture</b> components with a matching group will be painted by this component.</summary>
		public P3dGroup Group { set { group = value; } get { return group; } } [SerializeField] private P3dGroup group;

		/// <summary>Should the color be read during preview painting too?</summary>
		public bool Preview { set { preview = value; } get { return preview; } } [SerializeField] private bool preview;

		/// <summary>The last read color value.</summary>
		public Color Color { set { color = value; } get { return color; } } [SerializeField] private Color color;

		/// <summary>When a color is read, this event will be invoked.
		/// Color = The color that was read.</summary>
		public ColorEvent OnColor { get { if (onColor == null) onColor = new ColorEvent(); return onColor; } } [SerializeField] private ColorEvent onColor;

		public void HandleHitCoord(bool preview, int priority, float pressure, int seed, P3dHit hit, Quaternion rotation)
		{
			if (preview == true && this.preview == false)
			{
				return;
			}

			var model = hit.Root.GetComponent<P3dModel>();

			if (model != null)
			{
				var paintableTextures = P3dPaintableTexture.FilterAll(model, group);

				for (var i = paintableTextures.Count - 1; i >= 0; i--)
				{
					var paintableTexture = paintableTextures[i];
					var coord            = paintableTexture.GetCoord(ref hit);

					color = P3dCommon.GetPixel(paintableTexture.Current, coord);

					if (onColor != null)
					{
						onColor.Invoke(color);
					}
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dReadColor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dReadColor_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("group", "Only the P3dPaintableTexture components with a matching group will be read by this component.");
			Draw("preview", "Should the color be read during preview painting too?");
			Draw("color", "The last read color value.");

			Separator();

			Draw("onColor");
		}
	}
}
#endif