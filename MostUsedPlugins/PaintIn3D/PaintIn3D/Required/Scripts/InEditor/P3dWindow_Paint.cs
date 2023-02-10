#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using CW.Common;

namespace PaintIn3D
{
	public partial class P3dWindow
	{
		private Vector2 paintScrollPosition;

		private bool selectingTool;

		private bool selectingMaterial;

		private bool selectingShape;

		private void DrawPaintTab()
		{
			if (selectingTool == true)
			{
				DrawTool(); return;
			}

			if (selectingMaterial == true)
			{
				DrawMaterial(); return;
			}

			if (selectingShape == true)
			{
				DrawShape(); return;
			}

			paintScrollPosition = GUILayout.BeginScrollView(paintScrollPosition, GUILayout.ExpandHeight(true));
				DrawTop();

				EditorGUILayout.Separator();

				CwEditor.BeginLabelWidth(100);
					DrawRadius();
				CwEditor.EndLabelWidth();

				EditorGUILayout.Separator();

				CwEditor.BeginLabelWidth(100);
					DrawColor();
				CwEditor.EndLabelWidth();

				EditorGUILayout.Separator();

				CwEditor.BeginLabelWidth(100);
					DrawAngle();
				CwEditor.EndLabelWidth();

				EditorGUILayout.Separator();

				CwEditor.BeginLabelWidth(100);
					DrawTiling();
				CwEditor.EndLabelWidth();

				EditorGUILayout.Separator();

				CwEditor.BeginLabelWidth(100);
					DrawNormal();
				CwEditor.EndLabelWidth();

				EditorGUILayout.Separator();

				CwEditor.BeginLabelWidth(100);
					DrawModifiers();
				CwEditor.EndLabelWidth();
			GUILayout.EndScrollView();

			GUILayout.FlexibleSpace();

			if (Application.isPlaying == false)
			{
				EditorGUILayout.HelpBox("You must enter play mode to begin painting.", MessageType.Warning);
			}
			else
			{
				if (P3dSceneTool.IsActive == true)
				{
					if (P3dSceneTool.LastValid == false)
					{
						EditorGUILayout.HelpBox("You must paint in the Scene view.", MessageType.Warning);
					}

					if (SceneContainsNormalPaintComponent() == true)
					{
						EditorGUILayout.HelpBox("Your scene contains active paint components. These may conflict with the in-editor painting features.", MessageType.Warning);
					}
				}
				else
				{
					if (CwEditor.HelpButton("You must select the Paint in 3D tool from the top left tools menu to paint.", MessageType.Warning, "Select", 50) == true)
					{
						P3dSceneTool.SelectThisTool();
					}
				}
			}

			UpdatePaint();
		}

		private static bool SceneContainsNormalPaintComponent()
		{
			foreach (var paintDecal in FindObjectsOfType<P3dPaintDecal>())
			{
				if (paintDecal.GetComponentInParent<P3dTool>() == null)
				{
					return true;
				}
			}

			foreach (var paintDecal in FindObjectsOfType<P3dPaintSphere>())
			{
				if (paintDecal.GetComponentInParent<P3dTool>() == null)
				{
					return true;
				}
			}

			return false;
		}

