// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using Sirenix.OdinInspector;
using UnityEditor.UI;
using UnityEngine;


namespace Game.Creative.Scripts
{
	/// <summary>
	/// Mesh Generation with "Bezier Path Creator" Asset
	///
	/// Attach this script onto GameObject that includes "PathCreator.cs" script from Bezier Path Creator on it.
	///
	/// After adjusting the path by using tool's points, use "GenerateMesh" method to generate mesh.
	///
	/// You can access the Bezier Path's point through in game by code, through -> PathCreator.path.localPoints[i], i is index.
	///   
	/// </summary>
	
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	
	public class MeshGenerator : MonoBehaviour
	{
		public PathCreator PathCreator;
		public MeshFilter MeshFilter;

		[Button]
		private void GenerateMesh()
		{
			var generateMeshData = GenerateMeshData(0.1f,0.1f);
			MeshFilter.mesh = generateMeshData;
		}
		
		private Mesh GenerateMeshData (float roadWidth, float thickness)
		{
			var vertexPath = PathCreator.path;
			
			// Access the created paths point by index and change their locations through script in Update.
			// PathCreator.path.localPoints ...
			
			Vector3[] verts = new Vector3[vertexPath.NumPoints * 8];
			Vector2[] uvs = new Vector2[verts.Length];
			Vector3[] normals = new Vector3[verts.Length];
 
			int numTris = 2 * (vertexPath.NumPoints - 1);
			int[] roadTriangles = new int[numTris * 3];
			int[] underRoadTriangles = new int[numTris * 3];
			int[] sideOfRoadTriangles = new int[numTris * 2 * 3];
 
			int vertIndex = 0;
			int triIndex = 0;
 
			// Vertices for the top of the road are layed out:
			// 0  1
			// 8  9
			// and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
			int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
			int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };
 
			for (int i = 0; i < vertexPath.NumPoints; i++) {
				Vector3 localUp = Vector3.Cross (vertexPath.GetTangent (i), vertexPath.GetNormal (i));
				Vector3 localRight = vertexPath.GetNormal (i);
 
				// Find position to left and right of current path vertex
				Vector3 vertSideA = vertexPath.GetPoint (i) - localRight * Mathf.Abs (roadWidth);
				Vector3 vertSideB = vertexPath.GetPoint (i) + localRight * Mathf.Abs (roadWidth);
 
				// Add top of road vertices
				verts[vertIndex + 0] = vertSideA;
				verts[vertIndex + 1] = vertSideB;
				// Add bottom of road vertices
				verts[vertIndex + 2] = vertSideA - localUp * thickness;
				verts[vertIndex + 3] = vertSideB - localUp * thickness;
 
				// Duplicate vertices to get flat shading for sides of road
				verts[vertIndex + 4] = verts[vertIndex + 0];
				verts[vertIndex + 5] = verts[vertIndex + 1];
				verts[vertIndex + 6] = verts[vertIndex + 2];
				verts[vertIndex + 7] = verts[vertIndex + 3];
 
				// Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
				uvs[vertIndex + 0] = new Vector2 (0, vertexPath.cumulativeLengthAtEachVertex[i]);
				uvs[vertIndex + 1] = new Vector2 (1, vertexPath.cumulativeLengthAtEachVertex[i]);
 
				// Top of road normals
				normals[vertIndex + 0] = localUp;
				normals[vertIndex + 1] = localUp;
				// Bottom of road normals
				normals[vertIndex + 2] = -localUp;
				normals[vertIndex + 3] = -localUp;
				// Sides of road normals
				normals[vertIndex + 4] = -localRight;
				normals[vertIndex + 5] = localRight;
				normals[vertIndex + 6] = -localRight;
				normals[vertIndex + 7] = localRight;
 
				// Set triangle indices
				if (i < vertexPath.NumPoints - 1 || vertexPath.isClosedLoop) {
					for (int j = 0; j < triangleMap.Length; j++) {
						roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
						// reverse triangle map for under road so that triangles wind the other way and are visible from underneath
						underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
					}
					for (int j = 0; j < sidesTriangleMap.Length; j++) {
						sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
					}
 
				}
 
				vertIndex += 8;
				triIndex += 6;
			}

			var mesh = new Mesh();
			mesh.Clear ();
			mesh.vertices = verts;
			mesh.uv = uvs;
			mesh.normals = normals;
			mesh.subMeshCount = 3;
			mesh.SetTriangles (roadTriangles, 0);
			mesh.SetTriangles (underRoadTriangles, 1);
			mesh.SetTriangles (sideOfRoadTriangles, 2);
			mesh.RecalculateBounds ();
			return mesh;
		}
 
	 }
}