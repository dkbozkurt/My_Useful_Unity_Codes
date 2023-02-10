using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class allows you to change the painting radius based on the paint pressure.</summary>
	[System.Serializable]
	public class P3dModifyRadiusPressure : P3dModifier
	{
		public enum BlendType
		{
			Replace,
			Multiply,
			Increment
		}

		public static string Group = "Radius";

		public static string Title = "Pressure";

		/// <summary>The paint component's <b>Radius</b> value will be modified using this value based on the current <b>Blend</b> setting.</summary>
		public float Radius { set { radius = value; } get { return radius; } } [SerializeField] private float radius = 1.0f;

		/// <summary>This allows you to control how this new <b>Radius</b> value will modify the old value in the paint component.
		/// Replace = Transition between [old, new] based on pressure.
		/// Multiply = Transition between [old, old*new] based on pressure.
		/// Increment = Transition between [old, old+new] based on pressure.</summary>
		public BlendType Blend { set { blend = value; } get { return blend; } } [SerializeField] private BlendType blend;

		protected override void OnModifyRadius(ref float currentRadius, float pressure)
		{
			var targetRadius = default(float);

			switch (blend)
			{
				case BlendType.Replace:
				{
					targetRadius = radius;
				}
				break;

				case BlendType.Multiply:
				{
					targetRadius = currentRadius * radius;
				}
				break;

				case BlendType.Increment:
				{
					targetRadius = currentRadius + radius;
				}
				break;
			}

			currentRadius += (targetRadius - currentRadius) * pressure;
		}

#if UNITY_EDITOR
		public override void DrawEditorLayout()
		{
			radius = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Radius", "The paint component's Radius value will be modified using this value based on the current Blend setting."), radius);
			blend = (BlendType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Blend", "This allows you to control how this new Radius value will modify the old value in the paint component.\n\nReplace = Transition between [old, new] based on pressure.\n\nMultiply = Transition between [old, old*new] based on pressure.\n\nIncrement = Transition between [old, old+new] based on pressure."), blend);
		}
#endif
	}
}