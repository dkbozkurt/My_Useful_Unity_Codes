using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class allows you to randomize the painting angle of the attached component (e.g. P3dPaintDecal).</summary>
	[System.Serializable]
	public class P3dModifyAngleRandom : P3dModifier
	{
		public enum BlendType
		{
			Replace,
			Multiply,
			Increment
		}

		public static string Group = "Angle";

		public static string Title = "Random";

		/// <summary>This is the minimum random angle that will be picked.</summary>
		public float Min { set { min = value; } get { return min; } } [SerializeField] private float min = -180.0f;

		/// <summary>This is the maximum random angle that will be picked.</summary>
		public float Max { set { max = value; } get { return max; } } [SerializeField] private float max = 180.0f;

		/// <summary>The way the picked angle value will be blended with the current one.</summary>
		public BlendType Blend { set { blend = value; } get { return blend; } } [SerializeField] private BlendType blend;

		protected override void OnModifyAngle(ref float angle, float pressure)
		{
			var pickedAngle = Random.Range(min, max);

			switch (blend)
			{
				case BlendType.Replace:
				{
					angle = pickedAngle;
				}
				break;

				case BlendType.Multiply:
				{
					angle *= pickedAngle;
				}
				break;

				case BlendType.Increment:
				{
					angle += pickedAngle;
				}
				break;
			}
		}
#if UNITY_EDITOR
		public override void DrawEditorLayout()
		{
			min = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Min", "This is the minimum random angle that will be picked."), min);
			max = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Max", "This is the maximum random angle that will be picked."), max);
			blend = (BlendType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Blend", "The way the picked angle value will be blended with the current one."), blend);
		}
#endif
	}
}