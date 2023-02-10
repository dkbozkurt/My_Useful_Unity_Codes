using UnityEngine;

namespace PaintIn3D
{
	/// <summary>This class allows you to randomize the painting position of the attached component (e.g. P3dPaintDecal).</summary>
	[System.Serializable]
	public class P3dModifyPositionRandom : P3dModifier
	{
		public static string Group = "Position";

		public static string Title = "Random";

		/// <summary>The position will be offset up to this radius away in world space.</summary>
		public float Radius { set { radius = value; } get { return radius; } } [SerializeField] private float radius = 1.0f;

		protected override void OnModifyPosition(ref Vector3 position, float pressure)
		{
			position += Random.insideUnitSphere * radius;
		}
#if UNITY_EDITOR
		public override void DrawEditorLayout()
		{
			radius = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Radius", "The position will be offset up to this radius away in world space."), radius);
		}
#endif
	}
}