using System.Collections.Generic;
using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component can be used to create shape prefabs for in-editor painting. These will automatically appear in the Paint tab's Shape list.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dShape")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Shape")]
	public class P3dShape : MonoBehaviour, IBrowsable
	{
		public string Category { set { category = value; } get { return category; } } [SerializeField] private string category;

		public Texture2D Icon { set { icon = value; } get { return icon; } } [SerializeField] private Texture2D icon;

		private static List<P3dShape> cachedShapes;

		public static List<P3dShape> CachedShapes
		{
			get
			{
				if (cachedShapes == null)
				{
					cachedShapes = new List<P3dShape>();
#if UNITY_EDITOR
					var scriptGuid  = P3dCommon.FindScriptGUID<P3dShape>();

					if (scriptGuid != null)
					{
						foreach (var prefabGuid in UnityEditor.AssetDatabase.FindAssets("t:prefab"))
						{
							var shape = P3dCommon.LoadPrefabIfItContainsScriptGUID<P3dShape>(prefabGuid, scriptGuid);

							if (shape != null)
							{
								cachedShapes.Add(shape);
							}
						}
					}
#endif
				}

				return cachedShapes;
			}
		}

		public static void ClearCache()
		{
			cachedShapes = null;
		}

		public string GetCategory()
		{
			return category;
		}

		public string GetTitle()
		{
			return name;
		}

		public Texture2D GetIcon()
		{
			return icon;
		}

		public Object GetObject()
		{
			return this;
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dShape;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dShape_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			if (P3dShape.CachedShapes.Contains(tgt) == false && CwHelper.IsAsset(tgt) == true)
			{
				P3dShape.CachedShapes.Add(tgt);
			}

			Draw("category");
			Draw("icon");
		}

		[MenuItem("Assets/Create/Paint in 3D/Shape")]
		private static void CreateAsset()
		{
			var brush = new GameObject("Shape").AddComponent<P3dShape>();
			var guids = Selection.assetGUIDs;
			var path  = guids.Length > 0 ? AssetDatabase.GUIDToAssetPath(guids[0]) : null;

			if (string.IsNullOrEmpty(path) == true)
			{
				path = "Assets";
			}
			else if (AssetDatabase.IsValidFolder(path) == false)
			{
				path = System.IO.Path.GetDirectoryName(path);
			}

			var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/NewBrush.prefab");
			var asset            = PrefabUtility.SaveAsPrefabAsset(brush.gameObject, assetPathAndName);

			DestroyImmediate(brush.gameObject);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow();

			Selection.activeObject = asset; EditorGUIUtility.PingObject(asset);
		}
	}
}
#endif