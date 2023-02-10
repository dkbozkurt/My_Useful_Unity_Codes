using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class allows you to randomize the painting radius of the attached component (e.g. P3dPaintDecal).</summary>
	[System.Serializable]
	public class P3dModifyRadiusRandom : P3dModifier
	{
		public enum BlendType
		{
			Replace,
			Multiply,
			Increment
		}

		public static string Group = "Radius";

		public static string Title = "Random";

		/// <summary>This is the minimum random radius that will be picked.</summary>
		public float Min { set { min = value; } get { return min; } } [SerializeField] private float min = 0.6666f;

		/// <summary>This is the maximum random radius that will be picked.</summary>
		public float Max { set { max = value; } get { return max; } } [SerializeField] private float max = 1.5f;

		/// <summary>The way the picked radius value will be blended with the current one.</summary>
		public BlendType Blend { set { blend = value; } get { return blend; } } [SerializeField] private BlendType blend;

		protected override void OnModifyRadius(ref float radius, float pressure)
		{
			var pickedRadius = Random.Range(min, max);

			switch (blend)
			{
				case BlendType.Replace:
				{
					radius = pickedRadius;
				}
				break;

				case BlendType.Multiply:
				{
					radius *= pickedRadius;
				}
				break;

				case BlendType.Increment:
				{
					radius += pickedRadius;
				}
				break;
			}
		}

#if UNITY_EDITOR
		public override void DrawEditorLayout()
		{
			min = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Min", "This is the minimum random radius that will be picked."), min);
			max = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Max", "This is the maximum random radius that will be picked."), max);
			blend = (BlendType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Blend", "The way the picked radius value will be blended with the current one."), blend);
		}
#endif
	}
}