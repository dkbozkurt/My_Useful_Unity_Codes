using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component will total up all RGBA channels in the specified P3dPaintableTexture that exceed the threshold value.</summary>
	[ExecuteInEditMode]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dChannelCounter")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Channel Counter")]
	public class P3dChannelCounter : P3dPaintableTextureMonitorMask
	{
		/// <summary>This stores all active and enabled instances.</summary>
		public static LinkedList<P3dChannelCounter> Instances = new LinkedList<P3dChannelCounter>(); private LinkedListNode<P3dChannelCounter> instancesNode;

		/// <summary>The RGBA value must be higher than this for it to be counted.</summary>
		public float Threshold { set { if (threshold != value) { threshold = value; MarkCurrentReaderAsDirty(); } } get { return threshold; } } [Range(0.0f, 1.0f)] [SerializeField] private float threshold = 0.5f;

		/// <summary>The previously counted amount of pixels with a red channel value above the threshold.</summary>
		public int CountR { get { return countR; } } [SerializeField] private int countR;

		/// <summary>The previously counted amount of pixels with a green channel value above the threshold.</summary>
		public int CountG { get { return countG; } } [SerializeField] private int countG;

		/// <summary>The previously counted amount of pixels with a blue channel value above the threshold.</summary>
		public int CountB { get { return countB; } } [SerializeField] private int countB;

		/// <summary>The previously counted amount of pixels with a alpha channel value above the threshold.</summary>
		public int CountA { get { return countA; } } [SerializeField] private int countA;

		/// <summary>The <b>CountR/Total</b> value, allowing you to easily see how much % of the red channel is above the threshold.</summary>
		public float RatioR { get { return total > 0 ? countR / (float)total : 0.0f; } }

		/// <summary>The <b>CountG/Total</b> value, allowing you to easily see how much % of the green channel is above the threshold.</summary>
		public float RatioG { get { return total > 0 ? countG / (float)total : 0.0f; } }

		/// <summary>The <b>CountB/Total</b> value, allowing you to easily see how much % of the blue channel is above the threshold.</summary>
		public float RatioB { get { return total > 0 ? countB / (float)total : 0.0f; } }

		/// <summary>The <b>CountA/Total</b> value, allowing you to easily see how much % of the alpha channel is above the threshold.</summary>
		public float RatioA { get { return total > 0 ? countA / (float)total : 0.0f; } }

		/// <summary>The <b>RatioR/G/B/A</b> values packed into a Vector4.</summary>
		public Vector4 RatioRGBA
		{
			get
			{
				if (total > 0)
				{
					var ratios = default(Vector4);
					var scale  = 1.0f / total;

					ratios.x = Mathf.Clamp01(countR * scale);
					ratios.y = Mathf.Clamp01(countG * scale);
					ratios.z = Mathf.Clamp01(countB * scale);
					ratios.w = Mathf.Clamp01(countA * scale);

					return ratios;
				}

				return Vector4.zero;
			}
		}

		/// <summary>The <b>Total</b> of the specified counters.</summary>
		public static long GetTotal(ICollection<P3dChannelCounter> counters = null)
		{
			var total = 0L; foreach (var counter in counters ?? Instances) { if (counter != null) total += counter.total; } return total;
		}

		/// <summary>The <b>CountR</b> of the specified counters.</summary>
		public static long GetCountR(ICollection<P3dChannelCounter> counters = null)
		{
			var solid = 0L; foreach (var counter in counters ?? Instances) { if (counter != null) solid += counter.countR; } return solid;
		}

		/// <summary>The <b>CountG</b> of the specified counters.</summary>
		public static long GetCountG(ICollection<P3dChannelCounter> counters = null)
		{
			var solid = 0L; foreach (var counter in counters ?? Instances) { if (counter != null) solid += counter.countG; } return solid;
		}

		/// <summary>The <b>CountB</b> of the specified counters.</summary>
		public static long GetCountB(ICollection<P3dChannelCounter> counters = null)
		{
			var solid = 0L; foreach (var counter in counters ?? Instances) { if (counter != null) solid += counter.countB; } return solid;
		}

		/// <summary>The <b>CountA</b> of the specified counters.</summary>
		public static long GetCountA(ICollection<P3dChannelCounter> counters = null)
		{
			var solid = 0L; foreach (var counter in counters ?? Instances) { if (counter != null) solid += counter.countA; } return solid;
		}

		/// <summary>The <b>CountR / Total</b> of the specified counters.</summary>
		public static float GetRatioR(ICollection<P3dChannelCounter> counters = null)
		{
			return CwHelper.Divide(GetCountR(counters), GetTotal(counters));
		}

		/// <summary>The <b>CountG / Total</b> of the specified counters.</summary>
		public static float GetRatioG(ICollection<P3dChannelCounter> counters = null)
		{
			return CwHelper.Divide(GetCountG(counters), GetTotal(counters));
		}

		/// <summary>The <b>CountB / Total</b> of the specified counters.</summary>
		public static float GetRatioB(ICollection<P3dChannelCounter> counters = null)
		{
			return CwHelper.Divide(GetCountB(counters), GetTotal(counters));
		}

		/// <summary>The <b>CountA / Total</b> of the specified counters.</summary>
		public static float GetRatioA(ICollection<P3dChannelCounter> counters = null)
		{
			return CwHelper.Divide(GetCountA(counters), GetTotal(counters));
		}

		/// <summary>The <b>GetCountR/G/B/A / GetTotal</b> of the specified counters stored in a Vector4.</summary>
		public static Vector4 GetRatioRGBA(ICollection<P3dChannelCounter> counters = null)
		{
			if (counters == null) counters = Instances;

			if (counters.Count > 0)
			{
				var total = Vector4.zero;
				var count = 0;

				foreach (var counter in counters)
				{
					if (counter != null) 
					{
						count += 1;

						total.x += counter.RatioR;
						total.y += counter.RatioG;
						total.z += counter.RatioB;
						total.w += counter.RatioA;
					}
				}

				return count > 0 ? Vector4.zero : total / count;
			}

			return Vector4.zero;
		}

		protected override void OnEnable()
		{
			instancesNode = Instances.AddLast(this);

			base.OnEnable();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			Instances.Remove(instancesNode); instancesNode = null;
		}

		protected override void HandleComplete(int boost)
		{
			if (currentPixels.IsCreated == false || maskPixels.IsCreated == false || currentPixels.Length != maskPixels.Length)
			{
				return;
			}

			var threshold32 = (byte)(threshold * 255.0f);

			// Reset totals
			countR = 0;
			countG = 0;
			countB = 0;
			countA = 0;
			total  = 0;

			// Calculate totals
			for (var i = 0; i < currentPixels.Length; i++)
			{
				if (maskPixels[i] > 127)
				{
					total++;

					var currentPixel32 = currentPixels[i];

					if (currentPixel32.r >= threshold32) countR++;
					if (currentPixel32.g >= threshold32) countG++;
					if (currentPixel32.b >= threshold32) countB++;
					if (currentPixel32.a >= threshold32) countA++;
				}
			}

			// Scale totals to account for downsampling
			countR *= boost;
			countG *= boost;
			countB *= boost;
			countA *= boost;
			total  *= boost;

			InvokeOnUpdated();
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dChannelCounter;

	[CustomEditor(typeof(TARGET))]
	public class P3dChannelCounter_Editor : P3dPaintableTextureMonitorMask_Editor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			base.OnInspector();

			var markAsDirty = false;

			Draw("threshold", ref markAsDirty, "The RGBA value must be higher than this for it to be counted.");

			Separator();

			BeginDisabled();
				EditorGUILayout.IntField("Total", tgt.Total);

				DrawChannel("countR", "Ratio R", tgt.RatioR);
				DrawChannel("countG", "Ratio G", tgt.RatioG);
				DrawChannel("countB", "Ratio B", tgt.RatioB);
				DrawChannel("countA", "Ratio A", tgt.RatioA);
			EndDisabled();

			if (markAsDirty == true)
			{
				Each(tgts, t => t.MarkCurrentReaderAsDirty(), true, true);
			}
		}

		private void DrawChannel(string countTitle, string ratioTitle, float ratio)
		{
			var rect  = Reserve();
			var rectL = rect; rectL.xMax -= (rect.width - EditorGUIUtility.labelWidth) / 2 + 1;
			var rectR = rect; rectR.xMin = rectL.xMax + 2;

			EditorGUI.PropertyField(rectL, serializedObject.FindProperty(countTitle));
			EditorGUI.ProgressBar(rectR, ratio, ratioTitle);
		}
	}
}
#endif