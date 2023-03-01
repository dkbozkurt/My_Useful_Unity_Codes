using UnityEngine;
using System.Reflection;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component modifies <b>P3dHitScreen</b> to work in the editor <b>SceneView</b> without colliders.</summary>
	[AddComponentMenu("")]
	public class P3dHitScreen_InEditor : P3dHitScreen
	{
#if UNITY_EDITOR
		private static MethodInfo method_IntersectRayMesh;

		private CwInputManager.Finger virtualHover = new CwInputManager.Finger();

		private CwInputManager.Finger virtualPaint = new CwInputManager.Finger();

		private bool mouseSet;

		static P3dHitScreen_InEditor() 
		{
			method_IntersectRayMesh = typeof(UnityEditor.HandleUtility).GetMethod("IntersectRayMesh", BindingFlags.Static | BindingFlags.NonPublic);
		}

		protected override void DoQuery(Vector2 screenPosition, ref Camera camera, ref Ray ray, ref RaycastHit hit3D, ref RaycastHit2D hit2D)
		{
			camera = P3dSceneTool.LastCamera;
			ray    = P3dSceneTool.GetRay(screenPosition);

			var bestDistance = float.PositiveInfinity;

			foreach (var model in P3dModel.Instances)
			{
				var hit    = default(RaycastHit);
				var mesh   = default(Mesh);
				var matrix = default(Matrix4x4);

				model.GetPrepared(ref mesh, ref matrix, P3dCoord.First);

				if (IntersectRayMesh(ray, mesh, matrix, out hit) == true)
				{
					if (hit.distance < bestDistance)
					{
						bestDistance = hit.distance;

						hit3D = hit;
					}
				}
			}
		}

		protected override void LateUpdate()
		{
			fingers.Clear();

			Connector.ClearHitCache();

			var screenPosition = P3dSceneTool.LastPosition;

			if (P3dSceneTool.LastSet == true || mouseSet == true)
			{
				virtualPaint.Index = CwInputManager.MOUSE_FINGER_INDEX;
				virtualPaint.Down  = mouseSet == false;
				virtualPaint.Up    = P3dSceneTool.LastSet == false;

				if (virtualPaint.Down == true)
				{
					virtualPaint.Age                     = 0.0f;
					virtualPaint.StartScreenPosition     = screenPosition;
					virtualPaint.ScreenPositionOld       = screenPosition;
					virtualPaint.ScreenPositionOldOld    = screenPosition;
					virtualPaint.ScreenPositionOldOldOld = screenPosition;
				}
				else
				{
					virtualPaint.Age                     += Time.deltaTime;
					virtualPaint.ScreenPositionOldOldOld  = virtualPaint.ScreenPositionOldOld;
					virtualPaint.ScreenPositionOldOld     = virtualPaint.ScreenPositionOld;
					virtualPaint.ScreenPositionOld        = virtualPaint.ScreenPosition;
				}

				virtualPaint.Pressure       = P3dSceneTool.LastPressure;
				virtualPaint.ScreenPosition = screenPosition;

				mouseSet = P3dSceneTool.LastSet;

				fingers.Add(virtualPaint);
			}

			virtualHover.Index          = CwInputManager.HOVER_FINGER_INDEX;
			virtualHover.ScreenPosition = screenPosition;

			fingers.Add(virtualHover);

			base.LateUpdate();
		}

		public static bool IntersectRayMesh(Ray ray, MeshFilter meshFilter, out RaycastHit hit)
		{
			return IntersectRayMesh(ray, meshFilter.mesh, meshFilter.transform.localToWorldMatrix, out hit);
		}

		public static bool IntersectRayMesh(Ray ray, Mesh mesh, Matrix4x4 matrix, out RaycastHit hit)
		{
			var parameters = new object[] { ray, mesh, matrix, null };
			var result     = (bool)method_IntersectRayMesh.Invoke(null, parameters);

			hit = (RaycastHit)parameters[3];

			return result;
		}
#else
		protected override void DoQuery(Vector2 screenPosition, ref Camera camera, ref Ray ray, ref RaycastHit hit3D, ref RaycastHit2D hit2D)
		{
		}

		protected override void LateUpdate()
		{
		}
#endif
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dHitScreen_InEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dHitScreen_InEditor_Editor : P3dHitScreen_Editor
	{
		protected override void OnInspector()
		{
			base.OnInspector();
		}
	}
}
#endif