using UnityEngine;
using UnityEngine.EventSystems;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to perform the Undo All action. This can be done by attaching it to a clickable object, or manually from the RedoAll method.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dButtonUndoAll")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Button Undo All")]
	public class P3dButtonUndoAll : MonoBehaviour, IPointerClickHandler
	{
		public void OnPointerClick(PointerEventData eventData)
		{
			UndoAll();
		}

		/// <summary>If you want to manually trigger UndoAll, then call this function.</summary>
		[ContextMenu("Undo All")]
		public void UndoAll()
		{
			P3dStateManager.UndoAll();
		}

		protected virtual void Update()
		{
			var group = GetComponent<CanvasGroup>();

			if (group != null)
			{
				group.alpha = P3dStateManager.CanUndo == true ? 1.0f : 0.5f;
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dButtonUndoAll;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dUndoAll_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Info("This component allows you to perform the Undo All action. This can be done by attaching it to a clickable object, or manually from the UndoAll method.");
		}
	}
}
#endif