		private void DrawTop()
		{
			var toolIcon      = default(Texture2D);
			var toolTitle     = "None";
			var materialIcon  = default(Texture2D);
			var materialTitle = "None";
			var shapeIcon     = default(Texture2D);
			var shapeTitle    = "None";
			var width         = Mathf.FloorToInt((position.width - 30) / 3);

			if (Settings.CurrentTool != null)
			{
				toolIcon  = Settings.CurrentTool.GetIcon();
				toolTitle = Settings.CurrentTool.GetTitle();
			}

			if (Settings.CurrentMaterial != null)
			{
				materialIcon  = Settings.CurrentMaterial.GetIcon();
				materialTitle = Settings.CurrentMaterial.GetTitle();
			}

			if (Settings.CurrentShape != null)
			{
				shapeIcon  = Settings.CurrentShape.GetIcon();
				shapeTitle = Settings.CurrentShape.GetTitle();
			}

			EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginVertical();
					var rectA = DrawIcon(width, toolIcon, toolTitle, Settings.CurrentTool != null, "Tool");
				EditorGUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginVertical();
					var rectB = DrawIcon(width, materialIcon, materialTitle, Settings.CurrentMaterial != null, "Material");
				EditorGUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				EditorGUILayout.BeginVertical();
					var rectC = DrawIcon(width, shapeIcon, shapeTitle, Settings.CurrentShape != null, "Shape");
				EditorGUILayout.EndVertical();
				GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			if (Event.current.type == EventType.MouseDown && rectA.Contains(Event.current.mousePosition) == true)
			{
				if (Event.current.button == 0)
				{
					selectingTool = true;
				}
				else
				{
					CwHelper.SelectAndPing(Settings.CurrentTool);
				}
			}

			if (Event.current.type == EventType.MouseDown && rectB.Contains(Event.current.mousePosition) == true)
			{
				if (Event.current.button == 0)
				{
					selectingMaterial = true;
				}
				else
				{
					CwHelper.SelectAndPing(Settings.CurrentMaterial);
				}
			}

			if (Event.current.type == EventType.MouseDown && rectC.Contains(Event.current.mousePosition) == true)
			{
				if (Event.current.button == 0)
				{
					selectingShape = true;
				}
				else
				{
					CwHelper.SelectAndPing(Settings.CurrentShape);
				}
			}
		}

		private void DrawRadius()
		{
			Settings.OverrideRadius = EditorGUILayout.Toggle("Override Radius", Settings.OverrideRadius);

			if (Settings.OverrideRadius == true)
			{
				EditorGUI.indentLevel++;
					Settings.Radius = LogSlider("Radius", Settings.Radius, -4, 4);
				EditorGUI.indentLevel--;
			}
		}

		private void DrawColor()
		{
			Settings.OverrideColor = EditorGUILayout.Toggle("Override Color", Settings.OverrideColor);

			if (Settings.OverrideColor == true)
			{
				EditorGUI.indentLevel++;
					Settings.Color   = EditorGUILayout.ColorField("Color", Settings.Color);
					Settings.Color.r = Slider("Red", Settings.Color.r, 0.0f, 1.0f);
					Settings.Color.g = Slider("Green", Settings.Color.g, 0.0f, 1.0f);
					Settings.Color.b = Slider("Blue", Settings.Color.b, 0.0f, 1.0f);
					Settings.Color.a = Slider("Alpha", Settings.Color.a, 0.0f, 1.0f);

					float h, s, v; Color.RGBToHSV(Settings.Color, out h, out s, out v);

					h = Slider("Hue"       , h, 0.0f, 1.0f);
					s = Slider("Saturation", s, 0.0f, 1.0f);
					v = Slider("Value"     , v, 0.0f, 1.0f);

					var newColor = Color.HSVToRGB(h, s, v);

					Settings.Color.r = newColor.r;
					Settings.Color.g = newColor.g;
					Settings.Color.b = newColor.b;
				EditorGUI.indentLevel--;
			}
		}

		private void DrawAngle()
		{
			Settings.OverrideAngle = EditorGUILayout.Toggle("Override Angle", Settings.OverrideAngle);

			if (Settings.OverrideAngle == true)
			{
				EditorGUI.indentLevel++;
					Settings.Angle = Slider("Angle", Settings.Angle, -180.0f, 180.0f);
				EditorGUI.indentLevel--;
			}
		}

		private void DrawTiling()
		{
			Settings.OverrideTiling = EditorGUILayout.Toggle("Override Tiling", Settings.OverrideTiling);

			if (Settings.OverrideTiling == true)
			{
				EditorGUI.indentLevel++;
					Settings.Tiling = Slider("Tiling", Settings.Tiling, 0.1f, 10.0f);
				EditorGUI.indentLevel--;
			}
		}

