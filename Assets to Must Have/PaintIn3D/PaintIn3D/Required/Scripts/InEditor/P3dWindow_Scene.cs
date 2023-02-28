#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CW.Common;

namespace PaintIn3D
{
	public partial class P3dWindow
	{
		private Vector2 sceneScrollPosition;

		private static HashSet<Transform> roots = new HashSet<Transform>();

		private static void RunRoots()
		{
			roots.Clear();

			foreach (var transform in Selection.transforms)
			{
				RunRoots(transform);
			}
		}

		private static void RunRoots(Transform t)
		{
			if (t.GetComponent<P3dPaintable>() == null)
			{
				roots.Add(t);
			}

			foreach (Transform child in t)
			{
				RunRoots(child);
			}
		}

		private void DrawSceneTab(P3dPaintable[] paintables, P3dPaintableTexture[] paintableTextures)
		{
			RunRoots();

			var removePaintable = default(P3dPaintable);

			Settings.OverrideStateLimit  = EditorGUILayout.Toggle(new GUIContent("Override State Limit", "Automatically make any newly added textures support undo/redo?"), Settings.OverrideStateLimit);

			EditorGUILayout.Separator();

			sceneScrollPosition = GUILayout.BeginScrollView(sceneScrollPosition, GUILayout.ExpandHeight(true));
				foreach (var root in roots)
				{
					var mr  = root.GetComponent<MeshRenderer>();
					var smr = root.GetComponent<SkinnedMeshRenderer>();

					if (mr != null || smr != null)
					{
						EditorGUILayout.BeginHorizontal();
							EditorGUI.BeginDisabledGroup(true);
								EditorGUILayout.ObjectField(GUIContent.none, root.gameObject, typeof(GameObject), true, GUILayout.MinWidth(10));
							EditorGUI.EndDisabledGroup();
							if (GUILayout.Button("Make Paintable", EditorStyles.miniButton, GUILayout.ExpandWidth(false)) == true)
							{
								root.gameObject.AddComponent<P3dPaintable>();
							}
						EditorGUILayout.EndHorizontal();
					}
				}
				foreach (var paintable in paintables)
				{
					EditorGUILayout.BeginHorizontal();
						EditorGUI.BeginDisabledGroup(true);
							EditorGUILayout.ObjectField(GUIContent.none, paintable, typeof(P3dPaintable), true, GUILayout.MinWidth(10));
						EditorGUI.EndDisabledGroup();
						if (GUILayout.Button(new GUIContent("X", "Remove all paintable components from this GameObject?"), EditorStyles.miniButton, GUILayout.ExpandWidth(false)) == true)
						{
							if (EditorUtility.DisplayDialog("Are you sure?", "Remove painting components from this GameObject?", "OK") == true)
							{
								removePaintable = paintable;
							}
						}
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel++;
						DrawMaterials(paintable, paintable.Materials, paintable.GetComponents<P3dPaintableTexture>());
					EditorGUI.indentLevel--;
				}
			GUILayout.EndScrollView();

			if (paintables.Length == 0)
			{
				GUILayout.FlexibleSpace();

				EditorGUILayout.HelpBox("Your scene doesn't contain any paintable objects.", MessageType.Warning);
			}

			DrawSceneFooter(paintableTextures);

			if (removePaintable != null)
			{
				removePaintable.RemoveComponents();
			}
		}

