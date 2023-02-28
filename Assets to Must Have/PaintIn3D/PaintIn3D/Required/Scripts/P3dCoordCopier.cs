using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This tool allows you to copy UV1 data into UV0. This is useful if you let Unity automatically generate lightmap UV data for you and you want to use them to paint normally.</summary>
	[ExecuteInEditMode]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dCoordCopier")]
	public class P3dCoordCopier : ScriptableObject
	{
		public enum Coord
		{
			First,
			Second,
			Third,
			Fourth,
			None
		}

		/// <summary>The original mesh whose UV seams you want to fix.</summary>
		public Mesh Source { set { source = value; } get { return source; } } [SerializeField] private Mesh source;

		/// <summary>The coord that will be copied into the first UV channel of the output mesh.</summary>
		public Coord First { set { first = value; } get { return first; } } [SerializeField] private Coord first = Coord.Second;

		/// <summary>The coord that will be copied into the second UV channel of the output mesh.</summary>
		public Coord Second { set { second = value; } get { return second; } } [SerializeField] private Coord second = Coord.None;

		/// <summary>The coord that will be copied into the third UV channel of the output mesh.</summary>
		public Coord Third { set { third = value; } get { return third; } } [SerializeField] private Coord third = Coord.None;

		/// <summary>The coord that will be copied into the fourth UV channel of the output mesh.</summary>
		public Coord Fourth { set { fourth = value; } get { return fourth; } } [SerializeField] private Coord fourth = Coord.None;

		[SerializeField]
		private Mesh mesh;

		[System.NonSerialized]
		private static List<BoneWeight> boneWeights = new List<BoneWeight>();

		[System.NonSerialized]
		private static List<Color32> colors = new List<Color32>();

		[System.NonSerialized]
		private static List<Vector3> positions = new List<Vector3>();

		[System.NonSerialized]
		private static List<Vector3> normals = new List<Vector3>();

		[System.NonSerialized]
		private static List<Vector4> tangents = new List<Vector4>();

		[System.NonSerialized]
		private static List<Vector4> coords0 = new List<Vector4>();

		[System.NonSerialized]
		private static List<Vector4> coords1 = new List<Vector4>();

		[System.NonSerialized]
		private static List<Vector4> coords2 = new List<Vector4>();

		[System.NonSerialized]
		private static List<Vector4> coords3 = new List<Vector4>();

		[System.NonSerialized]
		private static List<Vector4> coordsNone = new List<Vector4>();

		[System.NonSerialized]
		private static List<int> indices = new List<int>();

		public List<Vector4> GetCoords(Coord coord)
		{
			switch (coord)
			{
				case Coord.First:  return coords0;
				case Coord.Second: return coords1;
				case Coord.Third:  return coords2;
				case Coord.Fourth: return coords3;
			}

			return coordsNone;
		}

		public void Generate()
		{
			if (source != null)
			{
				if (mesh == null)
				{
					mesh = new Mesh();
				}

				mesh.Clear(false);
				mesh.name = source.name + " (Copied Coords)";

				mesh.bindposes = source.bindposes;
				mesh.bounds = source.bounds;
				mesh.subMeshCount = source.subMeshCount;

				source.GetBoneWeights(boneWeights);
				source.GetColors(colors);
				source.GetNormals(normals);
				source.GetTangents(tangents);
				source.GetUVs(0, coords0);
				source.GetUVs(1, coords1);
				source.GetUVs(2, coords2);
				source.GetUVs(3, coords3);
				source.GetVertices(positions);

				mesh.SetVertices(positions);

				for (var i = 0; i < source.subMeshCount; i++)
				{
					source.GetTriangles(indices, i);

					mesh.SetTriangles(indices, i);
				}

				mesh.boneWeights = boneWeights.ToArray();
				mesh.SetColors(colors);
				mesh.SetNormals(normals);
				mesh.SetTangents(tangents);

				mesh.SetUVs(0, GetCoords(first ));
				mesh.SetUVs(1, GetCoords(second));
				mesh.SetUVs(2, GetCoords(third ));
				mesh.SetUVs(3, GetCoords(fourth));

				if (source.blendShapeCount > 0)
				{
					var deltaVertices = new Vector3[source.vertexCount];
					var deltaNormals  = new Vector3[source.vertexCount];
					var deltaTangents = new Vector3[source.vertexCount];

					for (var i = 0; i < source.blendShapeCount; i++)
					{
						var shapeName  = source.GetBlendShapeName(i);
						var frameCount = source.GetBlendShapeFrameCount(i);

						for (var j = 0; j < frameCount; j++)
						{
							source.GetBlendShapeFrameVertices(i, j, deltaVertices, deltaNormals, deltaTangents);

							mesh.AddBlendShapeFrame(shapeName, source.GetBlendShapeFrameWeight(i, j), deltaVertices, deltaNormals, deltaTangents);
						}
					}
				}

#if UNITY_EDITOR
				if (CwHelper.IsAsset(this) == true)
				{
					var assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(UnityEditor.AssetDatabase.GetAssetPath(this));

					for (var i = 0; i < assets.Length; i++)
					{
						var assetMesh = assets[i] as Mesh;

						if (assetMesh != null && assetMesh != mesh)
						{
							DestroyImmediate(assetMesh, true);
						}
					}

					if (CwHelper.IsAsset(mesh) == false)
					{
						UnityEditor.AssetDatabase.AddObjectToAsset(mesh, this);

						UnityEditor.AssetDatabase.SaveAssets();
					}
				}
#endif
			}

#if UNITY_EDITOR
			if (CwHelper.IsAsset(this) == true)
			{
				CwHelper.ReimportAsset(this);
			}
#endif
		}

		protected virtual void OnDestroy()
		{
			CwHelper.Destroy(mesh);
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dCoordCopier;

	[CustomEditor(typeof(TARGET))]
	public class P3dCoordCopier_Editor : CwEditor
	{
		[MenuItem("CONTEXT/Mesh/Coord Copier (Paint in 3D)")]
		public static void Create(MenuCommand menuCommand)
		{
			var mesh = menuCommand.context as Mesh;

			if (mesh != null)
			{
				var path = AssetDatabase.GetAssetPath(mesh);

				if (string.IsNullOrEmpty(path) == false)
				{
					path = System.IO.Path.GetDirectoryName(path);
				}
				else
				{
					path = "Assets";
				}

				path += "/Coord Copier (" + mesh.name + ").asset";

				var instance = CreateInstance<P3dCoordCopier>();

				instance.Source = mesh;

				ProjectWindowUtil.CreateAsset(instance, path);
			}
		}

		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Info("This tool will copy the UV1 data (e.g. lightmap UVs) into UV0, so you can use it with normal painting. The fixed mesh will be placed as a child of this tool in your Project window. To use the fixed mesh, drag and drop it into your MeshFilter or SkinnedMeshRenderer.");

			Separator();

			BeginError(Any(tgts, t => t.Source == null));
				Draw("source", "The original mesh whose UV data you want to copy.");
			EndError();

			Separator();

			Draw("first", "The coord that will be copied into the first UV channel of the output mesh.");
			Draw("second", "The coord that will be copied into the second UV channel of the output mesh.");
			Draw("third", "The coord that will be copied into the third UV channel of the output mesh.");
			Draw("fourth", "The coord that will be copied into the fourth UV channel of the output mesh.");

			Separator();

			if (Button("Generate") == true)
			{
				Each(tgts, t => t.Generate());
			}
		}
	}
}
#endif