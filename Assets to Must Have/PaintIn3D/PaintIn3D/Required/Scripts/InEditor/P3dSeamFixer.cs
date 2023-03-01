using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This tool allows you to convert a normal mesh with UV seams to a fixed mesh without UV seams.
	/// This tool can be accessed from the context menu (⋮ button at top right) of any mesh/model inspector.</summary>
	[ExecuteInEditMode]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dSeamFixer")]
	public class P3dSeamFixer : ScriptableObject
	{
		[System.Serializable]
		public class Pair
		{
			/// <summary>The original mesh.</summary>
			public Mesh Source;

			/// <summary>The mesh with fixed seams.</summary>
			public Mesh Output;
		}

		private class Ring
		{
			public List<Point> Points = new List<Point>();

			public Point GetPoint(int index)
			{
				if (index < 0)
				{
					index = Points.Count - 1;
				}
				else if (index >= Points.Count)
				{
					index = 0;
				}

				return Points[index];
			}
		}

		private class Edge
		{
			public bool  Used;
			public Point PointA;
			public Point PointB;
			public bool  Flip;

			public bool Match(Point a, Point b)
			{
				if (PointA == a && PointB == b) return true;
				if (PointA == b && PointB == a) return true;

				return false;
			}
		}

		private class Point
		{
			public int       Index;
			public Vector2   Coord;
			public Insertion Outer;

			public List<Edge> Edges = new List<Edge>();
		}

		private class Insertion
		{
			public int     Index;
			public int     NewIndex;
			public Vector2 NewCoord;
		}

		// LEGACY
		[SerializeField] private Mesh source;

		// LEGACY
		[SerializeField] private Mesh mesh;

		/// <summary>The meshes we will fix the seams of.</summary>
		public List<Pair> Meshes { get { if (meshes == null) meshes = new List<Pair>(); return meshes; } } [SerializeField] private List<Pair> meshes;

		/// <summary>The UV channel whose seams will be fixed.</summary>
		public P3dCoord Coord { set { coord = value; } get { return coord; } } [SerializeField] private P3dCoord coord;

		/// <summary>The threshold below which vertex UV coordinates will be snapped.</summary>
		public float Threshold { set { threshold = value; } get { return threshold; } } [SerializeField] private float threshold = 0.000001f;

		/// <summary>The thickness of the UV borders in the fixed mesh.</summary>
		public float Border { set { border = value; } get { return border; } } [SerializeField] private float border = 0.005f;

		private static Dictionary<Mesh, Mesh> cacheFirst   = new Dictionary<Mesh, Mesh>();
		private static Dictionary<Mesh, Mesh> cacheSecond  = new Dictionary<Mesh, Mesh>();
		private static Dictionary<Mesh, Mesh> cacheThird   = new Dictionary<Mesh, Mesh>();
		private static Dictionary<Mesh, Mesh> cacheFourth  = new Dictionary<Mesh, Mesh>();

		public static Mesh GetCachedMesh(Mesh source, P3dCoord coord, bool allowGeneration = true)
		{
			switch (coord)
			{
				case P3dCoord.First:  return TryGetCachedMesh(cacheFirst , source, coord, allowGeneration);
				case P3dCoord.Second: return TryGetCachedMesh(cacheSecond, source, coord, allowGeneration);
				case P3dCoord.Third:  return TryGetCachedMesh(cacheThird , source, coord, allowGeneration);
				case P3dCoord.Fourth: return TryGetCachedMesh(cacheFourth, source, coord, allowGeneration);
			}

			return default(Mesh);
		}

		private static Mesh TryGetCachedMesh(Dictionary<Mesh, Mesh> cache, Mesh source, P3dCoord coord, bool allowGeneration = true)
		{
			var fixedMesh = default(Mesh);

			if (source != null && cache.TryGetValue(source, out fixedMesh) == false && allowGeneration == true)
			{
				fixedMesh = new Mesh();

				fixedMesh.name = source.name + " (Auto Fixed Seams)";

				Generate(source, fixedMesh, coord, 0.000001f, 0.005f);

				cache.Add(source, fixedMesh);
			}

			return fixedMesh;
		}

		/// <summary>This allows you to add a mesh to the seam fixer.
		/// NOTE: You must later call <b>Generate</b> to seam fix the added meshes.</summary>
		public void AddMesh(Mesh mesh)
		{
			if (mesh != null)
			{
				Meshes.Add(new Pair() { Source = mesh });
			}
		}

		public void ConvertLegacy()
		{
			if (source != null)
			{
				Meshes.Add(new Pair() { Source = source, Output = mesh });

				source = null;
				mesh   = null;
			}
		}

		[ContextMenu("Generate")]
		public void Generate()
		{
#if UNITY_EDITOR
			UnityEditor.Undo.RecordObject(this, "Generate Seam Fix");
#endif

			if (meshes != null)
			{
				foreach (var pair in meshes)
				{
					if (pair.Source != null)
					{
						if (pair.Output == null)
						{
							pair.Output = new Mesh();
						}

						pair.Output.name = pair.Source.name + " (Fixed Seams)";

						Generate(pair.Source, pair.Output, coord, threshold, border);
					}
					else
					{
						DestroyImmediate(pair.Output);

						pair.Output = null;
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

					if (assetMesh != null)
					{
						if (meshes == null || meshes.Exists(p => p.Output == assetMesh) == false)
						{
							DestroyImmediate(assetMesh, true);
						}
					}
				}

				if (meshes != null)
				{
					foreach (var pair in meshes)
					{
						if (pair.Output != null && CwHelper.IsAsset(pair.Output) == false)
						{
							UnityEditor.AssetDatabase.AddObjectToAsset(pair.Output, this);

							UnityEditor.AssetDatabase.SaveAssets();
						}
					}
				}
			}

			if (CwHelper.IsAsset(this) == true)
			{
				CwHelper.ReimportAsset(this);
			}

			UnityEditor.EditorUtility.SetDirty(this);
#endif
		}

		/// <summary>This static method allows you to fix the seams of the source mesh at runtime.</summary>
		public static void Generate(Mesh source, Mesh output, P3dCoord coord, float threshold, float border)
		{
			if (source != null && output != null && border != 0.0f)
			{
				output.Clear(false);

				var points     = new Dictionary<Vector2, Point>();
				var insertions = new List<Insertion>();
				var submeshes  = new List<List<int>>();
				var coords     = default(Vector2[]);

				switch (coord)
				{
					case P3dCoord.First : coords = source.uv ; break;
					case P3dCoord.Second: coords = source.uv2; break;
					case P3dCoord.Third : coords = source.uv3; break;
					case P3dCoord.Fourth: coords = source.uv4; break;
				}

				for (var i = 0; i < coords.Length; i++)
				{
					var uv = coords[i];

					uv *= 16384.0f;
					uv.x = Mathf.Floor(uv.x);
					uv.y = Mathf.Floor(uv.y);
					uv /= 16384.0f;

					coords[i] = uv;

					AddPoint(points, uv, i);
				}

				var rings       = new List<Ring>();
				var vertexIndex = source.vertexCount;

				for (var i = 0; i < source.subMeshCount; i++)
				{
					var edges   = new List<Edge>();
					var indices = new List<int>(); source.GetTriangles(indices, i);

					submeshes.Add(indices);

					foreach (var pair in points)
					{
						pair.Value.Edges.Clear();
					}

					if (coords.Length > 0)
					{
						for (var j = 0; j < indices.Count; j += 3)
						{
							var pointA = points[coords[indices[j + 0]]];
							var pointB = points[coords[indices[j + 1]]];
							var pointC = points[coords[indices[j + 2]]];

							AddTriangle(edges, pointA, pointB, pointC);
						}
					}

#if UNITY_EDITOR
					if (P3dSeamFixer_Editor.DebugScale > 0.0f)
					{
						for (var j = edges.Count - 1; j >= 0; j--)
						{
							var edge = edges[j];

							if (edge.Used == true)
							{
								Debug.DrawLine(edge.PointA.Coord * P3dSeamFixer_Editor.DebugScale, edge.PointB.Coord * P3dSeamFixer_Editor.DebugScale, new Color(1.0f, 0.0f, 0.0f, 0.5f), 1.0f);
							}
							else
							{
								Debug.DrawLine(edge.PointA.Coord * P3dSeamFixer_Editor.DebugScale, edge.PointB.Coord * P3dSeamFixer_Editor.DebugScale, new Color(1.0f, 1.0f, 1.0f, 0.5f), 1.0f);
							}
						}
					}
#endif
					for (var j = edges.Count - 1; j >= 0; j--)
					{
						var edge = edges[j];

						if (edge.Used == false)
						{
							edge.Used = true;

							var ring = new Ring();

							ring.Points.Add(edge.PointA);
							ring.Points.Add(edge.PointB);

							TraceEdges(ring, edge.PointB);

							if (ring.Points.Count > 2)
							{
								rings.Add(ring);

								for (var k = 0; k < ring.Points.Count; k++)
								{
									var pointA = ring.GetPoint(k - 1);
									var pointB = ring.GetPoint(k    );
									var pointC = ring.GetPoint(k + 1);

									var normalA   = (pointA.Coord - pointB.Coord).normalized; normalA = new Vector2(-normalA.y, normalA.x);
									var normalB   = (pointB.Coord - pointC.Coord).normalized; normalB = new Vector2(-normalB.y, normalB.x);
									var average   = normalA + normalB;
									var magnitude = average.sqrMagnitude;
									var insertion = new Insertion();

									insertion.Index    = pointB.Index;
									insertion.NewCoord = pointB.Coord;
									insertion.NewIndex = vertexIndex++;

									pointB.Outer = insertion;

									if (magnitude > 0.0f)
									{
										magnitude = Mathf.Sqrt(magnitude);

										insertion.NewCoord += (average / magnitude) * border;
									}

									insertions.Add(insertion);
								}

								for (var k = 0; k < ring.Points.Count; k++)
								{
									var pointA = ring.GetPoint(k    );
									var pointB = ring.GetPoint(k + 1);
									var indexA = pointA.Index;
									var indexB = pointB.Index;
									var indexC = pointA.Outer.NewIndex;
									var indexD = pointB.Outer.NewIndex;
									var coordA = pointA.Coord;
									var coordB = pointB.Coord;
									var coordC = pointA.Outer.NewCoord;
									var coordD = pointB.Outer.NewCoord;

									if (edge.Flip == true)
									{
										indices.Add(indexA);
										indices.Add(indexC);
										indices.Add(indexB);

										indices.Add(indexD);
										indices.Add(indexB);
										indices.Add(indexC);
									}
									else
									{
										indices.Add(indexA);
										indices.Add(indexB);
										indices.Add(indexC);

										indices.Add(indexD);
										indices.Add(indexC);
										indices.Add(indexB);
									}
#if UNITY_EDITOR
									if (P3dSeamFixer_Editor.DebugScale > 0.0f)
									{
										Debug.DrawLine(coordA * P3dSeamFixer_Editor.DebugScale, coordB * P3dSeamFixer_Editor.DebugScale, new Color(0.0f, 1.0f, 0.0f, 0.5f), 1.0f);
										Debug.DrawLine(coordB * P3dSeamFixer_Editor.DebugScale, coordC * P3dSeamFixer_Editor.DebugScale, new Color(0.0f, 1.0f, 0.0f, 0.5f), 1.0f);
										Debug.DrawLine(coordC * P3dSeamFixer_Editor.DebugScale, coordA * P3dSeamFixer_Editor.DebugScale, new Color(0.0f, 1.0f, 0.0f, 0.5f), 1.0f);

										Debug.DrawLine(coordD * P3dSeamFixer_Editor.DebugScale, coordC * P3dSeamFixer_Editor.DebugScale, new Color(0.0f, 1.0f, 0.0f, 0.5f), 1.0f);
										Debug.DrawLine(coordC * P3dSeamFixer_Editor.DebugScale, coordB * P3dSeamFixer_Editor.DebugScale, new Color(0.0f, 1.0f, 0.0f, 0.5f), 1.0f);
										Debug.DrawLine(coordB * P3dSeamFixer_Editor.DebugScale, coordD * P3dSeamFixer_Editor.DebugScale, new Color(0.0f, 1.0f, 0.0f, 0.5f), 1.0f);
									}
#endif
								}
							}
						}
					}
				}

				FixSeams(source, output, submeshes, rings, insertions, coord);
			}
		}

		private static void AddCoord(List<Vector4> coords, Insertion insertion, bool write)
		{
			var uv = coords[insertion.Index];

			if (write == true)
			{
				uv.x = insertion.NewCoord.x;
				uv.y = insertion.NewCoord.y;
			}

			coords.Add(uv);
		}

		private static void FixSeams(Mesh source, Mesh output, List<List<int>> submeshes, List<Ring> rings, List<Insertion> insertions, P3dCoord coord)
		{
			output.bindposes    = source.bindposes;
			output.bounds       = source.bounds;
			output.subMeshCount = source.subMeshCount;
			output.indexFormat  = source.indexFormat;

			if (source.vertexCount + insertions.Count >= 65535)
			{
				output.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
			}

			var weights   = new List<BoneWeight>(); source.GetBoneWeights(weights);
			var colors    = new List<Color32>();    source.GetColors(colors);
			var normals   = new List<Vector3>();    source.GetNormals(normals);
			var tangents  = new List<Vector4>();    source.GetTangents(tangents);
			var coords0   = new List<Vector4>();    source.GetUVs(0, coords0);
			var coords1   = new List<Vector4>();    source.GetUVs(1, coords1);
			var coords2   = new List<Vector4>();    source.GetUVs(2, coords2);
			var coords3   = new List<Vector4>();    source.GetUVs(3, coords3);
			var positions = new List<Vector3>();    source.GetVertices(positions);
#if UNITY_2019_1_OR_NEWER
			var boneVertices = new List<byte>(source.GetBonesPerVertex());
			var boneWeights  = new List<BoneWeight1>(source.GetAllBoneWeights());
			var boneIndices  = new List<int>();

			if (boneVertices.Count > 0)
			{
				var total = 0;

				foreach (var count in boneVertices)
				{
					boneIndices.Add(total);
				
					total += count;
				}

				weights.Clear();
			}
#endif
			foreach (var insertion in insertions)
			{
#if UNITY_2019_1_OR_NEWER
				if (boneVertices.Count > 0)
				{
					var index = boneIndices[insertion.Index];
					var count = boneVertices[insertion.Index];

					boneVertices.Add(count);

					for (var i = 0; i < count; i++)
					{
						boneWeights.Add(boneWeights[index + i]);
					}
				}
#endif
				if (weights.Count > 0) weights.Add(weights[insertion.Index]);

				if (colors.Count > 0) colors.Add(colors[insertion.Index]);

				if (normals.Count > 0) normals.Add(normals[insertion.Index]);

				if (tangents.Count > 0) tangents.Add(tangents[insertion.Index]);

				if (coords0.Count > 0) AddCoord(coords0, insertion, coord == P3dCoord.First);

				if (coords1.Count > 0) AddCoord(coords1, insertion, coord == P3dCoord.Second);

				if (coords2.Count > 0) AddCoord(coords2, insertion, coord == P3dCoord.Third);

				if (coords3.Count > 0) AddCoord(coords3, insertion, coord == P3dCoord.Fourth);

				positions.Add(positions[insertion.Index]);
			}

			output.SetVertices(positions);

			if (weights.Count > 0)
			{
				output.boneWeights = weights.ToArray();
			}
#if UNITY_2019_1_OR_NEWER
			if (boneVertices.Count > 0)
			{
				var na1 = new Unity.Collections.NativeArray<byte>(boneVertices.ToArray(), Unity.Collections.Allocator.Temp);
				var na2 = new Unity.Collections.NativeArray<BoneWeight1>(boneWeights.ToArray(), Unity.Collections.Allocator.Temp);
				output.SetBoneWeights(na1, na2);
				na2.Dispose();
				na1.Dispose();
			}
#endif
			output.SetColors(colors);
			output.SetNormals(normals);
			output.SetTangents(tangents);
			output.SetUVs(0, coords0);
			output.SetUVs(1, coords1);
			output.SetUVs(2, coords2);
			output.SetUVs(3, coords3);

			var deltaVertices = new List<Vector3>();
			var deltaNormals = new List<Vector3>();
			var deltaTangents = new List<Vector3>();

			if (source.blendShapeCount > 0)
			{
				var tempDeltaVertices = new Vector3[source.vertexCount];
				var tempDeltaNormals  = new Vector3[source.vertexCount];
				var tempDeltaTangents = new Vector3[source.vertexCount];

				for (var i = 0; i < source.blendShapeCount; i++)
				{
					var shapeName  = source.GetBlendShapeName(i);
					var frameCount = source.GetBlendShapeFrameCount(i);

					for (var j = 0; j < frameCount; j++)
					{
						source.GetBlendShapeFrameVertices(i, j, tempDeltaVertices, tempDeltaNormals, tempDeltaTangents);

						deltaVertices.Clear();
						deltaNormals.Clear();
						deltaTangents.Clear();

						deltaVertices.AddRange(tempDeltaVertices);
						deltaNormals.AddRange(tempDeltaNormals);
						deltaTangents.AddRange(tempDeltaTangents);

						foreach (var insertion in insertions)
						{
							deltaVertices.Add(deltaVertices[insertion.Index]);
							deltaNormals.Add(deltaNormals[insertion.Index]);
							deltaTangents.Add(deltaTangents[insertion.Index]);
						}

						output.AddBlendShapeFrame(shapeName, source.GetBlendShapeFrameWeight(i, j), deltaVertices.ToArray(), deltaNormals.ToArray(), deltaTangents.ToArray());
					}
				}
			}

			for (var i = 0; i < submeshes.Count; i++)
			{
				output.SetTriangles(submeshes[i], i);
			}
		}

		private static void TraceEdges(Ring ring, Point point)
		{
			var edges = point.Edges;

			for (var i = 0; i < edges.Count; i++)
			{
				var edge = edges[i];

				if (edge.Used == false && edge.PointA == point)
				{
					edge.Used = true;

					if (edge.PointB == ring.Points[0])
					{
						return;
					}

					ring.Points.Add(edge.PointB);
					
					point = edge.PointB;
					edges = edge.PointB.Edges;
					i     = -1;
				}
			}
		}

		private static void AddPoint(Dictionary<Vector2, Point> points, Vector2 coord, int index)
		{
			var point = default(Point);

			if (points.TryGetValue(coord, out point) == false)
			{
				point = new Point();

				point.Coord = coord;
				point.Index = index;

				points.Add(coord, point);
			}
		}

		private static void AddTriangle(List<Edge> edges, Point pointA, Point pointB, Point pointC)
		{
			var ab = pointB.Coord - pointA.Coord;
			var ac = pointC.Coord - pointA.Coord;

			// Ignore degenerate triangles
			if (Vector3.Cross(ab, ac).sqrMagnitude >= 0.0f)
			{
				// Clockwise?
				if (((pointB.Coord.x - pointA.Coord.x) * (pointC.Coord.y - pointA.Coord.y) - (pointC.Coord.x - pointA.Coord.x) * (pointB.Coord.y - pointA.Coord.y)) >= 0.0f)
				{
					AddTriangle2(edges, pointA, pointB, pointC, true);
				}
				else
				{
					AddTriangle2(edges, pointC, pointB, pointA, false);
				}
			}
		}

		private static void AddTriangle2(List<Edge> edges, Point pointA, Point pointB, Point pointC, bool flip)
		{
			RemoveOrAddEdge(edges, pointA, pointB, flip);
			RemoveOrAddEdge(edges, pointB, pointC, flip);
			RemoveOrAddEdge(edges, pointC, pointA, flip);
		}

		private static void RemoveOrAddEdge(List<Edge> edges, Point pointA, Point pointB, bool flip)
		{
			for (var i = 0; i < pointA.Edges.Count; i++)
			{
				var edge = pointA.Edges[i];

				if (edge.Match(pointA, pointB) == true)
				{
					edge.Used = true; return;
				}
			}

			for (var i = 0; i < pointB.Edges.Count; i++)
			{
				var edge = pointB.Edges[i];

				if (edge.Match(pointA, pointB) == true)
				{
					edge.Used = true; return;
				}
			}

			var newEdge = new Edge();

			newEdge.PointA = pointA;
			newEdge.PointB = pointB;
			newEdge.Flip   = flip;

			pointA.Edges.Add(newEdge);
			pointB.Edges.Add(newEdge);

			edges.Add(newEdge);
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dSeamFixer;

	[CustomEditor(typeof(TARGET))]
	public class P3dSeamFixer_Editor : CwEditor
	{
		/// <summary>If this is above 0 then Debug.Lines will be output during generation.</summary>
		public static float DebugScale;

		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			EditorGUILayout.HelpBox("This tool will convert a normal mesh into a mesh with UV seams suitable for painting. The fixed mesh will be placed as a child of this tool in your Project window. To use the fixed mesh, drag and drop it into your MeshFilter or SkinnedMeshRenderer.", MessageType.Info);

			Separator();

			Each(tgts, t => t.ConvertLegacy()); serializedObject.Update();

			var sMeshes = serializedObject.FindProperty("meshes");
			var sDel    = -1;

			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Meshes");
				if (GUILayout.Button("Add", EditorStyles.miniButton, GUILayout.ExpandWidth(false)) == true)
				{
					sMeshes.InsertArrayElementAtIndex(sMeshes.arraySize);
				}
			EditorGUILayout.EndHorizontal();

			EditorGUI.indentLevel++;
				for (var i = 0; i < tgt.Meshes.Count; i++)
				{
					var sSource = sMeshes.GetArrayElementAtIndex(i).FindPropertyRelative("Source");

					EditorGUILayout.BeginHorizontal();
						BeginError(sSource.objectReferenceValue == null);
							EditorGUILayout.PropertyField(sSource);
						EndError();
						if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.ExpandWidth(false)) == true)
						{
							sDel = i;
						}
					EditorGUILayout.EndHorizontal();
				}
			EditorGUI.indentLevel--;

			if (sDel >= 0)
			{
				sMeshes.DeleteArrayElementAtIndex(sDel);
			}

			Separator();

			Draw("coord", "The UV channel whose seams will be fixed.");
			BeginError(Any(tgts, t => t.Threshold <= 0.0f));
				Draw("threshold", "The threshold below which vertex UV coordinates will be snapped.");
			EndError();
			BeginError(Any(tgts, t => t.Border <= 0.0f));
				Draw("border", "The thickness of the UV borders in the fixed mesh.");
			EndError();
			DebugScale = EditorGUILayout.FloatField("Debug Scale", DebugScale);

			Separator();

			if (Button("Generate") == true)
			{
				Each(tgts, t => t.Generate());
			}
		}

		[MenuItem("CONTEXT/Mesh/Fix Seams (Paint in 3D)")]
		[MenuItem("CONTEXT/ModelImporter/Fix Seams (Paint in 3D)")]
		public static void Create(MenuCommand menuCommand)
		{
			var sources = new List<Mesh>();
			var mesh    = menuCommand.context as Mesh;
			var name    = "";

			if (mesh != null)
			{
				sources.Add(mesh);

				name = mesh.name;
			}
			else
			{
				var modelImporter = menuCommand.context as ModelImporter;

				if (modelImporter != null)
				{
					var assets = AssetDatabase.LoadAllAssetsAtPath(modelImporter.assetPath);

					for (var i = 0; i < assets.Length; i++)
					{
						var assetMesh = assets[i] as Mesh;

						if (assetMesh != null)
						{
							sources.Add(assetMesh);
						}
					}

					name = System.IO.Path.GetFileNameWithoutExtension(modelImporter.assetPath);
				}
			}
			
			if (sources.Count > 0)
			{
				var path = AssetDatabase.GetAssetPath(menuCommand.context);

				if (string.IsNullOrEmpty(path) == false)
				{
					path = System.IO.Path.GetDirectoryName(path);
				}
				else
				{
					path = "Assets";
				}

				path += "/Seam Fixer (" + name + ").asset";

				var instance = CreateInstance<P3dSeamFixer>();

				foreach (var source in sources)
				{
					instance.AddMesh(source);
				}

				ProjectWindowUtil.CreateAsset(instance, path);
			}
		}
	}
}
#endif