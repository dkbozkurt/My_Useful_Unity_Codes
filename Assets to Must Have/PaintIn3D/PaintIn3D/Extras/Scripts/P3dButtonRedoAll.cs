using UnityEngine;
using UnityEngine.EventSystems;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to perform the Redo All action. This can be done by attaching it to a clickable object, or manually from the RedoAll method.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dButtonRedoAll")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Button Redo All")]
	public class P3dButtonRedoAll : MonoBehaviour, IPointerClickHandler
	{
		public void OnPointerClick(PointerEventData eventData)
		{
			RedoAll();
		}

		/// <summary>If you want to manually trigger RedoAll, then call this function.</summary>
		[ContextMenu("Redo All")]
		public void RedoAll()
		{
			P3dStateManager.RedoAll();
		}

		protected virtual void Update()
		{
			var group = GetComponent<CanvasGroup>();

			if (group != null)
			{
				group.alpha = P3dStateManager.CanRedo == true ? 1.0f : 0.5f;
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dButtonRedoAll;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dRedoAll_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Info("This component allows you to perform the Redo All action. This can be done by attaching it to a clickable object, or manually from the RedoAll method.");
		}
	}
}
#endif