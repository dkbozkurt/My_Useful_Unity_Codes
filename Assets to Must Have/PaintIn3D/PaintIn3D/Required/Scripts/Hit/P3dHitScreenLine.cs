using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component will perform a raycast under the mouse or finger as it moves across the screen. It will then send hit events to components like <b>P3dPaintDecal</b>, allowing you to paint the scene.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dHitScreenLine")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Hit Screen Line")]
	public class P3dHitScreenLine : P3dHitScreenBase
	{
		public enum FrequencyType
		{
			StartAndEnd,
			PixelInterval,
			ScaledPixelInterval,
			StretchedPixelInterval,
			StretchedScaledPixelInterval,
			Once
		}

		/// <summary>This allows you to control how many hit points will be generated along the drawn line.
		/// StartAndEnd = Once at the start, and once at the end.
		/// PixelInterval = Once at the start, and then every <b>Interval</b> pixels.
		/// ScaledPixelInterval = Once at the start, and then every <b>Interval</b> scaled pixels.
		/// StretchedPixelInterval = Like <b>ScaledPixelInterval</b>, but the hits are stretched to reach the end.
		/// StretchedScaledPixelInterval = Like <b>ScaledPixelInterval</b>, but the hits are stretched to reach the end.
		/// Once = Once at the specified <b>Position</b> and <b>PixelOffset</b> along the line.</summary>
		public FrequencyType Frequency { set { frequency = value; } get { return frequency; } } [SerializeField] private FrequencyType frequency = FrequencyType.PixelInterval;

		/// <summary>This allows you to set the pixels between each hit point based on the current <b>Frequency</b> setting.</summary>
		public float Interval { set { interval = value; } get { return interval; } } [SerializeField] private float interval = 10.0f;

		/// <summary>When using <b>Frequency = Once</b>, this allows you to set the 0..1 position along the line.</summary>
		public float Position { set { position = value; } get { return position; } } [SerializeField] [Range(0.0f, 1.0f)] private float position;

		/// <summary>When using <b>Frequency = Once</b>, this allows you to set the pixel offset along the line.</summary>
		public float PixelOffset { set { pixelOffset = value; } get { return pixelOffset; } } [SerializeField] private float pixelOffset;

		/// <summary>This allows you to connect the hit points together to form lines.</summary>
		public P3dPointConnector Connector { get { if (connector == null) connector = new P3dPointConnector(); return connector; } } [SerializeField] private P3dPointConnector connector;

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

			Connector.ResetConnections();
		}

		protected virtual void Update()
		{
			connector.Update();
		}

		protected override void HandleFingerUpdate(CwInputManager.Finger finger, bool down, bool up)
		{
			if (finger.Index == CwInputManager.HOVER_FINGER_INDEX)
			{
				return;
			}

			if (up == true)
			{
				if (storeStates == true)
				{
					P3dStateManager.StoreAllStates();
				}
			}

			switch (frequency)
			{
				case FrequencyType.StartAndEnd: PaintStartEnd(finger, up); break;
				case FrequencyType.PixelInterval: PaintStartInterval(finger, up, interval, false); break;
				case FrequencyType.ScaledPixelInterval: PaintStartInterval(finger, up, interval / CwInputManager.ScaleFactor, false); break;
				case FrequencyType.StretchedPixelInterval: PaintStartInterval(finger, up, interval, true); break;
				case FrequencyType.StretchedScaledPixelInterval: PaintStartInterval(finger, up, interval / CwInputManager.ScaleFactor, true); break;
				case FrequencyType.Once: PaintOne(finger, up, position, pixelOffset); break;
			}

			connector.BreakHits(finger);
		}

		private void PaintStartEnd(CwInputManager.Finger finger, bool up)
		{
			var preview = up == false;
			var pointS  = finger.StartScreenPosition;
			var pointE  = finger.ScreenPosition;
			var pointV  = pointE - pointS;

			PaintAt(connector, connector.HitCache, pointS, pointS - pointV, preview, finger.Pressure, finger);
			PaintAt(connector, connector.HitCache, pointE, pointE - pointV, preview, finger.Pressure, finger);
		}

		private void PaintStartInterval(CwInputManager.Finger finger, bool up, float pixelSpacing, bool stretch)
		{
			var preview = up == false;
			var pointS  = finger.StartScreenPosition;
			var pointE  = finger.ScreenPosition;
			var pointV  = pointE - pointS;
			var pointM  = pointV.magnitude;
			var steps   = 0;

			if (pixelSpacing > 0.0f)
			{
				steps = Mathf.FloorToInt(pointM / pixelSpacing);

				if (stretch == true && steps > 0)
				{
					pixelSpacing = pointM / steps;
				}
			}

			for (var i = 0; i <= steps; i++)
			{
				PaintAt(connector, connector.HitCache, pointS, pointS - pointV, preview, finger.Pressure, finger);

				pointS = Vector2.MoveTowards(pointS, pointE, pixelSpacing);
			}
		}

		private void PaintOne(CwInputManager.Finger finger, bool up, float frac, float pixelOff)
		{
			var preview = up == false;
			var pointS  = finger.StartScreenPosition;
			var pointE  = finger.ScreenPosition;
			var pointV  = pointE - pointS;
			var pointN  = pointV.normalized;

			pointS += pointV * frac + pointN * pixelOff;

			PaintAt(connector, connector.HitCache, pointS, pointS - pointV, preview, finger.Pressure, finger);
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dHitScreenLine;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dHitScreenLine_Editor : P3dHitScreenBase_Editor
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

			Draw("frequency", "This allows you to control how many hit points will be generated along the drawn line.\n\nStartAndEnd = Once at the start, and once at the end.\n\nPixelInterval = Once at the start, and then every <b>Interval</b> pixels.\n\nScaledPixelInterval = Once at the start, and then every <b>Interval</b> scaled pixels.\n\nStretchedPixelInterval = Like <b>ScaledPixelInterval</b>, but the hits are stretched to reach the end.\n\nStretchedScaledPixelInterval = Like <b>ScaledPixelInterval</b>, but the hits are stretched to reach the end.\n\nOnce = Once at the specified <b>Position</b> and <b>PixelOffset</b> along the line.");
			BeginIndent();
				if (Any(tgts, t => t.Frequency == P3dHitScreenLine.FrequencyType.PixelInterval || t.Frequency == P3dHitScreenLine.FrequencyType.ScaledPixelInterval || t.Frequency == P3dHitScreenLine.FrequencyType.StretchedPixelInterval || t.Frequency == P3dHitScreenLine.FrequencyType.StretchedScaledPixelInterval))
				{
					BeginError(Any(tgts, t => t.Interval <= 0.0f));
						Draw("interval", "This allows you to set the pixels/seconds between each hit point based on the current Frequency setting.");
					EndError();
				}
				if (Any(tgts, t => t.Frequency == P3dHitScreenLine.FrequencyType.Once))
				{
					Draw("position", "When using <b>Frequency = Once</b>, this allows you to set the 0..1 position along the line.");
					Draw("pixelOffset", "When using <b>Frequency = Once</b>, this allows you to set the pixel offset along the line.");
				}
			EndIndent();
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