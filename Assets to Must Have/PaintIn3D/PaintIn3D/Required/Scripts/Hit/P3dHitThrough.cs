using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component constantly draws lines between the two specified points.</summary>
	[ExecuteInEditMode]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dHitThrough")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Hit Through")]
	public class P3dHitThrough : MonoBehaviour
	{
		public enum PhaseType
		{
			Update,
			FixedUpdate
		}

		public enum OrientationType
		{
			WorldUp,
			CameraUp
		}

		/// <summary>Where in the game loop should this component hit?</summary>
		public PhaseType PaintIn { set { paintIn = value; } get { return paintIn; } } [SerializeField] private PhaseType paintIn;

		/// <summary>The time in seconds between each hit.
		/// 0 = Every frame.
		/// -1 = Manual only.</summary>
		public float Interval { set { interval = value; } get { return interval; } } [SerializeField] private float interval = 0.05f;

		/// <summary>The start point of the raycast.</summary>
		public Transform PointA { set { pointA = value; } get { return pointA; } } [SerializeField] private Transform pointA;

		/// <summary>The end point of the raycast.</summary>
		public Transform PointB { set { pointB = value; } get { return pointB; } } [SerializeField] private Transform pointB;

		/// <summary>How should the hit point be oriented?
		/// WorldUp = It will be rotated to the normal, where the up vector is world up.
		/// CameraUp = It will be rotated to the normal, where the up vector is world up.</summary>
		public OrientationType Orientation { set { orientation = value; } get { return orientation; } } [SerializeField] private OrientationType orientation;

		/// <summary>Orient to a specific camera?
		/// None = MainCamera.</summary>
		public Camera Camera { set { _camera = value; } get { return _camera; } } [SerializeField] private Camera _camera;

		/// <summary>This allows you to control the pressure of the painting. This could be controlled by a VR trigger or similar for more advanced effects.</summary>
		public float Pressure { set { pressure = value; } get { return pressure; } } [Range(0.0f, 1.0f)] [SerializeField] private float pressure = 1.0f;

		/// <summary>Should the applied paint be applied as a preview?</summary>
		public bool Preview { set { preview = value; } get { return preview; } } [SerializeField] private bool preview;

		/// <summary>This allows you to override the order this paint gets applied to the object during the current frame.</summary>
		public int Priority { set { priority = value; } get { return priority; } } [SerializeField] private int priority;

		/// <summary>If you want to draw a line between the start point and the his point then you can set the line here.</summary>
		public LineRenderer Line { set { line = value; } get { return line; } } [SerializeField] private LineRenderer line;

		/// <summary>This allows you to connect the hit points together to form lines.</summary>
		public P3dLineConnector Connector { get { if (connector == null) connector = new P3dLineConnector(); return connector; } } [SerializeField] private P3dLineConnector connector;

		[System.NonSerialized]
		private float current;

		/// <summary>This method will immediately submit a non-preview hit. This can be used to apply real paint to your objects.</summary>
		[ContextMenu("Manually Hit Now")]
		public void ManuallyHitNow()
		{
			SubmitHit(false);
		}

		/// <summary>This component sends hit events to a cached list of components that can receive them. If this list changes then you must manually call this method.</summary>
		[ContextMenu("Clear Hit Cache")]
		public void ClearHitCache()
		{
			Connector.ClearHitCache();
		}

		/// <summary>If this GameObject has teleported and you have <b>ConnectHits</b> or <b>HitSpacing</b> enabled, then you can call this to prevent a line being drawn between the previous and current points.</summary>
		[ContextMenu("Reset Connections")]
		public void ResetConnections()
		{
			connector.ResetConnections();
		}

		protected virtual void OnEnable()
		{
			Connector.ResetConnections();
		}

		protected virtual void Update()
		{
			connector.Update();

			if (preview == true)
			{
				SubmitHit(true);
			}
			else if (paintIn == PhaseType.Update)
			{
				UpdateHit();
			}
		}

		protected virtual void LateUpdate()
		{
			UpdatePointAndLine();
		}

		protected virtual void FixedUpdate()
		{
			if (preview == false && paintIn == PhaseType.FixedUpdate)
			{
				UpdateHit();
			}
		}

		private void SubmitHit(bool preview)
		{
			if (pointA != null && pointB != null)
			{
				var camera    = CwHelper.GetCamera(_camera);
				var positionA = pointA.position;
				var positionB = pointB.position;
				var finalUp   = orientation == OrientationType.CameraUp && camera != null ? camera.transform.up : Vector3.up;
				var vector    = positionB - positionA;
				var rotation  = vector != Vector3.zero ? Quaternion.LookRotation(vector, finalUp) : Quaternion.identity;

				connector.SubmitLine(gameObject, preview, priority, pressure, pointA.position, pointB.position, rotation, this);
			}
		}

		private void UpdateHit()
		{
			current += Time.deltaTime;

			if (interval > 0.0f)
			{
				if (current >= interval)
				{
					current %= interval;

					SubmitHit(false);
				}
			}
			else if (interval == 0.0f)
			{
				SubmitHit(false);
			}
		}

		private void UpdatePointAndLine()
		{
			if (pointA != null && pointB != null)
			{
				var a = pointA.position;
				var b = pointB.position;

				if (line != null)
				{
					line.positionCount = 2;

					line.SetPosition(0, a);
					line.SetPosition(1, b);
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dHitThrough;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dHitThrough_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			BeginDisabled(true);
				EditorGUILayout.TextField("Emit", "Lines In 3D", EditorStyles.popup);
			EndDisabled();

			Draw("paintIn", "Where in the game loop should this component hit?");
			Draw("interval", "The time in seconds between each hit.\n\n0 = Every frame.\n\n-1 = Manual only.");

			Separator();

			BeginError(Any(tgts, t => t.PointA == null));
				Draw("pointA", "The start point of the raycast.");
			EndError();
			BeginError(Any(tgts, t => t.PointB == null));
				Draw("pointB", "The end point of the raycast.");
			EndError();
			Draw("orientation", "How should the hit point be oriented?\n\nWorldUp = It will be rotated to the normal, where the up vector is world up.\n\nCameraUp = It will be rotated to the normal, where the up vector is world up.");
			BeginIndent();
				if (Any(tgts, t => t.Orientation == P3dHitThrough.OrientationType.CameraUp))
				{
					Draw("_camera", "Orient to a specific camera?\nNone = MainCamera.");
				}
			EndIndent();

			Separator();

			Draw("preview", "Should the applied paint be applied as a preview?");
			Draw("pressure", "This allows you to control the pressure of the painting. This could be controlled by a VR trigger or similar for more advanced effects.");

			Separator();

			if (DrawFoldout("Advanced", "Show advanced settings?") == true)
			{
				BeginIndent();
					P3dLineConnector_Editor.Draw();

					Separator();
					
					Draw("priority", "This allows you to override the order this paint gets applied to the object during the current frame.");
					Draw("line", "If you want to draw a line between the start point and the his point then you can set the line here");
				EndIndent();
			}

			Separator();

			var line = true;
			var quad = tgt.Connector.ConnectHits == true;

			tgt.Connector.HitCache.Inspector(tgt.gameObject, line: line, quad: quad);
		}
	}
}
#endif