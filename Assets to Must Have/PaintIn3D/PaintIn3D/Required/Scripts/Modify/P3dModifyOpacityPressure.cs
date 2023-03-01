using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class allows you to change the painting opacity based on the paint pressure.</summary>
	[System.Serializable]
	public class P3dModifyOpacityPressure : P3dModifier
	{
		public enum BlendType
		{
			Replace,
			Multiply,
			Increment
		}

		public static string Group = "Opacity";

		public static string Title = "Pressure";

		/// <summary>The paint component's <b>Opacity</b> value will be modified using this value based on the current <b>Blend</b> setting.</summary>
		public float Opacity { set { opacity = value; } get { return opacity; } } [SerializeField] private float opacity = 1.0f;

		/// <summary>This allows you to control how this new <b>Opacity</b> value will modify the old value in the paint component.
		/// Replace = Transition between [old, new] based on pressure.
		/// Multiply = Transition between [old, old*new] based on pressure.
		/// Increment = Transition between [old, old+new] based on pressure.</summary>
		public BlendType Blend { set { blend = value; } get { return blend; } } [SerializeField] private BlendType blend;

		protected override void OnModifyOpacity(ref float currentOpacity, float pressure)
		{
			var targetOpacity = default(float);

			switch (blend)
			{
				case BlendType.Replace:
				{
					targetOpacity = opacity;
				}
				break;

				case BlendType.Multiply:
				{
					targetOpacity = currentOpacity * opacity;
				}
				break;

				case BlendType.Increment:
				{
					targetOpacity = currentOpacity + opacity;
				}
				break;
			}

			currentOpacity += (targetOpacity - currentOpacity) * pressure;
		}

#if UNITY_EDITOR
		public override void DrawEditorLayout()
		{
			opacity = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Opacity", "The paint component's Opacity value will be modified using this value based on the current Blend setting."), opacity);
			blend = (BlendType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Blend", "This allows you to control how this new Opacity value will modify the old value in the paint component.\n\nReplace = Transition between [old, new] based on pressure.\n\nMultiply = Transition between [old, old*new] based on pressure.\n\nIncrement = Transition between [old, old+new] based on pressure."), blend);
		}
#endif
	}
}