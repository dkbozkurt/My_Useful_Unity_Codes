using System.Collections.Generic;
using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component can be used to create tool prefabs for in-editor painting. These will automatically appear in the Paint tab's Tool list.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dTool")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Tool")]
	public class P3dTool : MonoBehaviour, IBrowsable
	{
		public string Category { set { category = value; } get { return category; } } [SerializeField] private string category;

		public Texture2D Icon { set { icon = value; } get { return icon; } } [SerializeField] private Texture2D icon;

		private static List<P3dTool> cachedTools;

		public static List<P3dTool> CachedTools
		{
			get
			{
				if (cachedTools == null)
				{
					cachedTools = new List<P3dTool>();
#if UNITY_EDITOR
					var scriptGuid = P3dCommon.FindScriptGUID<P3dTool>();

					if (scriptGuid != null)
					{
						foreach (var prefabGuid in UnityEditor.AssetDatabase.FindAssets("t:prefab"))
						{
							var tool = P3dCommon.LoadPrefabIfItContainsScriptGUID<P3dTool>(prefabGuid, scriptGuid);

							if (tool != null)
							{
								cachedTools.Add(tool);
							}
						}
					}
#endif
				}

				return cachedTools;
			}
		}

		public static void ClearCache()
		{
			cachedTools = null;
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
	using TARGET = P3dTool;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dTool_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			if (P3dTool.CachedTools.Contains(tgt) == false && CwHelper.IsAsset(tgt) == true)
			{
				P3dTool.CachedTools.Add(tgt);
			}

			Draw("category");
			Draw("icon");
		}

		[MenuItem("Assets/Create/Paint in 3D/Tool")]
		private static void CreateAsset()
		{
			var tool  = new GameObject("Tool").AddComponent<P3dTool>();
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

			var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/NewTool.prefab");

			var asset = PrefabUtility.SaveAsPrefabAsset(tool.gameObject, assetPathAndName);

			DestroyImmediate(tool.gameObject);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow();

			Selection.activeObject = asset; EditorGUIUtility.PingObject(asset);
		}
	}
}
#endif