		private void DrawMaterials(P3dPaintable paintable, Material[] materials, P3dPaintableTexture[] paintableTextures)
		{
			for (var i = 0; i < materials.Length; i++)
			{
				var material = materials[i];

				EditorGUILayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup(true);
						EditorGUILayout.ObjectField(GUIContent.none, material, typeof(Material), true, GUILayout.MinWidth(10));
					EditorGUI.EndDisabledGroup();
					if (material != null && paintableTextures.Length > 0)
					{
						if (GUILayout.Button(new GUIContent("Export", "Export this material and all its textures to your project as assets?"), EditorStyles.miniButton, GUILayout.ExpandWidth(false)) == true)
						{
							var path = AssetDatabase.GetAssetPath(material);
							var dir  = string.IsNullOrEmpty(path) == false ? System.IO.Path.GetDirectoryName(path) : "Assets";

							path = EditorUtility.SaveFilePanelInProject("Export Material & Textures", name, "mat", "Export Your Material and Textures", dir);

							if (string.IsNullOrEmpty(path) == false)
							{
								Undo.RecordObjects(paintableTextures, "Export Material & Textures");

								var clone = Instantiate(material);

								AssetDatabase.CreateAsset(clone, path);

								foreach (var paintableTexture in paintableTextures)
								{
									var path2 = System.IO.Path.GetDirectoryName(path) + "/" + System.IO.Path.GetFileNameWithoutExtension(path) + paintableTexture.Slot.Name + ".png";

									System.IO.File.WriteAllBytes(path2, paintableTexture.GetPngData(true));

									AssetDatabase.ImportAsset(path2);

									paintableTexture.Output = AssetDatabase.AssetPathToGUID(path2);

									clone.SetTexture(paintableTexture.Slot.Name, AssetDatabase.LoadAssetAtPath<Texture>(path2));
								}

								EditorUtility.SetDirty(this);
							}
						}
					}
					if (GUILayout.Button(new GUIContent("+Preset", "Automatically configure textures for painting based on presets for this shader?"), EditorStyles.miniButton, GUILayout.ExpandWidth(false)) == true)
					{
						var menu       = new GenericMenu();
						var stateLimit = Settings.OverrideStateLimit == true ? Settings.StateLimit : -1;
						
						foreach (var cachedPreset in P3dPreset.CachedPresets)
						{
							if (cachedPreset != null && material != null && cachedPreset.Targets(material.shader) == true)
							{
								var preset = cachedPreset;
								var index  = i;

								if (preset.CanAddTo(paintable, index) == true)
								{
									menu.AddItem(new GUIContent(preset.FinalName), false, () => preset.AddTo(paintable, material.shader, index, stateLimit));
								}
								else
								{
									menu.AddDisabledItem(new GUIContent(preset.FinalName));
								}
							}
						}

						if (menu.GetItemCount() == 0)
						{
							menu.AddDisabledItem(new GUIContent("Failed to find any presets for this material or shader."));
						}

						menu.ShowAsContext();
					}
				EditorGUILayout.EndHorizontal();

				foreach (var paintableTexture in paintableTextures)
				{
					if (paintableTexture.Slot.Index == i)
					{
						EditorGUI.indentLevel++;
							DrawPaintableTexture(paintableTexture, material);
						EditorGUI.indentLevel--;
					}
				}
			}
		}

