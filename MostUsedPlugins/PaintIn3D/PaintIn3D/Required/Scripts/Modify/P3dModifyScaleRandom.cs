using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class allows you to randomize the painting scale of the attached component (e.g. P3dPaintDecal).</summary>
	[System.Serializable]
	public class P3dModifyScaleRandom : P3dModifier
	{
		public enum BlendType
		{
			Replace,
			Multiply,
			Increment
		}

		public static string Group = "Scale";

		public static string Title = "Random";

		/// <summary>This is the minimum random scale that will be picked.</summary>
		public Vector3 Min { set { min = value; } get { return min; } } [SerializeField] private Vector3 min = new Vector3(0.6666f, 0.6666f, 0.6666f);

		/// <summary>This is the maximum random scale that will be picked.</summary>
		public Vector3 Max { set { max = value; } get { return max; } } [SerializeField] private Vector3 max = new Vector3(1.5f, 1.5f, 1.5f);

		/// <summary>The way the picked scale value will be blended with the current one.</summary>
		public BlendType Blend { set { blend = value; } get { return blend; } } [SerializeField] private BlendType blend;

		/// <summary>If you disable this then each x, y, and z value will be scaled separately.</summary>
		public bool Uniform { set { uniform = value; } get { return uniform; } } [SerializeField] private bool uniform;

		protected override void OnModifyScale(ref Vector3 scale, float pressure)
		{
			Vector3 pickedScale;

			if (uniform == true)
			{
				pickedScale = Vector3.LerpUnclamped(min, max, Random.value);
			}
			else
			{
				pickedScale.x = Mathf.LerpUnclamped(min.x, max.x, Random.value);
				pickedScale.y = Mathf.LerpUnclamped(min.y, max.y, Random.value);
				pickedScale.z = Mathf.LerpUnclamped(min.z, max.z, Random.value);
			}

			switch (blend)
			{
				case BlendType.Replace:
				{
					scale = pickedScale;
				}
				break;

				case BlendType.Multiply:
				{
					scale.x *= pickedScale.x;
					scale.y *= pickedScale.y;
					scale.z *= pickedScale.z;
				}
				break;

				case BlendType.Increment:
				{
					scale += pickedScale;
				}
				break;
			}
		}

#if UNITY_EDITOR
		public override void DrawEditorLayout()
		{
			min = UnityEditor.EditorGUILayout.Vector3Field(new GUIContent("Min", "This is the minimum random scale that will be picked."), min);
			max = UnityEditor.EditorGUILayout.Vector3Field(new GUIContent("Max", "This is the maximum random scale that will be picked."), max);
			blend = (BlendType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Blend", "The way the picked scale value will be blended with the current one."), blend);
			uniform = UnityEditor.EditorGUILayout.Toggle(new GUIContent("uniform", "If you disable this then each x, y, and z value will be scaled separately."), uniform);
		}
#endif
	}
}