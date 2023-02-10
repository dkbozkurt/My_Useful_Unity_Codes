using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component fills the attached UI Image based on the total amount of pixels that have been painted in the specified <b>P3dChangeCounterFill</b> components.</summary>
	[RequireComponent(typeof(Image))]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dChangeCounterFill")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Change Counter Fill")]
	public class P3dChangeCounterFill : MonoBehaviour
	{
		/// <summary>This allows you to specify the counters that will be used.
		/// Zero = All active and enabled counters in the scene.</summary>
		public List<P3dChangeCounter> Counters { get { if (counters == null) counters = new List<P3dChangeCounter>(); return counters; } } [SerializeField] private List<P3dChangeCounter> counters;

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
			var ratio         = P3dChangeCounter.GetRatio(finalCounters);

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
	using TARGET = P3dChangeCounterFill;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dChangeCounterFill_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("counters", "This allows you to specify the counters that will be used.\n\nZero = All active and enabled counters in the scene.");

			Separator();

			Draw("inverse", "Inverse the fill?");
		}
	}
}
#endif