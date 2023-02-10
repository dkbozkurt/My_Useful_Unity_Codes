using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class allows you to randomize the painting color of the attached component (e.g. P3dPaintDecal).</summary>
	[System.Serializable]
	public class P3dModifyColorRandom : P3dModifier
	{
		public enum BlendType
		{
			Replace,
			Multiply,
			Increment
		}

		public static string Group = "Color";

		public static string Title = "Random";

		/// <summary>This is the gradient containing all the possible colors. A color will be randomly picked from this.</summary>
		public Gradient Gradient { get { if (gradient == null) gradient = new Gradient(); return gradient; } } [SerializeField] private Gradient gradient;

		/// <summary>The way the picked color value will be blended with the current one.</summary>
		public BlendType Blend { set { blend = value; } get { return blend; } } [SerializeField] private BlendType blend;

		protected override void OnModifyColor(ref Color color, float pressure)
		{
			if (gradient != null)
			{
				var pickedColor = gradient.Evaluate(Random.value);

				switch (blend)
				{
					case BlendType.Replace:
					{
						color = pickedColor;
					}
					break;

					case BlendType.Multiply:
					{
						color *= pickedColor;
					}
					break;

					case BlendType.Increment:
					{
						color += pickedColor;
					}
					break;
				}
			}
		}

#if UNITY_EDITOR
		public override void DrawEditorLayout()
		{
			gradient = UnityEditor.EditorGUILayout.GradientField(new GUIContent("Gradient", "This is the gradient containing all the possible colors. A color will be randomly picked from this."), Gradient);
			blend = (BlendType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Blend", "The way the picked color value will be blended with the current one."), blend);
		}
#endif
	}
}