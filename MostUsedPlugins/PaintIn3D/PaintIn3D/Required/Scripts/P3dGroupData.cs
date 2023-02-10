using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This object allows you to define information about a paint group like its name, which can then be selected using the <b>P3dGroup</b> setting on components like <b>P3dPaintableTexture</b> and <b>P3dPaintDecal</b>.</summary>
	public class P3dGroupData : ScriptableObject
	{
		[System.Serializable]
		public class TextureData
		{
			public string Name;

			public P3dBlendMode BlendMode = P3dBlendMode.AlphaBlend(Vector4.one);
		}

		class Entry
		{
			public string Path;

			public string Property;
		}

		/// <summary>This allows you to set the ID of this group (e.g. 100).
		/// NOTE: This number should be unique, and not shared by any other <b>P3dGroupData</b>.</summary>
		public int Index { set { index = value; } get { return index; } } [SerializeField] private int index;

		/// <summary>This allows you to specify the way each channel of this group's pixels are mapped to textures. This is mainly used by the in-editor painting material builder tool.</summary>
		public List<TextureData> TextureDatas { get { if (textureDatas == null) textureDatas = new List<TextureData>(); return textureDatas; } } [SerializeField] private List<TextureData> textureDatas;

		/// <summary>This allows you to specify which shaders and their properties are associated with this group.</summary>
		public string ShaderData { set { shaderData = value; } get { return shaderData; } } [SerializeField] [Multiline(10)] private string shaderData;

		private List<Entry> entries = new List<Entry>();

		public void TryGetShaderSlotName(string shaderPath, ref string propertyName)
		{
			if (entries.Count == 0 && shaderData != null)
			{
				var lines = shaderData.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

				foreach (var line in lines)
				{
					var divider = line.IndexOf("@");

					if (divider > 0)
					{
						var entry = new Entry();

						entry.Property = line.Substring(0, divider);
						entry.Path     = line.Substring(divider + 1);

						entries.Add(entry);
					}
				}
			}

			foreach (var entry in entries)
			{
				if (entry.Path == shaderPath)
				{
					propertyName = entry.Property;

					return;
				}
			}
		}

		public bool Supports(Shader shader)
		{
			return shaderData != null && shaderData.Contains("@" + shader.name) == true;
		}

		/// <summary>This method allows you to get the <b>name</b> of the current group, with an optional prefix of the <b>Index</b> (e.g. "100: Albedo").</summary>
		public string GetName(bool prefixNumber)
		{
			if (prefixNumber == true)
			{
				return index + ": " + name;
			}

			return name;
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dGroupData;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dGroupData_Editor : CwEditor
	{
		class Entry
		{
			public string Path;

			public List<P3dCommon.TexEnv> TexEnvs = new List<P3dCommon.TexEnv>();
		}

		private string filter;

		private bool clean;

		private List<Entry> entries = new List<Entry>();

		private HashSet<string> uniques = new HashSet<string>();

		private static List<P3dGroupData> cachedInstances = new List<P3dGroupData>();

		private static bool cachedInstancesSet;

		/// <summary>This static method calls <b>GetAlias</b> on the <b>P3dGroupData</b> with the specified <b>Index</b> setting, or null.</summary>
		public static string GetGroupName(int index, bool prefixNumber)
		{
			var groupData = GetGroupData(index);

			return groupData != null ? groupData.GetName(prefixNumber) : null;
		}

		/// <summary>This static method returns the <b>P3dGroupData</b> with the specified <b>Index</b> setting, or null.</summary>
		public static P3dGroupData GetGroupData(int index)
		{
			foreach (var cachedGroupName in CachedInstances)
			{
				if (cachedGroupName != null && cachedGroupName.Index == index)
				{
					return cachedGroupName;
				}
			}

			return null;
		}

		/// <summary>This static method forces the cached instance list to update.
		/// NOTE: This does nothing in-game.</summary>
		public static void UpdateCachedInstances()
		{
			cachedInstancesSet = true;

			cachedInstances.Clear();

			foreach (var guid in UnityEditor.AssetDatabase.FindAssets("t:P3dGroupData"))
			{
				var groupName = UnityEditor.AssetDatabase.LoadAssetAtPath<P3dGroupData>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));

				cachedInstances.Add(groupName);
			}
		}

		/// <summary>This static property returns a list of all cached <b>P3dGroupData</b> instances.
		/// NOTE: This will be empty in-game.</summary>
		public static List<P3dGroupData> CachedInstances
		{
			get
			{
				if (cachedInstancesSet == false)
				{
					UpdateCachedInstances();
				}

				return cachedInstances;
			}
		}

		protected virtual void OnEnable()
		{
			UpdateCachedInstances();
		}

		private void CheckForDuplicates(TARGET tgt)
		{
			if (tgt.ShaderData != null)
			{
				var lines = tgt.ShaderData.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

				uniques.Clear();

				foreach (var line in lines)
				{
					var middle = line.IndexOf('@');

					if (middle >= 0)
					{
						var right = line.Substring(middle);

						if (uniques.Add(right) == false)
						{
							Error("There are multiple entries for " + right);
						}
					}
				}
			}
		}

		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			var clashes = CachedInstances.Where(d => d.Index == tgt.Index);

			BeginError(clashes.Count() > 1);
				Draw("index", "This allows you to set the ID of this group (e.g. 100).\n\nNOTE: This number should be unique, and not shared by any other P3dGroupData.");
			EndError();
			Draw("textureDatas", "This allows you to specify the way each channel of this group's pixels are mapped to textures. This is mainly used by the in-editor painting material builder tool.", "Components");

			Separator();

			EditorGUILayout.LabelField("Shader Texture Associations", EditorStyles.boldLabel);
			BeginDisabled();
				EditorGUILayout.TextField("Format", "TEXTURE_NAME@SHADER_PATH");
			EndDisabled();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("shaderData"), GUIContent.none);
			//Draw("shaderData", "This allows you to specify which shaders and their properties are associated with this group.");


			CheckForDuplicates(tgt);

			Separator();

			EditorGUILayout.LabelField("Current Groups", EditorStyles.boldLabel);

			var groupDatas = CachedInstances.OrderBy(d => d.Index);

			BeginDisabled(true);
				foreach (var groupData in groupDatas)
				{
					if (groupData != null)
					{
						EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField(groupData.name);
							EditorGUILayout.IntField(groupData.Index);
						EditorGUILayout.EndHorizontal();
					}
				}
			EndDisabled();

			Separator();

			EditorGUILayout.LabelField("Shader Properties", EditorStyles.boldLabel);

			filter = EditorGUILayout.TextField("Filter", filter);
			clean  = EditorGUILayout.Toggle("Clean", clean);

			var text = "";

			if (string.IsNullOrEmpty(filter) == false)
			{
				var tokens = filter.Split(' ');

				if (entries.Count == 0)
				{
					foreach (var shaderInfo in ShaderUtil.GetAllShaderInfo())
					{
						var entry = new Entry();

						entry.Path    = shaderInfo.name;
						entry.TexEnvs.AddRange(P3dCommon.GetTexEnvs(Shader.Find(shaderInfo.name)));

						entries.Add(entry);
					}
				}

				foreach (var entry in entries)
				{
					foreach (var texEnv in entry.TexEnvs)
					{
						foreach (var token in tokens)
						{
							if (string.IsNullOrEmpty(token) == false)
							{
								if (Contains(texEnv.Name, token) == true || Contains(texEnv.Desc, token) == true || Contains(entry.Path, token) == true)
								{
									var line = "";

									if (clean == true)
									{
										line += texEnv.Name + "@" + entry.Path + "\n";
									}
									else
									{
										line += texEnv.Name + " - " + texEnv.Desc + " - " + entry.Path + "\n";
									}

									if (text.Contains(line) == false)
									{
										text += line;
									}

									continue;
								}
							}
						}
					}
				}

				EditorGUILayout.TextArea(text, GUILayout.ExpandHeight(true));
			}
		}

		private bool Contains(string paragraph, string word)
		{
			return System.Globalization.CultureInfo.InvariantCulture.CompareInfo.IndexOf(paragraph, word,  System.Globalization.CompareOptions.IgnoreCase) >= 0;
		}

		[MenuItem("Assets/Create/Paint in 3D/Group Data")]
		private static void CreateAsset()
		{
			var asset = CreateInstance<P3dGroupData>();
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

			var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(P3dGroupData).ToString() + ".asset");

			AssetDatabase.CreateAsset(asset, assetPathAndName);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow();

			Selection.activeObject = asset; EditorGUIUtility.PingObject(asset);

			cachedInstances.Add(asset);
		}
	}
}
#endif