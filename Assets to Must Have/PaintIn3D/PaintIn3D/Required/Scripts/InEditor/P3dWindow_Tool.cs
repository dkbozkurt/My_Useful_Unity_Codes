#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PaintIn3D
{
	public partial class P3dWindow
	{
		private Vector2 toolScrollPosition;

		private P3dTool toolInstance;

		private List<IBrowsable> toolItems = new List<IBrowsable>();

		private void ClearTool()
		{
			if (toolInstance != null)
			{
				DestroyImmediate(toolInstance.gameObject);
			}

			toolInstance = null;
		}

		private void LoadTool(P3dTool prefab)
		{
			if (materialInstance != null)
			{
				materialInstance.transform.SetParent(null, false);
			}

			ClearTool();

			Settings.CurrentTool = prefab;

			if (EditorApplication.isPlaying == true)
			{
				toolInstance = Instantiate(prefab);

				//toolInstance.gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;

				toolInstance.transform.SetParent(P3dPaintableManager.GetOrCreateInstance().transform);

				if (materialInstance != null)
				{
					materialInstance.transform.SetParent(toolInstance.transform, false);
				}
			}

			Repaint();
		}

		private void DrawTool()
		{
			toolItems.Clear();

			foreach (var cachedTool in P3dTool.CachedTools)
			{
				if (cachedTool != null)
				{
					toolItems.Add(cachedTool);
				}
			}

			toolScrollPosition = GUILayout.BeginScrollView(toolScrollPosition, GUILayout.ExpandHeight(true));
				var selected = DrawBrowser(toolItems, Settings.CurrentTool);

				if (selected != null)
				{
					LoadTool((P3dTool)selected); selectingTool = false;
				}
			GUILayout.EndScrollView();

			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
				Settings.IconSize = EditorGUILayout.IntSlider(Settings.IconSize, 32, 256);

				EditorGUILayout.Separator();

				if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)) == true)
				{
					P3dTool.ClearCache(); AssetDatabase.Refresh();
				}
			EditorGUILayout.EndHorizontal();
		}
	}
}
#endif