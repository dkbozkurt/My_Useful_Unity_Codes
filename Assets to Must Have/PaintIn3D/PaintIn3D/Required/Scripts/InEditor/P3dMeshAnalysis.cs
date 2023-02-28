#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This window allows you to examine the UV data of a mesh. This can be accessed from the context menu (⋮ button at top right) of any mesh in the inspector.</summary>
	public class P3dMeshAnalysis : CwEditorWindow
	{
		private Mesh mesh;

		private int coord;

		private int submesh;

		private int mode;

		private float pitch;

		private float yaw;

		private bool ready;

		private int triangleCount;

		private int invalidCount;

		private int partiallyCount;

		private float utilizationPercent;

		private float overlapPercent;

		private Texture2D overlapTex;

		private int outsideCount;

		private int overlapCount;

		private Material overlapMaterial;

		private string[] submeshNames = new string[0];

		private List<int> indices = new List<int>();

		private List<Vector3> positions = new List<Vector3>();

		private List<Vector2> coords = new List<Vector2>();

		private Vector3[] arrayA;

		private List<Vector3> listA = new List<Vector3>();

		private Vector3[] arrayB;

		private List<Vector3> listB = new List<Vector3>();

		private List<float> ratioList = new List<float>();

		private static int _Coord = Shader.PropertyToID("_Coord");

#if UNITY_EDITOR
		[MenuItem("CONTEXT/Mesh/Analyze Mesh (Paint in 3D)")]
		public static void Create(MenuCommand menuCommand)
		{
			var mesh = menuCommand.context as Mesh;

			if (mesh != null)
			{
				OpenWith(mesh);
			}
		}
#endif

		public static void OpenWith(GameObject gameObject, Mesh mesh = null)
		{
			OpenWith(P3dCommon.GetMesh(gameObject, mesh));
		}

		public static void OpenWith(Mesh mesh)
		{
			var window = GetWindow<P3dMeshAnalysis>("Mesh Analysis", true);

			window.mesh  = mesh;
			window.ready = false;
		}

		private static bool IsOutside(Vector2 coord)
		{
			return coord.x < 0.0f || coord.x > 1.0f || coord.y < 0.0f || coord.y > 1.0f;
		}

		protected virtual void OnDestroy()
		{
			CwHelper.Destroy(overlapTex);
		}

		private void BakeOverlap()
		{
			var desc          = new RenderTextureDescriptor(1024, 1024, RenderTextureFormat.ARGB32, 0);
			var renderTexture = P3dCommon.GetRenderTexture(desc);

			if (overlapMaterial == null)
			{
				overlapMaterial = P3dCommon.BuildMaterial("Hidden/Paint in 3D/Overlap");
			}

			overlapMaterial.SetVector(_Coord, P3dCommon.IndexToVector(coord));

			var oldActive = RenderTexture.active;

			RenderTexture.active = renderTexture;

			GL.Clear(true, true, Color.black);

			overlapMaterial.SetPass(0);

			Graphics.DrawMeshNow(mesh, Matrix4x4.identity, submesh);

			foreach (var obj in Selection.objects)
			{
				var otherMesh = obj as Mesh;

				if (otherMesh != null && otherMesh != mesh)
				{
					Graphics.DrawMeshNow(otherMesh, Matrix4x4.identity, submesh);
				}
			}

			RenderTexture.active = oldActive;

			overlapTex = P3dCommon.GetReadableCopy(renderTexture);

			P3dCommon.ReleaseRenderTexture(renderTexture);

			var utilizationCount = 0;
			var overlapCount     = 0;

			for (var y = 0; y < overlapTex.height; y++)
			{
				for (var x = 0; x < overlapTex.width; x++)
				{
					var pixel = CwHelper.ToLinear(overlapTex.GetPixel(x, y));

					if (pixel.r > 0.0f)
					{
						if (pixel.r > 1.5 / 255.0f)
						{
							pixel = Color.red;

							overlapCount += 1;
						}
						else
						{
							pixel = Color.gray;
						}

						utilizationCount += 1;

						overlapTex.SetPixel(x, y, pixel);
					}
				}
			}

			var total = overlapTex.width * overlapTex.height * 0.01f;

			utilizationPercent = utilizationCount / total;
			overlapPercent     = overlapCount / total;

			overlapTex.Apply();
		}

		protected override void OnInspector()
		{
			EditorGUILayout.BeginHorizontal();
			var newMesh = (Mesh)EditorGUILayout.ObjectField("Mesh", mesh, typeof(Mesh), false);
			if (GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(60)) == true)
			{
				ready = false;
			}
			EditorGUILayout.EndHorizontal();

			if (newMesh != mesh)
			{
				ready = false;
				mesh  = newMesh;
			}

			if (mesh != null)
			{
				if (mesh.subMeshCount != submeshNames.Length)
				{
					var submeshNamesList = new List<string>();

					for (var i = 0; i < mesh.subMeshCount; i++)
					{
						submeshNamesList.Add(i.ToString());
					}

					submeshNames = submeshNamesList.ToArray();
				}

				EditorGUILayout.Separator();

				var newSubmesh  = EditorGUILayout.Popup("Submesh", submesh, submeshNames);
				var newCoord    = EditorGUILayout.Popup("Coord", coord, new string[] { "UV0", "UV1", "UV2", "UV3" });
				var newMode     = EditorGUILayout.Popup("Mode", mode, new string[] { "Texcoord", "Triangles" });

				if (mode == 1) // Triangles
				{
					EditorGUILayout.BeginHorizontal();
						var newPitch = EditorGUILayout.FloatField("Pitch", pitch);
						var newYaw   = EditorGUILayout.FloatField("Yaw", yaw);
					EditorGUILayout.EndHorizontal();

					if (newPitch != pitch || newYaw != yaw)
					{
						ready = false;
						pitch = newPitch;
						yaw   = newYaw;
					}
				}

				EditorGUILayout.Separator();

				EditorGUILayout.LabelField("Triangles", EditorStyles.boldLabel);
				EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.IntField("Total", triangleCount);
						CwEditor.BeginError(invalidCount > 0);
							EditorGUILayout.IntField("With No UV", invalidCount);
						CwEditor.EndError();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
						CwEditor.BeginError(outsideCount > 0);
							EditorGUILayout.IntField("Out Of Bounds", outsideCount);
						CwEditor.EndError();
						CwEditor.BeginError(partiallyCount > 0);
							EditorGUILayout.IntField("Partially Out Of Bounds", partiallyCount);
						CwEditor.EndError();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
						CwEditor.BeginError(utilizationPercent < 40.0f);
							EditorGUILayout.FloatField("Utilization %", utilizationPercent);
						CwEditor.EndError();
						CwEditor.BeginError(overlapPercent > 0);
							EditorGUILayout.FloatField("Overlap %", overlapPercent);
						CwEditor.EndError();
					EditorGUILayout.EndHorizontal();
				EditorGUI.EndDisabledGroup();

				if (coord != newCoord || newSubmesh != submesh || newMode != mode || ready == false)
				{
					ready   = true;
					coord   = newCoord;
					submesh = newSubmesh;
					mode    = newMode;
					
					listA.Clear();
					listB.Clear();
					ratioList.Clear();
					mesh.GetTriangles(indices, submesh);
					mesh.GetVertices(positions);
					mesh.GetUVs(coord, coords);

					triangleCount      = indices.Count / 3;
					invalidCount       = 0;
					outsideCount       = 0;
					partiallyCount     = 0;
					overlapTex         = CwHelper.Destroy(overlapTex);
					utilizationPercent = 0.0f;
					overlapPercent     = 0.0f;

					if (coords.Count > 0)
					{
						if (mode == 0) // Texcoord
						{
							BakeOverlap();
						}

						var rot  = Quaternion.Euler(pitch, yaw, 0.0f);
						var off  = -mesh.bounds.center;
						var mul  = CwHelper.Reciprocal(mesh.bounds.size.magnitude);
						var half = Vector3.one * 0.5f;

						for (var i = 0; i < indices.Count; i += 3)
						{
							var positionA = positions[indices[i + 0]];
							var positionB = positions[indices[i + 1]];
							var positionC = positions[indices[i + 2]];
							var coordA    = coords[indices[i + 0]];
							var coordB    = coords[indices[i + 1]];
							var coordC    = coords[indices[i + 2]];
							var outside   = 0; outside += IsOutside(coordA) ? 1 : 0; outside += IsOutside(coordB) ? 1 : 0; outside += IsOutside(coordC) ? 1 : 0;
							var area      = Vector3.Cross(coordA - coordB, coordA - coordC).sqrMagnitude;
							var invalid   = area <= float.Epsilon;

							if (invalid == true)
							{
								invalidCount++;
							}

							if (outside == 3)
							{
								outsideCount++;
							}

							if (outside == 1 || outside == 2)
							{
								partiallyCount++;
							}

							if (mode == 0) // Texcoord
							{
								listA.Add(coordA); listA.Add(coordB);
								listA.Add(coordB); listA.Add(coordC);
								listA.Add(coordC); listA.Add(coordA);
							}

							if (mode == 1) // Triangles
							{
								positionA = half + rot * (off + positionA) * mul;
								positionB = half + rot * (off + positionB) * mul;
								positionC = half + rot * (off + positionC) * mul;

								positionA.z = positionB.z = positionC.z = 0.0f;

								listA.Add(positionA); listA.Add(positionB);
								listA.Add(positionB); listA.Add(positionC);
								listA.Add(positionC); listA.Add(positionA);

								if (invalid == true)
								{
									listB.Add(positionA); listB.Add(positionB);
									listB.Add(positionB); listB.Add(positionC);
									listB.Add(positionC); listB.Add(positionA);
								}
							}
						}
					}
					else
					{
						invalidCount = triangleCount;
					}

					arrayA = listA.ToArray();
					arrayB = listB.ToArray();
				}

				var rect = EditorGUILayout.BeginVertical(); GUILayout.FlexibleSpace(); EditorGUILayout.EndVertical();
				var pos  = rect.min;
				var siz  = rect.size;

				GUI.Box(rect, "");

				if (mode == 0 && overlapTex != null) // Texcoord
				{
					GUI.DrawTexture(rect, overlapTex);
				}

				Handles.BeginGUI();
					if (listA.Count > 0)
					{
						for (var i = listA.Count - 1; i >= 0; i--)
						{
							var coord = listA[i];

							coord.x = pos.x + siz.x * coord.x;
							coord.y = pos.y + siz.y * (1.0f - coord.y);

							arrayA[i] = coord;
						}

						Handles.DrawLines(arrayA);

						for (var i = listB.Count - 1; i >= 0; i--)
						{
							var coord = listB[i];

							coord.x = pos.x + siz.x * coord.x;
							coord.y = pos.y + siz.y * (1.0f - coord.y);

							arrayB[i] = coord;
						}

						Handles.color = Color.red;
						Handles.DrawLines(arrayB);
					}
				Handles.EndGUI();
			}
			else
			{
				EditorGUILayout.HelpBox("No Mesh Selected.\nTo select a mesh, click Analyze Mesh from the P3dPaintable component, or from the Mesh inspector context menu (gear icon at top right).", MessageType.Info);
			}
		}
	}
}
#endif