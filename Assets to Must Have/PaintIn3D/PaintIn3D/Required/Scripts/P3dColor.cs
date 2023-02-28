using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to define a color that can later be counted from the <b>P3dColorCounter</b> component.
	/// NOTE: You should put this component its own GameObject, so you can give it a unique name.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dColor")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Color")]
	public class P3dColor : MonoBehaviour
	{
		[SerializeField]
		private class Contribution
		{
			public P3dColorCounter Counter;
			public int             Solid;
		}

		/// <summary>The color associated with this component and GameObject name.</summary>
		public Color Color { set { color = value; } get { return color; } } [SerializeField] private Color color;

		[SerializeField]
		private List<Contribution> contributions;

		/// <summary>This stores all active and enabled instances in the open scenes.</summary>
		public static LinkedList<P3dColor> Instances { get { return instances; } } private static LinkedList<P3dColor> instances = new LinkedList<P3dColor>(); private LinkedListNode<P3dColor> instancesNode;

		/// <summary>This tells you how many pixels this color could be painted on.</summary>
		public int Total
		{
			get
			{
				var total = 0;

				foreach (var colorCounter in P3dColorCounter.Instances)
				{
					total += colorCounter.Total;
				}

				return total;
			}
		}

		/// <summary>This tells you how many pixels this color has been painted on.</summary>
		public int Solid
		{
			get
			{
				var solid = 0;

				if (contributions != null)
				{
					for (var i = contributions.Count - 1; i >= 0; i--)
					{
						var contribution = contributions[i];

						if (CwHelper.Enabled(contribution.Counter) == true)
						{
							solid += contribution.Solid;
						}
						else
						{
							contributions.RemoveAt(i);
						}
					}
				}

				return solid;
			}
		}

		/// <summary>This is Solid/Total, allowing you to quickly see the percentage of paintable pixels that have been painted by this color.</summary>
		public float Ratio
		{
			get
			{
				var total = Total;

				if (total > 0)
				{
					return Solid / (float)total;
				}

				return 0.0f;
			}
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);

			foreach (var colorCounter in P3dColorCounter.Instances)
			{
				colorCounter.MarkCurrentReaderAsDirty();
			}
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode); instancesNode = null;

			foreach (var colorCounter in P3dColorCounter.Instances)
			{
				colorCounter.MarkCurrentReaderAsDirty();
			}
		}

		public void Contribute(P3dColorCounter counter, int solid)
		{
			var contribution = default(Contribution);

			if (TryGetContribution(counter, ref contribution) == false)
			{
				if (solid <= 0)
				{
					return;
				}

				contribution = new Contribution();

				contributions.Add(contribution);

				contribution.Counter = counter;
			}

			contribution.Solid = solid;
		}

		private bool TryGetContribution(P3dColorCounter counter, ref Contribution contribution)
		{
			if (contributions == null)
			{
				contributions = new List<Contribution>();
			}

			for (var i = contributions.Count - 1; i >= 0; i--)
			{
				contribution = contributions[i];

				if (contribution.Counter == counter)
				{
					return true;
				}
			}

			return false;
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dColor;

	[CustomEditor(typeof(TARGET))]
	public class P3dColor_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("color", "The color associated with this component and GameObject name.");

			Separator();

			BeginDisabled(true);
				EditorGUILayout.IntField(new GUIContent("Total", "This tells you how many pixels this color could be painted on."), tgt.Total);
				var rect  = Reserve();
				var rectL = rect; rectL.xMax -= (rect.width - EditorGUIUtility.labelWidth) / 2 + 1;
				var rectR = rect; rectR.xMin = rectL.xMax + 2;

				EditorGUI.IntField(rectL, new GUIContent("Solid", "This tells you how many pixels this color has been painted on."), tgt.Solid);
				EditorGUI.ProgressBar(rectR, tgt.Ratio, "Ratio");
			EndDisabled();
		}
	}
}
#endif