using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component continuously fires hit events using the current Transform position.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dHitNearby")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Hit Nearby")]
	public class P3dHitNearby : MonoBehaviour
	{
		public enum PhaseType
		{
			ManuallyOnly = -1,
			Update,
			FixedUpdate
		}

		/// <summary>Where in the game loop should this component hit?</summary>
		public PhaseType PaintIn { set { paintIn = value; } get { return paintIn; } } [SerializeField] private PhaseType paintIn;

		/// <summary>The time in seconds between each hit.
		/// 0 = Every frame.</summary>
		public float Interval { set { interval = value; } get { return interval; } } [SerializeField] private float interval = 0.05f;

		/// <summary>Should the applied paint be applied as a preview?</summary>
		public bool Preview { set { preview = value; } get { return preview; } } [SerializeField] private bool preview;

		/// <summary>This allows you to override the order this paint gets applied to the object during the current frame.</summary>
		public int Priority { set { priority = value; } get { return priority; } } [SerializeField] private int priority;

		/// <summary>This allows you to control the pressure of the painting. This could be controlled by a VR trigger or similar for more advanced effects.</summary>
		public float Pressure { set { pressure = value; } get { return pressure; } } [Range(0.0f, 1.0f)] [SerializeField] private float pressure = 1.0f;

		/// <summary>This allows you to connect the hit points together to form lines.</summary>
		public P3dPointConnector Connector { get { if (connector == null) connector = new P3dPointConnector(); return connector; } } [SerializeField] private P3dPointConnector connector;

		[System.NonSerialized]
		private float current;

		[SerializeField]
		private Vector3 lastPosition;

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

		protected virtual void FixedUpdate()
		{
			if (preview == false && paintIn == PhaseType.FixedUpdate)
			{
				UpdateHit();
			}
		}

		private void SubmitHit(bool preview)
		{
			connector.SubmitPoint(gameObject, preview, priority, pressure, transform.position, transform.rotation, this);
		}

		private void UpdateHit()
		{
			current += Time.inFixedTimeStep == true ? Time.fixedDeltaTime : Time.deltaTime;

			if (interval > 0.0f)
			{
				if (current >= interval)
				{
					current %= interval;

					SubmitHit(false);
				}
			}
			else
			{
				SubmitHit(false);
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dHitNearby;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dHitNearby_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			BeginDisabled(true);
				EditorGUILayout.TextField("Emit", "Points In 3D", EditorStyles.popup);
			EndDisabled();

			Draw("paintIn", "Where in the game loop should this component hit?");
			if (Any(tgts, t => t.PaintIn != TARGET.PhaseType.ManuallyOnly))
			{
				Draw("interval", "The time in seconds between each hit.\n\n0 = Every frame.");
			}

			Separator();

			Draw("preview", "Should the applied paint be applied as a preview?");
			Draw("pressure", "This allows you to control the pressure of the painting. This could be controlled by a VR trigger or similar for more advanced effects.");

			Separator();

			if (DrawFoldout("Advanced", "Show advanced settings?") == true)
			{
				BeginIndent();
					P3dPointConnector_Editor.Draw();

					Separator();

					Draw("priority", "This allows you to override the order this paint gets applied to the object during the current frame.");
				EndIndent();
			}

			Separator();

			var point = true;
			var line  = tgt.Connector.ConnectHits == true;

			tgt.Connector.HitCache.Inspector(tgt.gameObject, point: point, line: line);
		}
	}
}
#endif