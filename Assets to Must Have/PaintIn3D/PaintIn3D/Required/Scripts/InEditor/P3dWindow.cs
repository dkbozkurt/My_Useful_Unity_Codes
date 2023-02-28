#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using CW.Common;

namespace PaintIn3D
{
	public partial class P3dWindow : CwEditorWindow
	{
		enum PageType
		{
			Scene,
			Paint,
			Config
		}

		private PageType currentPage;

		[MenuItem("Window/Paint in 3D")]
		public static void OpenWindow()
		{
			GetWindow();
		}

		public static P3dWindow GetWindow()
		{
			return GetWindow<P3dWindow>("Paint in 3D", true);
		}

		protected virtual void OnDestroy()
		{
			ClearMaterial();
			ClearTool();
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			LoadSettings();

			EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			SaveSettings();

			EditorApplication.playModeStateChanged -= HandlePlayModeStateChanged;

			Tools.current = Tool.Move;
		}

		private void HandlePlayModeStateChanged(PlayModeStateChange state)
		{
			ClearShape();
			ClearMaterial();
			ClearTool();

			if (currentPage == PageType.Paint && state == PlayModeStateChange.EnteredPlayMode)
			{
				P3dSceneTool.SelectThisTool();
			}
		}

		protected virtual void Update()
		{
			var load = false;

			if (EditorApplication.isPlaying == true)
			{
				if (P3dSceneTool.IsActive == true)
				{
					load = true;

					if (Settings.CurrentTool != null && toolInstance == null)
					{
						LoadTool(Settings.CurrentTool);
					}

					if (Settings.CurrentMaterial != null && materialInstance == null)
					{
						LoadMaterial(Settings.CurrentMaterial);
					}
				}
			}

			if (load == false)
			{
				ClearTool();
				ClearMaterial();
			}

			Repaint();
		}

		protected override void OnInspector()
		{
			var paintables        = FindObjectsOfType<P3dPaintable>();
			var paintableTextures = FindObjectsOfType<P3dPaintableTexture>();
			var previousPage      = currentPage;

			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
				EditorGUI.BeginDisabledGroup(P3dStateManager.CanUndo == false);
					if (GUILayout.Button(new GUIContent("↺", "Undo"), EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)) == true)
					{
						P3dStateManager.UndoAll();
					}
				EditorGUI.EndDisabledGroup();
				EditorGUI.BeginDisabledGroup(P3dStateManager.CanRedo == false);
					if (GUILayout.Button(new GUIContent("↻", "Redo"), EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)) == true)
					{
						P3dStateManager.RedoAll();
					}
				EditorGUI.EndDisabledGroup();

				if (GUILayout.Toggle(currentPage == PageType.Scene, "Scene", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)) == true)
				{
					currentPage = PageType.Scene; selectingTool = selectingMaterial = selectingShape = false;
				}

				if (GUILayout.Toggle(currentPage == PageType.Paint, "Paint", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)) == true)
				{
					currentPage = PageType.Paint;
				}

				if (GUILayout.Toggle(currentPage == PageType.Config, "Config", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)) == true)
				{
					currentPage = PageType.Config;
				}

				EditorGUILayout.Separator();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Separator();

			if (Application.isPlaying == true)
			{
				if (Settings.CurrentTool == null && P3dTool.CachedTools.Count > 0)
				{
					Settings.CurrentTool = P3dTool.CachedTools[0];
				}

				if (Settings.CurrentMaterial == null && P3dMaterial.CachedMaterials.Count > 0)
				{
					Settings.CurrentMaterial = P3dMaterial.CachedMaterials[0];
				}

				if (Settings.CurrentShape == null && P3dShape.CachedShapes.Count > 0)
				{
					Settings.CurrentShape = P3dShape.CachedShapes[0];
				}
			}
			else
			{
				ClearMaterial();
				ClearTool();
			}

			switch (currentPage)
			{
				case PageType.Scene:
				{
					DrawSceneTab(paintables, paintableTextures);
				}
				break;

				case PageType.Paint:
				{
					if (previousPage != PageType.Paint)
					{
						P3dSceneTool.SelectThisTool();
					}

					DrawPaintTab();
				}
				break;

				case PageType.Config:
				{
					DrawConfigTab();
				}
				break;
			}
		}

		private static GUIStyle selectableStyleA;
		private static GUIStyle selectableStyleB;
		private static GUIStyle selectableStyleC;

		private static Texture2D LoadTempTexture(string base64)
		{
			var tex  = new Texture2D(1, 1);
			var data = System.Convert.FromBase64String(base64);

			tex.LoadImage(data);

			return tex;
		}

		private static GUIStyle GetSelectableStyle(bool selected, bool pad)
		{
			if (selectableStyleA == null || selectableStyleA.normal.background == null)
			{
				selectableStyleA = new GUIStyle(); selectableStyleA.border = new RectOffset(4,4,4,4); selectableStyleA.normal.background = LoadTempTexture("iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAYAAADED76LAAAASUlEQVQYGWN0iL73nwEPYIHKNeJQUw9TAJI/gKbIAcRnQhbcv0TxAAgji6EoQJaAsZGtYHCMue8Ak4DRyAowJEGKYArqYTrQaQBpfAuV0+TyawAAAABJRU5ErkJggg==");
			}

			if (selectableStyleB == null || selectableStyleB.normal.background == null)
			{
				selectableStyleB = new GUIStyle(); selectableStyleB.border = new RectOffset(4,4,4,4); selectableStyleB.normal.background = LoadTempTexture("iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAYAAADED76LAAAARElEQVQYGWNkYGBwAGKcgAUq8wCHCgWYApD8BzRFAiA+E5ogSBGKQnQFaOoZGJCtAEmCjUVWhawAQxKkEKZAAVkXMhsAA6sEekpg61oAAAAASUVORK5CYII=");
			}

			if (selectableStyleC == null)
			{
				selectableStyleC = new GUIStyle(selectableStyleA); selectableStyleC.padding = new RectOffset(2,2,4,4);
			}

			if (selected == true)
			{
				return pad == true ? selectableStyleC : selectableStyleA;
			}

			return selectableStyleB;
		}

		private static float LogSlider(string title, float value, float logMin, float logMax)
		{
			var rect   = CwEditor.Reserve();
			var rectA  = rect; rectA.width = EditorGUIUtility.labelWidth + 50;
			var rectB  = rect; rectB.xMin += EditorGUIUtility.labelWidth + 52;
			var logOld = Mathf.Log10(value);
			var logNew = GUI.HorizontalSlider(rectB, logOld, logMin, logMax);

			if (logOld != logNew)
			{
				value = Mathf.Pow(10.0f, logNew);
			}

			return EditorGUI.FloatField(rectA, title, value);
		}

		private static float Slider(string title, float value, float min, float max)
		{
			var rect  = CwEditor.Reserve();
			var rectA = rect; rectA.width = EditorGUIUtility.labelWidth + 50;
			var rectB = rect; rectB.xMin += EditorGUIUtility.labelWidth + 52;

			value = GUI.HorizontalSlider(rectB, value, min, max);

			return EditorGUI.FloatField(rectA, title, value);
		}
	}
}
#endif