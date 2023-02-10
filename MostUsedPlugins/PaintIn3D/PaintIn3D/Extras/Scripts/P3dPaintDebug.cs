using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to debug hit points into the <b>Scene</b> tab. Hit points will automatically be sent by any <b>P3dHit___</b> component on this GameObject, or its ancestors.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dPaintDebug")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Paint Debug")]
	public class P3dPaintDebug : MonoBehaviour, IHitPoint, IHitLine, IHitTriangle, IHitQuad
	{
		/// <summary>The color of the debug.</summary>
		public Color Color { set { color = value; } get { return color; } } [SerializeField] private Color color = Color.white;

		/// <summary>The duration of the debug.</summary>
		public float Duration { set { duration = value; } get { return duration; } } [SerializeField] private float duration = 0.05f;

		/// <summary>The size of the debug.</summary>
		public float Size { set { size = value; } get { return size; } } [SerializeField] private float size = 0.05f;

		public void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			var tint = GetColor(preview, pressure, color);
			var back = position + rotation * new Vector3(0.0f, 0.0f, -size);

			DrawArrow(position, rotation, tint);

			Debug.DrawLine(position, back, tint, duration);
		}

		public void HandleHitLine(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip)
		{
			var tint = GetColor(preview, pressure, color);

			DrawArrow(endPosition, rotation, tint);

			Debug.DrawLine(position, endPosition, tint, duration);
		}

		public void HandleHitTriangle(bool preview, int priority, float pressure, int seed, Vector3 positionA, Vector3 positionB, Vector3 positionC, Quaternion rotation)
		{
			var tint = GetColor(preview, pressure, color);

			DrawArrow(positionA, rotation, tint);
			DrawArrow(positionB, rotation, tint);
			DrawArrow(positionC, rotation, tint);

			Debug.DrawLine(positionA, positionB, tint, duration);
			Debug.DrawLine(positionB, positionC, tint, duration);
			Debug.DrawLine(positionC, positionA, tint, duration);
		}

		public void HandleHitQuad(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2, Quaternion rotation, bool clip)
		{
			var tint = GetColor(preview, pressure, color);

			DrawArrow(endPosition, rotation, tint);
			DrawArrow(endPosition2, rotation, tint);

			Debug.DrawLine(position, endPosition, tint, duration);
			Debug.DrawLine(position2, endPosition2, tint, duration);
			Debug.DrawLine(position, position2, tint, duration);
			Debug.DrawLine(endPosition, endPosition2, tint, duration);
		}

		private Color GetColor(bool preview, float pressure, Color color)
		{
			if (preview == true)
			{
				color.a *= 0.5f;
			}

			color.a *= pressure * 0.75f + 0.25f;

			return color;
		}

		private void DrawArrow(Vector3 position, Quaternion rotation, Color tint)
		{
			var cornerA = position + rotation * new Vector3(-size, -size);
			var cornerB = position + rotation * new Vector3(-size, +size);
			var cornerC = position + rotation * new Vector3(+size, +size);
			var cornerD = position + rotation * new Vector3(+size, -size);

			Debug.DrawLine(cornerA, cornerB, tint, duration);
			Debug.DrawLine(cornerB, cornerC, tint, duration);
			Debug.DrawLine(cornerC, cornerD, tint, duration);
			Debug.DrawLine(cornerD, cornerA, tint, duration);

			var front = position + rotation * new Vector3(0.0f, 0.0f, +size);

			Debug.DrawLine(front, cornerA, tint, duration);
			Debug.DrawLine(front, cornerB, tint, duration);
			Debug.DrawLine(front, cornerC, tint, duration);
			Debug.DrawLine(front, cornerD, tint, duration);
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dPaintDebug;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dPaintDebug_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("color", "The color of the debug.");
			BeginError(Any(tgts, t => t.Duration <= 0.0f));
				Draw("duration", "The duration of the debug.");
			EndError();
			BeginError(Any(tgts, t => t.Size <= 0.0f));
				Draw("size", "The size of the debug.");
			EndError();
		}
	}
}
#endif