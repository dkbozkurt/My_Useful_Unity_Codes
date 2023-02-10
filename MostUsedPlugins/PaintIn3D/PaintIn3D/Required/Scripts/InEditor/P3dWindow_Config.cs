#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using CW.Common;

namespace PaintIn3D
{
	public partial class P3dWindow
	{
		[System.Serializable]
		public class SettingsData
		{
			public P3dGroup ColorGroup = 0;

			public bool OverrideStateLimit = true;

			public int StateLimit = 10;

			public int IconSize = 128;

			public bool OverrideColor = true;

			public Color Color = Color.red;

			public bool OverrideRadius = true;

			public float Radius = 1.0f;

			public bool OverrideAngle;

			public float Angle;

			public bool OverrideTiling;

			public float Tiling = 1.0f;

			public bool OverrideNormal;

			public float NormalFront = 1.0f;

			public float NormalBack = 0.0f;

			public float NormalFade = 0.01f;

			public bool OverrideModifiers;

			public P3dModifierList Modifiers = new P3dModifierList();

			public P3dTool CurrentTool;

			public P3dMaterial CurrentMaterial;

			public P3dShape CurrentShape;
		}

		public static SettingsData Settings = new SettingsData();

		private Vector2 configScrollPosition;

		private static void ClearSettings()
		{
			if (EditorPrefs.HasKey("PaintIn3D.Settings") == true)
			{
				EditorPrefs.DeleteKey("PaintIn3D.Settings");

				Settings = new SettingsData();
			}
		}

		private static void SaveSettings()
		{
			EditorPrefs.SetString("PaintIn3D.Settings", EditorJsonUtility.ToJson(Settings));
		}

		private static void LoadSettings()
		{
			if (EditorPrefs.HasKey("PaintIn3D.Settings") == true)
			{
				var json = EditorPrefs.GetString("PaintIn3D.Settings");

				if (string.IsNullOrEmpty(json) == false)
				{
					EditorJsonUtility.FromJsonOverwrite(json, Settings);
				}
			}
		}

		private void DrawConfigTab()
		{
			configScrollPosition = GUILayout.BeginScrollView(configScrollPosition, GUILayout.ExpandHeight(true));
				CwEditor.BeginLabelWidth(100);
					Settings.ColorGroup = EditorGUILayout.IntField("Color Group", Settings.ColorGroup);
					Settings.IconSize = EditorGUILayout.IntSlider("Icon Size", Settings.IconSize, 32, 256);
					EditorGUILayout.BeginHorizontal();
						Settings.StateLimit = EditorGUILayout.IntField("State Limit", Settings.StateLimit);
						if (GUILayout.Button(new GUIContent("Apply", "Apply this undo/redo state limit to all P3dPaintableTexture components in the scene?"), EditorStyles.miniButton, GUILayout.ExpandWidth(false)) == true)
						{
							if (EditorUtility.DisplayDialog("Are you sure?", "This will apply this StateLimit to all P3dPaintableTexture components in the scene.", "OK") == true)
							{
								ApplyStateLimit();
							}
						}
					EditorGUILayout.EndHorizontal();

					GUILayout.FlexibleSpace();

					if (GUILayout.Button("Clear Settings") == true)
					{
						if (EditorUtility.DisplayDialog("Are you sure?", "This will reset all editor painting settings to default.", "OK") == true)
						{
							ClearSettings();
						}
					}
				CwEditor.EndLabelWidth();
			GUILayout.EndScrollView();
		}

		private void ApplyStateLimit()
		{
			var paintableTextures = FindObjectsOfType<P3dPaintableTexture>();

			Undo.RecordObjects(paintableTextures, "Apply State Limit");

			foreach (var paintableTexture in paintableTextures)
			{
				if (paintableTexture.UndoRedo != P3dPaintableTexture.UndoRedoType.LocalCommandCopy)
				{
					paintableTexture.UndoRedo   = P3dPaintableTexture.UndoRedoType.FullTextureCopy;
					paintableTexture.StateLimit = Settings.StateLimit;

					EditorUtility.SetDirty(paintableTexture);
				}
			}
		}
	}
}
#endif