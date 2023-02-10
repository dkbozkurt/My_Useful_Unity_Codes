#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PaintIn3D
{
	public partial class P3dWindow
	{
		private Vector2 materialScrollPosition;

		private P3dMaterial materialInstance;

		private List<IBrowsable> materialItems = new List<IBrowsable>();

		private void ClearMaterial()
		{
			if (materialInstance != null)
			{
				DestroyImmediate(materialInstance.gameObject);
			}

			materialInstance = null;
		}

		private void LoadMaterial(P3dMaterial prefab)
		{
			ClearMaterial();

			Settings.CurrentMaterial = prefab;

			if (EditorApplication.isPlaying == true)
			{
				materialInstance = Instantiate(prefab);

				//materialInstance.gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;

				if (toolInstance != null)
				{
					materialInstance.transform.SetParent(toolInstance.transform, false);
				}
			}

			Repaint();
		}

		private void DrawMaterial()
		{
			materialItems.Clear();

			foreach (var cachedMaterial in P3dMaterial.CachedMaterials)
			{
				if (cachedMaterial != null)
				{
					materialItems.Add(cachedMaterial);
				}
			}

			materialScrollPosition = GUILayout.BeginScrollView(materialScrollPosition, GUILayout.ExpandHeight(true));
				var selected = DrawBrowser(materialItems, Settings.CurrentMaterial);

				if (selected != null)
				{
					LoadMaterial((P3dMaterial)selected); selectingMaterial = false;
				}
			GUILayout.EndScrollView();

			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
				Settings.IconSize = EditorGUILayout.IntSlider(Settings.IconSize, 32, 256);

				EditorGUILayout.Separator();

				if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)) == true)
				{
					P3dMaterial.ClearCache(); AssetDatabase.Refresh();
				}
			EditorGUILayout.EndHorizontal();
		}
	}
}
#endif