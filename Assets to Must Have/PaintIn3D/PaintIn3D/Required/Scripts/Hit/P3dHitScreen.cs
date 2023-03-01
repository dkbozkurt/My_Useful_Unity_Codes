using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component will perform a raycast under the mouse or finger as it moves across the screen. It will then send hit events to components like <b>P3dPaintDecal</b>, allowing you to paint the scene.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dHitScreen")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Hit Screen")]
	public class P3dHitScreen : P3dHitScreenBase
	{
		// This stores extra information for each finger unique to this component
		protected class Link : CwInputManager.Link
		{
			public float   Age;
			public bool    Down;
			public int     State;
			public float   Distance;
			public Vector2 ScreenDelta;
			public Vector2 ScreenOld;

			public List<Vector2> History = new List<Vector2>();

			public void Move(Vector2 screenNew)
			{
				if (State == 0)
				{
					ScreenOld = screenNew;
					State     = 1;
				}
				else
				{
					if (TryMove(screenNew) == true || State == 2)
					{
						State += 1;
					}
				}
			}

			private bool TryMove(Vector2 screenNew)
			{
				var threshold = 2.0f;
				var distance  = Vector2.Distance(ScreenOld, screenNew);

				if (distance >= threshold)
				{
					ScreenOld = Vector2.MoveTowards(ScreenOld, screenNew, distance - threshold * 0.5f);
					
					return true;
				}

				return false;
			}

			public override void Clear()
			{
				Age         = 0.0f;
				Down        = false;
				State       = 0;
				Distance    = 0.0f;
				ScreenDelta = Vector2.zero;
				ScreenOld   = Vector2.zero;

				History.Clear();
			}
		}

		public enum FrequencyType
		{
			PixelInterval,
			ScaledPixelInterval,
			TimeInterval,
			OnceOnRelease,
			OnceOnPress,
			OnceEveryFrame
		}

		/// <summary>This allows you to control how often the screen is painted.
		/// PixelInterval = Once every <b>Interval</b> pixels.
		/// ScaledPixelInterval = Like <b>PixelInterval</b>, but scaled to the screen DPI.
		/// TimeInterval = Once every <b>Interval</b> seconds.
		/// OnceOnRelease = When the finger/mouse goes down a preview will be shown, and when it goes up the paint will apply.
		/// OnceOnPress = When the finger/mouse goes down the paint will apply.
		/// OnceEveryFrame = Every frame the paint will apply.</summary>
		public FrequencyType Frequency { set { frequency = value; } get { return frequency; } } [SerializeField] private FrequencyType frequency = FrequencyType.OnceEveryFrame;

		/// <summary>This allows you to set the pixels/seconds between each hit point based on the current <b>Frequency</b> setting.</summary>
		public float Interval { set { interval = value; } get { return interval; } } [SerializeField] private float interval = 10.0f;

		/// <summary>This allows you to connect the hit points together to form lines.</summary>
		public P3dPointConnector Connector { get { if (connector == null) connector = new P3dPointConnector(); return connector; } } [SerializeField] private P3dPointConnector connector;

		[System.NonSerialized]
		private List<Link> links = new List<Link>();

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

		protected override void OnEnable()
		{
			base.OnEnable();

			foreach (var link in links)
			{
				link.Clear();
			}

			Connector.ResetConnections();
		}

		protected virtual void Update()
		{
			connector.Update();
		}

		protected override void HandleFingerUpdate(CwInputManager.Finger finger, bool down, bool up)
		{
			var link = CwInputManager.Link.Find(links, finger);
			var set  = true;

			if (link == null)
			{
				link = CwInputManager.Link.Create(ref links, finger);
			}

			link.Move(finger.ScreenPosition);

			if (finger.Index == CwInputManager.HOVER_FINGER_INDEX && keyPressed == false)
			{
				if (showPreview == true)
				{
					RecordAndPaintAt(link, finger.ScreenPosition, link.ScreenOld, true, 0.0f, this);
				}

				return;
			}

			if (NeedsDrawAngle == true)
			{
				down = link.State == 2;
				set  = link.State >= 2;
			}
			
			if (set == true)
			{
				switch (frequency)
				{
					case FrequencyType.PixelInterval: PaintSmooth(link, down, interval); break;
					case FrequencyType.ScaledPixelInterval: PaintSmooth(link, down, interval / CwInputManager.ScaleFactor); break;
					case FrequencyType.TimeInterval: PaintInterval(link, down); break;
					case FrequencyType.OnceOnRelease: PaintRelease(link, up); break;
					case FrequencyType.OnceOnPress: PaintPress(link, down); break;
					case FrequencyType.OnceEveryFrame: PaintEvery(link, down); break;
				}
			}
		}

		protected override void HandleFingerUp(CwInputManager.Finger finger)
		{
			var link = CwInputManager.Link.Find(links, finger);

			if (link == null)
			{
				link = CwInputManager.Link.Create(ref links, finger);
			}

			connector.BreakHits(link);

			OnFingerUp(link);

			link.Clear();
		}

		private void PaintSmooth(Link link, bool down, float pixelSpacing)
		{
			var head = link.Finger.GetSmoothScreenPosition(0.0f);

			if (down == true || link.History.Count == 0)
			{
				if (storeStates == true)
				{
					P3dStateManager.StoreAllStates();
				}

				RecordAndPaintAt(link, link.Finger.ScreenPosition, link.ScreenOld, false, link.Finger.Pressure, link);
			}

			if (pixelSpacing > 0.0f)
			{
				var steps = Mathf.Max(1, Mathf.FloorToInt(link.Finger.SmoothScreenPositionDelta));
				var step  = CwHelper.Reciprocal(steps);

				for (var i = 0; i <= steps; i++)
				{
					var next = link.Finger.GetSmoothScreenPosition(Mathf.Clamp01(i * step));
					var dist = Vector2.Distance(head, next);
					var gaps = Mathf.FloorToInt((link.Distance + dist) / pixelSpacing);

					for (var j = 0; j < gaps; j++)
					{
						var remainder = pixelSpacing - link.Distance;

						head = Vector2.MoveTowards(head, next, remainder);

						RecordAndPaintAt(link, head, link.History[link.History.Count - 1], false, link.Finger.Pressure, link);

						dist -= remainder;

						link.Distance = 0.0f;
					}

					link.Distance += dist;
					head = next;
				}
			}
		}

		protected virtual void OnFingerUp(Link link)
		{
		}

		private void PaintInterval(Link link, bool down)
		{
			if (down == true)
			{
				if (storeStates == true)
				{
					P3dStateManager.StoreAllStates();
				}

				link.Age = interval;
			}

			link.Age += Time.deltaTime;

			if (link.Age >= interval)
			{
				if (interval > 0.0f)
				{
					link.Age %= interval;
				}
				else
				{
					link.Age = 0.0f;
				}

				RecordAndPaintAt(link, link.Finger.ScreenPosition, link.ScreenOld, false, link.Finger.Pressure, link);
			}
		}

		private void PaintRelease(Link link, bool up)
		{
			var preview = true;

			if (up == true)
			{
				preview = false;

				if (storeStates == true)
				{
					P3dStateManager.StoreAllStates();
				}
			}

			RecordAndPaintAt(link, link.Finger.ScreenPosition, link.ScreenOld, preview, link.Finger.Pressure, link);
		}

		private void PaintPress(Link link, bool down)
		{
			if (down == true)
			{
				if (storeStates == true)
				{
					P3dStateManager.StoreAllStates();
				}

				RecordAndPaintAt(link, link.Finger.ScreenPosition, link.ScreenOld, false, link.Finger.Pressure, link);
			}
		}

		private void PaintEvery(Link link, bool down)
		{
			if (down == true)
			{
				if (storeStates == true)
				{
					P3dStateManager.StoreAllStates();
				}
			}

			RecordAndPaintAt(link, link.Finger.ScreenPosition, link.ScreenOld, false, link.Finger.Pressure, link);
		}

		private void RecordAndPaintAt(Link link, Vector2 screenNew, Vector2 screenOld, bool preview, float pressure, object owner)
		{
			link.History.Add(screenNew);

			PaintAt(connector, connector.HitCache, screenNew, screenOld, preview, pressure, owner);
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dHitScreen;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dHitScreen_Editor : P3dHitScreenBase_Editor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			base.OnInspector();

			Separator();

			DrawBasic();

			Separator();

			DrawAdvancedFoldout();

			Separator();

			var point    = tgt.Emit == P3dHitScreenBase.EmitType.PointsIn3D;
			var line     = tgt.Emit == P3dHitScreenBase.EmitType.PointsIn3D && tgt.Connector.ConnectHits == true;
			var triangle = tgt.Emit == P3dHitScreenBase.EmitType.TrianglesIn3D;
			var coord    = tgt.Emit == P3dHitScreenBase.EmitType.PointsOnUV;

			tgt.Connector.HitCache.Inspector(tgt.gameObject, point: point, line: line, triangle: triangle, coord: coord);
		}

		protected override void DrawBasic()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			base.DrawBasic();

			Draw("frequency", "This allows you to control how often the screen is painted.\n\nPixelInterval = Once every Interval pixels.\n\nScaledPixelInterval = Once every Interval scaled pixels.\n\nTimeInterval = Once every Interval seconds.\n\nOnceOnRelease = When the finger/mouse goes down a preview will be shown, and when it goes up the paint will apply.\n\nOnceOnPress = When the finger/mouse goes down the paint will apply.\n\nOnceEveryFrame = Every frame the paint will apply.");
			if (Any(tgts, t => t.Frequency == P3dHitScreen.FrequencyType.PixelInterval || t.Frequency == P3dHitScreen.FrequencyType.ScaledPixelInterval || t.Frequency == P3dHitScreen.FrequencyType.TimeInterval))
			{
				BeginIndent();
					BeginError(Any(tgts, t => t.Interval <= 0.0f));
						Draw("interval", "This allows you to set the pixels/seconds between each hit point based on the current Frequency setting.");
					EndError();
				EndIndent();
			}
		}

		protected override void DrawAdvanced()
		{
			base.DrawAdvanced();

			Separator();

			P3dPointConnector_Editor.Draw();
		}
	}
}
#endif