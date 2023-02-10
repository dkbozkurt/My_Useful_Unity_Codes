using System.Collections.Generic;
using UnityEngine;

namespace PaintIn3D
{
	public static class P3dMeshCache
	{
		class MeshData
		{
			Vector3[] positions;

			int[] indices;

			int total;

			public void Update(Mesh mesh)
			{
				positions = mesh.vertices;
				indices   = mesh.triangles;
				total     = indices.Length / 3;
			}

			public bool GetTrianglePositions(RaycastHit hit, ref Vector3 positionA, ref Vector3 positionB, ref Vector3 positionC)
			{
				var triangleIndex = hit.triangleIndex;

				if (triangleIndex >= 0 && triangleIndex < total)
				{
					var index     = triangleIndex * 3;
					var transform = hit.transform;

					positionA = transform.TransformPoint(positions[indices[index + 0]]);
					positionB = transform.TransformPoint(positions[indices[index + 1]]);
					positionC = transform.TransformPoint(positions[indices[index + 2]]);

					return true;
				}

				return false;
			}
		}

		private static Dictionary<Mesh, MeshData> cachedData = new Dictionary<Mesh, MeshData>();

		public static bool GetTrianglePositions(RaycastHit hit, ref Vector3 positionA, ref Vector3 positionB, ref Vector3 positionC)
		{
			var meshCollider = hit.collider as MeshCollider;

			if (meshCollider != null && meshCollider.convex == false)
			{
				var mesh = meshCollider.sharedMesh;

				if (mesh != null)
				{
					var meshData = default(MeshData);

					if (cachedData.TryGetValue(mesh, out meshData) == false)
					{
						meshData = new MeshData();

						meshData.Update(mesh);

						cachedData.Add(mesh, meshData);
					}

					return meshData.GetTrianglePositions(hit, ref positionA, ref positionB, ref positionC);
				}
			}

			return false;
		}
	}
}