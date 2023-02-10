using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component will search the specified paintable texture for pixel colors matching an active and enabled P3dColor.</summary>
	[ExecuteInEditMode]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dColorCounter")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Color Counter")]
	public class P3dColorCounter : P3dPaintableTextureMonitorMask
	{
		public class Contribution
		{
			public P3dColor Color;
			public int      Count;
			public float    Ratio;
			public byte     R;
			public byte     G;
			public byte     B;
			public byte     A;

			public static Stack<Contribution> Pool = new Stack<Contribution>();
		}

		/// <summary>This stores all active and enabled instances.</summary>
		public static LinkedList<P3dColorCounter> Instances = new LinkedList<P3dColorCounter>(); private LinkedListNode<P3dColorCounter> instancesNode;

		/// <summary>The RGBA values must be within this range of a color for it to be counted.</summary>
		public float Threshold { set { if (threshold != value) { threshold = value; MarkCurrentReaderAsDirty(); } } get { return threshold; } } [Range(0.0f, 1.0f)] [SerializeField] private float threshold = 0.1f;

		/// <summary>Each color contribution will be stored in this list.</summary>
		public List<Contribution> Contributions { get { return contributions; } } [System.NonSerialized] private List<Contribution> contributions = new List<Contribution>();

		/// <summary>The <b>Total</b> of the specified counters.</summary>
		public static long GetTotal(ICollection<P3dColorCounter> counters = null)
		{
			var total = 0L; foreach (var counter in counters ?? Instances) { if (counter != null) total += counter.total; } return total;
		}

		/// <summary>The <b>Count</b> of the specified counters.</summary>
		public static long GetCount(P3dColor color, ICollection<P3dColorCounter> counters = null)
		{
			var count = 0L; foreach (var counter in counters ?? Instances) { if (counter != null) count += counter.Count(color); } return count;
		}

		/// <summary>The <b>Ratio</b> of the specified counters.</summary>
		public static float GetRatio(P3dColor color, ICollection<P3dColorCounter> counters = null)
		{
			return CwHelper.Divide(GetCount(color, counters), GetTotal(counters));
		}

		/// <summary>This tells you how many pixels of the specified color are in the current <b>PaintableTexture</b>.</summary>
		public int Count(P3dColor color)
		{
			foreach (var contribution in contributions)
			{
				if (contribution.Color == color)
				{
					return contribution.Count;
				}
			}

			return 0;
		}

		public float Ratio(P3dColor color)
		{
			if (total > 0)
			{
				return Count(color) / (float)total;
			}

			return 0.0f;
		}

		protected override void OnEnable()
		{
			instancesNode = Instances.AddLast(this);

			base.OnEnable();
		}

		protected override void OnDisable()
		{
			Instances.Remove(instancesNode); instancesNode = null;

			base.OnDisable();

			Contribute(0);
		}

		protected override void HandleComplete(int boost)
		{
			if (currentPixels.IsCreated == false || maskPixels.IsCreated == false || currentPixels.Length != maskPixels.Length)
			{
				return;
			}

			var threshold32 = (byte)(threshold * 255.0f);

			PrepareContributions();

			total = 0;

			for (var i = 0; i < currentPixels.Length; i++)
			{
				if (maskPixels[i] > 127)
				{
					total++;

					var currentPixel = currentPixels[i];
					var bestIndex    = -1;
					var bestDistance = (int)threshold32;

					for (var c = 0; c < P3dColor.Instances.Count; c++)
					{
						var tempColor = contributions[c];
						var distance  = 0;

						distance += System.Math.Abs(tempColor.R - currentPixel.r);
						distance += System.Math.Abs(tempColor.G - currentPixel.g);
						distance += System.Math.Abs(tempColor.B - currentPixel.b);
						distance += System.Math.Abs(tempColor.A - currentPixel.a);

						if (distance <= bestDistance)
						{
							bestIndex    = c;
							bestDistance = distance;
						}
					}

					if (bestIndex >= 0)
					{
						contributions[bestIndex].Count++;
					}
				}
			}

			total *= boost;

			// Multiply totals to account for downsampling
			Contribute(boost);

			InvokeOnUpdated();
		}

		private void ClearContributions()
		{
			for (var i = contributions.Count - 1; i >= 0; i--)
			{
				Contribution.Pool.Push(contributions[i]);
			}

			contributions.Clear();
		}

		private void PrepareContributions()
		{
			ClearContributions();

			foreach (var color in P3dColor.Instances)
			{
				var contribution = Contribution.Pool.Count > 0 ? Contribution.Pool.Pop() : new Contribution();
				var color32      = (Color32)color.Color;

				contribution.Color = color;
				contribution.Count = 0;
				contribution.R     = color32.r;
				contribution.G     = color32.g;
				contribution.B     = color32.b;
				contribution.A     = color32.a;

				contributions.Add(contribution);
			}

			total = 0;
		}

		private void Contribute(int scale)
		{
			var totalRecip = total > 0 ? 1.0f / total : 1.0f;

			for (var i = contributions.Count - 1; i >= 0; i--)
			{
				var contribution = contributions[i];

				contribution.Count *= scale;
				contribution.Ratio  = contribution.Count * totalRecip;

				if (contribution.Color != null)
				{
					contribution.Color.Contribute(this, contribution.Count);
				}

				if (contribution.Count <= 0)
				{
					Contribution.Pool.Push(contribution);

					contributions.RemoveAt(i);
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dColorCounter;

	[CustomEditor(typeof(TARGET))]
	public class P3dColorCounter_Editor : P3dPaintableTextureMonitorMask_Editor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			base.OnInspector();

			var markAsDirty = false;

			Draw("threshold", ref markAsDirty, "The RGBA values must be within this range of a color for it to be counted.");

			Separator();

			BeginDisabled();
				EditorGUILayout.IntField("Total", tgt.Total);

				for (var i = 0; i < tgt.Contributions.Count; i++)
				{
					var contribution = tgt.Contributions[i];
					var rect         = Reserve();
					var rectL        = rect; rectL.xMax -= (rect.width - EditorGUIUtility.labelWidth) / 2 + 1;
					var rectR        = rect; rectR.xMin = rectL.xMax + 2;

					EditorGUI.IntField(rectL, contribution.Color != null ? contribution.Color.name : "", contribution.Count);
					EditorGUI.ProgressBar(rectR, contribution.Ratio, "Ratio");
				}
			EndDisabled();

			if (markAsDirty == true)
			{
				Each(tgts, t => t.MarkCurrentReaderAsDirty(), true, true);
			}
		}
	}
}
#endif