using UnityEngine;
using UnityEngine.EventSystems;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to perform the Clear action. This can be done by attaching it to a clickable object, or manually from the ClearAll method.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dButtonClearAll")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Button Clear All")]
	public class P3dButtonClearAll : MonoBehaviour, IPointerClickHandler
	{
		/// <summary>When clearing a texture, should its undo states be cleared too?</summary>
		public bool ClearStates { set { clearStates = value; } get { return clearStates; } } [SerializeField] private bool clearStates = true;

		public void OnPointerClick(PointerEventData eventData)
		{
			ClearAll();
		}

		[ContextMenu("Clear All")]
		public void ClearAll()
		{
			foreach (var paintableTexture in P3dPaintableTexture.Instances)
			{
				paintableTexture.Clear();

				if (clearStates == true)
				{
					paintableTexture.ClearStates();
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dButtonClearAll;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dButtonClearAll_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("clearStates", "When clearing a texture, should its undo states be cleared too?");
		}
	}
}
#endif