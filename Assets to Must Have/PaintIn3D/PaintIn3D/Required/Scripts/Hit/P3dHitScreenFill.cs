using UnityEngine;
using System.Collections.Generic;

namespace PaintIn3D
{
	/// <summary>This component works like <b>P3dHitScreen</b>, but it will fill in the shape you draw.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dHitScreenFill")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Hit Screen Fill")]
	public class P3dHitScreenFill : P3dHitScreen
	{
		/// <summary>This allows you to set the pixel distance between each grid point.
		/// NOTE: The lower you set this, the lower the performance will be.</summary>
		public float FillSpacing { set { fillSpacing = value; } get { return fillSpacing; } } [SerializeField] private float fillSpacing = 5.0f;

		protected override void OnFingerUp(Link link)
		{
			var rect = GetArea(link.History);

			rect.xMin += fillSpacing * 0.25f;
			rect.yMin += fillSpacing * 0.25f;
			rect.xMax -= fillSpacing * 0.25f;
			rect.yMax -= fillSpacing * 0.25f;

			if (fillSpacing > 0.0f && rect.width > 0.0f && rect.height > 0.0f && link.History.Count > 0)
			{
				var stepsH = Mathf.CeilToInt(rect.width  / fillSpacing);
				var stepsV = Mathf.CeilToInt(rect.height / fillSpacing);
				var corner = rect.center - new Vector2(stepsH, stepsV) * fillSpacing * 0.5f;

				for (var y = 0; y <= stepsV; y++)
				{
					for (var x = 0; x <= stepsH; x++)
					{
						var p = corner + new Vector2(x, y) * fillSpacing;

						if (Contains(link.History, p) == true)
						{
							PaintAt(null, Connector.HitCache, p, p, false, 1.0f, null);
						}
					}
				}
			}
		}

		private static Rect GetArea(List<Vector2> points)
		{
			if (points != null && points.Count > 0)
			{
				var area = new Rect(points[0], Vector2.zero);

				foreach (var point in points)
				{
					area.min = Vector2.Min(area.min, point);
					area.max = Vector2.Max(area.max, point);
				}

				return area;
			}

			return default(Rect);
		}

		private static double LineSide(Vector2 a, Vector2 b, Vector2 p)
		{
			return (b.y - a.y) * (p.x - a.x) - (b.x - a.x) * (p.y - a.y);
		}

		private static bool Contains(List<Vector2> points, Vector2 xy)
		{
			var pointA = points[0];
			var total  = 0;

			for (var j = points.Count - 1; j >= 0; j--)
			{
				var pointB = points[j];

				if (pointA.y <= xy.y)
				{
					if (pointB.y > xy.y && LineSide(pointA, pointB, xy) > 0.0f) total += 1;
				}
				else
				{
					if (pointB.y <= xy.y && LineSide(pointA, pointB, xy) < 0.0f) total -= 1;
				}

				pointA = pointB;
			}

			return total != 0;
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dHitScreenFill;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dHitScreenFill_Editor : P3dHitScreen_Editor
	{
		protected override void DrawBasic()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			base.DrawBasic();

			Draw("fillSpacing", "This allows you to set the pixel distance between each grid point.\n\nNOTE: The lower you set this, the lower the performance will be.");
		}
	}
}
#endif