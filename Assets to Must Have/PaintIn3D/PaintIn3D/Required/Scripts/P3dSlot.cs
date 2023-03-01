using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This struct stores a reference to a texture on a GameObject.</summary>
	[System.Serializable]
	public struct P3dSlot
	{
		/// <summary>The material index in the attached renderer.</summary>
		public int Index;

		/// <summary>The name of the texture in the specified material.</summary>
		public string Name;

		public P3dSlot(int newIndex, string newName)
		{
			Index = newIndex;
			Name  = newName;
		}

#if UNITY_EDITOR
		public string GetTitle(Material material)
		{
			if (material != null)
			{
				var shader = material.shader;

				if (shader != null)
				{
					foreach (var texEnv in P3dCommon.GetTexEnvs(shader))
					{
						if (texEnv.Name == Name)
						{
							return texEnv.Title;
						}
					}
				}
			}

			return Name;
		}
#endif

		public Texture FindTexture(GameObject gameObject)
		{
			if (gameObject != null)
			{
				var paintable = gameObject.GetComponentInParent<P3dPaintable>();

				if (paintable != null)
				{
					var material = P3dCommon.GetMaterial(paintable.CachedRenderer, Index);

					if (material != null && material.HasProperty(Name) == true)
					{
						return material.GetTexture(Name);
					}
				}
			}

			return null;
		}

		public bool IsTransformed(GameObject gameObject)
		{
			if (gameObject != null)
			{
				var paintable = gameObject.GetComponentInParent<P3dPaintable>();

				if (paintable != null)
				{
					var material = P3dCommon.GetMaterial(paintable.CachedRenderer, Index);

					if (material != null)
					{
						if (material.GetTextureScale(Name) != Vector2.one || material.GetTextureOffset(Name) != Vector2.zero)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode() ^ Name.GetHashCode();
		}

		public static bool operator == (P3dSlot a, P3dSlot b)
		{
			return a.Index == b.Index && a.Name == b.Name;
		}

		public static bool operator != (P3dSlot a, P3dSlot b)
		{
			return a.Index != b.Index || a.Name != b.Name;
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;

	[CustomPropertyDrawer(typeof(P3dSlot))]
	public class P3dSlot_Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var sObj      = property.serializedObject;
			var sIdx      = property.FindPropertyRelative("Index");
			var sNam      = property.FindPropertyRelative("Name");
			var rectA     = position; rectA.width = EditorGUIUtility.labelWidth;
			var rectB     = position; rectB.xMin += EditorGUIUtility.labelWidth; rectB.width = 20;
			var rectC     = position; rectC.xMin += EditorGUIUtility.labelWidth; rectC.xMin += 22; rectC.width -= 20;
			var rectD     = position; rectD.xMin = rectD.xMax - 18;
			var component = property.serializedObject.targetObject as Component;
			var paintable = component != null ? component.GetComponentInParent<P3dPaintable>() : null;
			var missing   = true;

			// Valid slot?
			if (paintable != null)
			{
				var material = P3dCommon.GetMaterial(paintable.CachedRenderer, sIdx.intValue);

				if (material != null && P3dCommon.TexEnvNameExists(material.shader, sNam.stringValue) == true)
				{
					missing = false;
				}
			}

			CwEditor.BeginError(missing);
			{
				EditorGUI.LabelField(rectA, label);

				sObj.Update();

				sIdx.intValue    = Mathf.Clamp(EditorGUI.IntField(rectB, sIdx.intValue), 0, 99);
				sNam.stringValue = EditorGUI.TextField(rectC, sNam.stringValue);

				sObj.ApplyModifiedProperties();

				// Draw menu
				if (GUI.Button(rectD, "", EditorStyles.popup) == true)
				{
					var menu = new GenericMenu();

					if (paintable != null)
					{
						var materials = paintable.CachedRenderer.sharedMaterials;

						if (materials.Length > 0)
						{
							for (var i = 0; i < materials.Length; i++)
							{
								var material     = materials[i];
								var materialName = i.ToString();
								var matIndex     = i;

								if (material != null)
								{
									materialName += " (" + material.name + ")";

									var texEnvs = P3dCommon.GetTexEnvs(material.shader);

									if (texEnvs != null && texEnvs.Count > 0)
									{
										foreach (var texEnv in texEnvs)
										{
											var texName  = texEnv.Name;
											var texTitle = texEnv.Title;
											var tex      = material.GetTexture(texName);

											if (tex != null)
											{
												texTitle += " (" + tex.name + ")";
											}
											else
											{
												texTitle += " (empty)";
											}

											menu.AddItem(new GUIContent(materialName + "/" + texTitle), sIdx.intValue == matIndex && sNam.stringValue == texName, () => { sObj.Update(); sIdx.intValue = matIndex; sNam.stringValue = texName; sObj.ApplyModifiedProperties(); });
										}
									}
									else
									{
										menu.AddDisabledItem(new GUIContent(materialName + "/This Material's shader has no textures!"));
									}
								}
								else
								{
									menu.AddDisabledItem(new GUIContent(materialName + "/This Material is null!"));
								}
							}
						}
						else
						{
							menu.AddDisabledItem(new GUIContent("This GameObject has no materials!"));
						}
					}

					menu.DropDown(rectD);
				}
			}
			CwEditor.EndError();
		}
	}
}
#endif