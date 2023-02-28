using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component fills the attached UI Image based on the total amount of opaque pixels that have been painted in all active and enabled <b>P3dChannelCounter</b> components in the scene.</summary>
	[RequireComponent(typeof(Image))]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dChannelCounterFill")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Channel Counter Fill")]
	public class P3dChannelCounterFill : MonoBehaviour
	{
		public enum ChannelType
		{
			Red,
			Green,
			Blue,
			Alpha
		}

		/// <summary>This allows you to specify the counters that will be used.
		/// Zero = All active and enabled counters in the scene.</summary>
		public List<P3dChannelCounter> Counters { get { if (counters == null) counters = new List<P3dChannelCounter>(); return counters; } } [SerializeField] private List<P3dChannelCounter> counters;

		/// <summary>This allows you to choose which channel will be output to the UI Image.</summary>
		public ChannelType Channel { set { channel = value; } get { return channel; } } [SerializeField] private ChannelType channel;

		/// <summary>Inverse the fill?</summary>
		public bool Inverse { set { inverse = value; } get { return inverse; } } [SerializeField] private bool inverse;

		[System.NonSerialized]
		private Image cachedImage;

		protected virtual void OnEnable()
		{
			cachedImage = GetComponent<Image>();
		}

		protected virtual void Update()
		{
			var finalCounters = counters.Count > 0 ? counters : null;
			var ratio         = 0.0f;

			switch (channel)
			{
				case ChannelType.Red:   ratio = P3dChannelCounter.GetRatioR(finalCounters); break;
				case ChannelType.Green: ratio = P3dChannelCounter.GetRatioG(finalCounters); break;
				case ChannelType.Blue:  ratio = P3dChannelCounter.GetRatioB(finalCounters); break;
				case ChannelType.Alpha: ratio = P3dChannelCounter.GetRatioA(finalCounters); break;
			}

			if (inverse == true)
			{
				ratio = 1.0f - ratio;
			}

			cachedImage.fillAmount = Mathf.Clamp01(ratio);
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dChannelCounterFill;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dChannelCounterFill_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("counters", "This allows you to specify the counters that will be used.\n\nZero = All active and enabled counters in the scene.");

			Separator();

			Draw("channel", "This allows you to choose which channel will be output to the UI Image.");
			Draw("inverse", "Inverse the fill?");
		}
	}
}
#endif