		private void DrawNormal()
		{
			Settings.OverrideNormal = EditorGUILayout.Toggle("Override Normal", Settings.OverrideNormal);

			if (Settings.OverrideNormal == true)
			{
				EditorGUI.indentLevel++;
					Settings.NormalFront = Slider("Front", Settings.NormalFront, 0.0f, 2.0f);
					Settings.NormalBack  = Slider("Back", Settings.NormalBack, 0.0f, 2.0f);
					Settings.NormalFade  = Slider("Fade", Settings.NormalFade, 0.0f, 0.5f);
				EditorGUI.indentLevel--;
			}
		}

		private void DrawModifiers()
		{
			Settings.OverrideModifiers = EditorGUILayout.Toggle("Override Modifiers", Settings.OverrideModifiers);

			if (Settings.OverrideModifiers == true)
			{
				EditorGUI.indentLevel++;
					Settings.Modifiers.DrawEditorLayout(true, "Color", "Angle", "Opacity", "Radius", "Hardness", "Texture", "Position");
				EditorGUI.indentLevel--;
			}
		}

		private void DrawPivot(Camera camera, Transform root, Matrix4x4 matrix)
		{
			var meshFilter   = root.GetComponent<MeshFilter>();
			var meshRenderer = root.GetComponent<MeshRenderer>();

			if (meshFilter != null && meshRenderer != null && meshFilter.sharedMesh != null && meshRenderer.sharedMaterial != null)
			{
				Graphics.DrawMesh(meshFilter.sharedMesh, matrix * root.localToWorldMatrix, meshRenderer.sharedMaterial, 0, camera, 0, null, UnityEngine.Rendering.ShadowCastingMode.Off, false);
			}

			foreach (Transform child in root)
			{
				DrawPivot(camera, child, matrix);
			}
		}

		private void UpdatePaint()
		{
			if (toolInstance != null)
			{
				/*
				foreach (var connectablePoint in toolInstance.GetComponentsInChildren<P3dPointConnector>())
				{
					connectablePoint.ClearHitCache();
				}

				foreach (var connectableLine in toolInstance.GetComponentsInChildren<P3dLineConnector>())
				{
					connectableLine.ClearHitCache();
				}
				*/
			}

			if (materialInstance != null)
			{
				foreach (var paintSphere in materialInstance.GetComponentsInChildren<P3dPaintSphere>())
				{
					if (paintSphere.Group == Settings.ColorGroup)
					{
						if (Settings.OverrideColor == true)
						{
							paintSphere.Color  = Settings.Color;
						}
					}

					if (Settings.OverrideRadius == true)
					{
						paintSphere.Radius = Settings.Radius;
					}
				}

				foreach (var paintDecal in materialInstance.GetComponentsInChildren<P3dPaintDecal>())
				{
					if (paintDecal.Group == Settings.ColorGroup)
					{
						if (Settings.OverrideColor == true)
						{
							paintDecal.Color = Settings.Color;
						}
					}

					if (Settings.OverrideRadius == true)
					{
						paintDecal.Radius = Settings.Radius;
					}

					if (Settings.OverrideAngle == true)
					{
						paintDecal.Angle = Settings.Angle;
					}

					if (Settings.OverrideTiling == true)
					{
						paintDecal.transform.localScale = Vector3.one * Settings.Tiling;

						paintDecal.TileTransform = paintDecal.transform;
					}

					if (Settings.OverrideNormal == true)
					{
						paintDecal.NormalFront = Settings.NormalFront;
						paintDecal.NormalBack  = Settings.NormalBack;
						paintDecal.NormalFade  = Settings.NormalFade;
					}

					if (Settings.OverrideModifiers == true)
					{
						paintDecal.Modifiers.Instances.Clear();

						foreach (var modifier in Settings.Modifiers.Instances)
						{
							paintDecal.Modifiers.Instances.Add(modifier);
						}
					}

					paintDecal.Shape        = Settings.CurrentShape != null ? Settings.CurrentShape.Icon : null;
					paintDecal.ShapeChannel = P3dChannel.Red;
				}
			}
		}
	}
}
#endif