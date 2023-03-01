using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component implements the fill paint mode, which will modify all pixels in the specified texture in the same way.
	/// This is useful if you want to gradually fade a texture to a specific color.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dPaintFill")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Paint Fill")]
	public class P3dPaintFill : MonoBehaviour, IHitCoord
	{
		/// <summary>Only the <b>P3dPaintableTexture</b> components with a matching group will be painted by this component.</summary>
		public P3dGroup Group { set { group = value; } get { return group; } } [SerializeField] private P3dGroup group;

		/// <summary>This allows you to choose how the paint from this component will combine with the existing pixels of the textures you paint.
		/// NOTE: See the <b>Blend Mode</b> section of the documentation for more information.</summary>
		public P3dBlendMode BlendMode { set { blendMode = value; } get { return blendMode; } } [SerializeField] private P3dBlendMode blendMode = P3dBlendMode.AlphaBlend(Vector4.one);

		/// <summary>The color of the paint.</summary>
		public Texture Texture { set { texture = value; } get { return texture; } } [SerializeField] private Texture texture;

		/// <summary>The color of the paint.</summary>
		public Color Color { set { color = value; } get { return color; } } [SerializeField] private Color color = Color.white;

		/// <summary>The opacity of the brush.</summary>
		public float Opacity { set { opacity = value; } get { return opacity; } } [Range(0.0f, 1.0f)] [SerializeField] private float opacity = 1.0f;

		/// <summary>The minimum RGBA value change. This is useful if you're doing very subtle color changes over time.</summary>
		public float Minimum { set { minimum = value; } get { return minimum; } } [Range(0.0f, 1.0f)] [SerializeField] private float minimum;

		/// <summary>This stores a list of all modifiers used to change the way this component applies paint (e.g. <b>P3dModifyColorRandom</b>).</summary>
		public P3dModifierList Modifiers { get { if (modifiers == null) modifiers = new P3dModifierList(); return modifiers; } } [SerializeField] private P3dModifierList modifiers;

		/// <summary>This method increments <b>Opacity</b> by the specified value.</summary>
		public void IncrementOpacity(float delta)
		{
			opacity = Mathf.Clamp01(opacity + delta);
		}

		public void HandleHitCoord(bool preview, int priority, float pressure, int seed, P3dHit hit, Quaternion rotation)
		{
			var model = hit.Root.GetComponentInParent<P3dModel>();

			if (model != null)
			{
				var paintableTextures = P3dPaintableTexture.FilterAll(model, group);

				if (paintableTextures.Count > 0)
				{
					var finalColor   = color;
					var finalOpacity = opacity;
					var finalTexture = texture;

					if (modifiers != null && modifiers.Count > 0)
					{
						CwHelper.BeginSeed(seed);
							modifiers.ModifyColor(ref finalColor, preview, pressure);
							modifiers.ModifyOpacity(ref finalOpacity, preview, pressure);
							modifiers.ModifyTexture(ref finalTexture, preview, pressure);
						CwHelper.EndSeed();
					}

					P3dCommandFill.Instance.SetState(preview, priority);
					P3dCommandFill.Instance.SetMaterial(blendMode, finalTexture, finalColor, opacity, minimum);

					for (var i = paintableTextures.Count - 1; i >= 0; i--)
					{
						var paintableTexture = paintableTextures[i];

						P3dPaintableManager.Submit(P3dCommandFill.Instance, model, paintableTexture);
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
	using TARGET = P3dPaintFill;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dPaintFill_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("group", "Only the P3dPaintableTexture components with a matching group will be painted by this component.");

			Separator();

			Draw("blendMode", "This allows you to choose how the paint from this component will combine with the existing pixels of the textures you paint.\n\nNOTE: See the Blend Mode section of the documentation for more information.");
			Draw("texture", "The texture of the paint.");
			Draw("color", "The color of the paint.");
			Draw("opacity", "The opacity of the brush.");
			Draw("minimum", "The minimum RGBA value change. This is useful if you're doing very subtle color changes over time.");

			Separator();

			tgt.Modifiers.DrawEditorLayout(serializedObject, target, "Color", "Opacity", "Texture");
		}
	}
}
#endif