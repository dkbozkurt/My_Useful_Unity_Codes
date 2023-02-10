using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class allows you to change the painting hardness based on the paint pressure.</summary>
	public class P3dModifyHardnessPressure : P3dModifier
	{
		public enum BlendType
		{
			Replace,
			Multiply,
			Increment
		}

		public static string Group = "Hardness";

		public static string Title = "Pressure";

		/// <summary>The paint component's <b>Hardness</b> value will be modified using this value based on the current <b>Blend</b> setting.</summary>
		public float Hardness { set { hardness = value; } get { return hardness; } } [SerializeField] private float hardness = 1.0f;

		/// <summary>This allows you to control how this new <b>Hardness</b> value will modify the old value in the paint component.
		/// Replace = Transition between [old, new] based on pressure.
		/// Multiply = Transition between [old, old*new] based on pressure.
		/// Increment = Transition between [old, old+new] based on pressure.</summary>
		public BlendType Blend { set { blend = value; } get { return blend; } } [SerializeField] private BlendType blend;

		protected override void OnModifyHardness(ref float currentHardness, float pressure)
		{
			var targetHardness = default(float);

			switch (blend)
			{
				case BlendType.Replace:
				{
					targetHardness = hardness;
				}
				break;

				case BlendType.Multiply:
				{
					targetHardness = currentHardness * hardness;
				}
				break;

				case BlendType.Increment:
				{
					targetHardness = currentHardness + hardness;
				}
				break;
			}

			currentHardness += (targetHardness - currentHardness) * pressure;
		}
#if UNITY_EDITOR
		public override void DrawEditorLayout()
		{
			hardness = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Hardness", "The paint component's Hardness value will be modified using this value based on the current Blend setting."), hardness);
			blend = (BlendType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Blend", "This allows you to control how this new Hardness value will modify the old value in the paint component.\n\nReplace = Transition between [old, new] based on pressure.\n\nMultiply = Transition between [old, old*new] based on pressure.\n\nIncrement = Transition between [old, old+new] based on pressure."), blend);
		}
#endif
	}
}