		private void DrawPaintableTexture(P3dPaintableTexture paintableTexture, Material material)
		{
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(paintableTexture.Slot.GetTitle(material));
				if (GUILayout.Button(new GUIContent("Export", "Export this texture to the project as an asset?"), EditorStyles.miniButton, GUILayout.ExpandWidth(false)) == true)
				{
					var path = AssetDatabase.GUIDToAssetPath(paintableTexture.Output);
					var name = paintableTexture.name + "_" + paintableTexture.Slot.Name;
					var dir  = string.IsNullOrEmpty(path) == false ? System.IO.Path.GetDirectoryName(path) : "Assets";

					if (string.IsNullOrEmpty(path) == false)
					{
						name = System.IO.Path.GetFileNameWithoutExtension(path);
					}

					path = EditorUtility.SaveFilePanelInProject("Export Texture", name, "png", "Export Your Texture", dir);

					if (string.IsNullOrEmpty(path) == false)
					{
						System.IO.File.WriteAllBytes(path, paintableTexture.GetPngData(true));

						AssetDatabase.ImportAsset(path);

						Undo.RecordObject(paintableTexture, "Output Changed");

						paintableTexture.Output = AssetDatabase.AssetPathToGUID(path);

						EditorUtility.SetDirty(this);
					}
				}
			EditorGUILayout.EndHorizontal();

			CwEditor.BeginLabelWidth(100);
				EditorGUI.indentLevel++;
					EditorGUILayout.BeginHorizontal();
						var outputTexture    = paintableTexture.OutputTexture;
						var newOutputTexture = EditorGUILayout.ObjectField(outputTexture, typeof(Texture2D), false);

						EditorGUI.BeginDisabledGroup(outputTexture == null || paintableTexture.Activated == false);
							if (GUILayout.Button(new GUIContent("Load", "Load the previously exported texture into this texture slot?"), EditorStyles.miniButton, GUILayout.ExpandWidth(false)) == true)
							{
								if (EditorUtility.DisplayDialog("Are you sure?", "This will replace this paintable texture with the currently exported texture state.", "OK") == true)
								{
									paintableTexture.Replace(outputTexture, Color.white);

									paintableTexture.Texture = outputTexture;
								}
							}
						EditorGUI.EndDisabledGroup();
					EditorGUILayout.EndHorizontal();

					if (outputTexture != newOutputTexture)
					{
						Undo.RecordObject(paintableTexture, "Output Changed");

						paintableTexture.Output = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(newOutputTexture));

						EditorUtility.SetDirty(this);
					}

					if (paintableTexture.UndoRedo == P3dPaintableTexture.UndoRedoType.None)
					{
						EditorGUILayout.HelpBox("This texture has no UndoRedo set, so you cannot undo or redo.", MessageType.Warning);
					}

					if (outputTexture == null)
					{
						EditorGUILayout.HelpBox("This texture hasn't been exported yet, so you cannot Export All.", MessageType.Warning);
					}
				EditorGUI.indentLevel--;
			CwEditor.EndLabelWidth();
		}

		private bool CanLoadAll(P3dPaintableTexture[] paintableTextures)
		{
			foreach (var paintableTexture in paintableTextures)
			{
				if (paintableTexture.Activated == true && paintableTexture.OutputTexture != null)
				{
					return true;
				}
			}

			return false;
		}

		private bool CanReExportAll(P3dPaintableTexture[] paintableTextures)
		{
			foreach (var paintableTexture in paintableTextures)
			{
				if (paintableTexture.OutputTexture != null)
				{
					return true;
				}
			}

			return false;
		}

		private void DrawSceneFooter(P3dPaintableTexture[] paintableTextures)
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
				EditorGUILayout.Separator();

				EditorGUI.BeginDisabledGroup(CanLoadAll(paintableTextures) == false);
					if (GUILayout.Button(new GUIContent("Load All", "Load all previously exported texture into their texture slots."), EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)) == true)
					{
						if (EditorUtility.DisplayDialog("Are you sure?", "This will replace all paintable textures with their currently exported texture state.", "OK") == true)
						{
							foreach (var paintableTexture in paintableTextures)
							{
								var outputTexture = paintableTexture.OutputTexture;

								if (outputTexture != null)
								{
									paintableTexture.Replace(outputTexture, Color.white);

									paintableTexture.Texture = outputTexture;
								}
							}
						}
					}
				EditorGUI.EndDisabledGroup();
				EditorGUI.BeginDisabledGroup(CanReExportAll(paintableTextures) == false);
					CwEditor.BeginColor(Color.green);
						if (GUILayout.Button("Re-Export All", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)) == true)
						{
							if (EditorUtility.DisplayDialog("Are you sure?", "This will re-export all paintable textures in your scene.", "OK") == true)
							{
								foreach (var paintableTexture in paintableTextures)
								{
									if (paintableTexture.OutputTexture != null)
									{
										System.IO.File.WriteAllBytes(AssetDatabase.GUIDToAssetPath(paintableTexture.Output), paintableTexture.GetPngData(true));
									}
								}

								AssetDatabase.Refresh();
							}
						}
					CwEditor.EndColor();
				EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndHorizontal();
		}
	}
}
#endif