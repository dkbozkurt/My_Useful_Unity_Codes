using UnityEngine;
using System.Collections.Generic;

namespace PaintIn3D
{
	/// <summary>This class stores lists of IHit__ instances, allowing components like P3dHit__ to easily invoke hit events.</summary>
	public class P3dHitCache
	{
		[System.NonSerialized]
		private bool cached;

		[System.NonSerialized]
		private List<IHitPoint> hitPoints = new List<IHitPoint>();

		[System.NonSerialized]
		private List<IHitLine> hitLines = new List<IHitLine>();

		[System.NonSerialized]
		private List<IHitTriangle> hitTriangles = new List<IHitTriangle>();

		[System.NonSerialized]
		private List<IHitQuad> hitQuads = new List<IHitQuad>();

		[System.NonSerialized]
		private List<IHitCoord> hitCoords = new List<IHitCoord>();

		[System.NonSerialized]
		private static List<IHit> hits = new List<IHit>();

		public bool Cached
		{
			get
			{
				return cached;
			}
		}

#if UNITY_EDITOR
		private static HashSet<object> tempHits = new HashSet<object>();

		public void Inspector(GameObject gameObject, bool point = false, bool line = false, bool triangle = false, bool quad = false, bool coord = false)
		{
			Cache(gameObject);

			tempHits.Clear();

			if (point == true)
			{
				for (var i = 0; i < hitPoints.Count; i++)
				{
					tempHits.Add(hitPoints[i]);
				}
			}

			if (line == true)
			{
				for (var i = 0; i < hitLines.Count; i++)
				{
					tempHits.Add(hitLines[i]);
				}
			}

			if (triangle == true)
			{
				for (var i = 0; i < hitTriangles.Count; i++)
				{
					tempHits.Add(hitTriangles[i]);
				}
			}

			if (quad == true)
			{
				for (var i = 0; i < hitQuads.Count; i++)
				{
					tempHits.Add(hitQuads[i]);
				}
			}

			if (coord == true)
			{
				for (var i = 0; i < hitCoords.Count; i++)
				{
					tempHits.Add(hitCoords[i]);
				}
			}

			if (tempHits.Count == 0)
			{
				UnityEditor.EditorGUILayout.HelpBox("This component isn't sending hit events to anything.", UnityEditor.MessageType.Warning);
			}
			else
			{
				var output = "This component is sending hit events to:";

				foreach (var hit in tempHits)
				{
					output += "\n" + hit;
				}

				UnityEditor.EditorGUILayout.HelpBox(output, UnityEditor.MessageType.Info);
			}
		}
#endif

		public void InvokePoint(GameObject gameObject, bool preview, int priority, float pressure, Vector3 position, Quaternion rotation)
		{
			if (cached == false)
			{
				Cache(gameObject);
			}

			var seed = Random.Range(int.MinValue, int.MaxValue);

			for (var i = 0; i < hitPoints.Count; i++)
			{
				hitPoints[i].HandleHitPoint(preview, priority, pressure, seed, position, rotation);
			}
		}

		public void InvokeLine(GameObject gameObject, bool preview, int priority, float pressure, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip)
		{
			if (cached == false)
			{
				Cache(gameObject);
			}

			var seed = Random.Range(int.MinValue, int.MaxValue);

			for (var i = 0; i < hitLines.Count; i++)
			{
				hitLines[i].HandleHitLine(preview, priority, pressure, seed, position, endPosition, rotation, clip);
			}
		}

		public void InvokeTriangle(GameObject gameObject, bool preview, int priority, float pressure, RaycastHit hit, Quaternion rotation)
		{
			var positionA = default(Vector3);
			var positionB = default(Vector3);
			var positionC = default(Vector3);

			if (P3dMeshCache.GetTrianglePositions(hit, ref positionA, ref positionB, ref positionC) == true)
			{
				InvokeTriangle(gameObject, preview, priority, pressure, positionA, positionB, positionC, rotation);
			}
		}

		public void InvokeTriangle(GameObject gameObject, bool preview, int priority, float pressure, Vector3 positionA, Vector3 positionB, Vector3 positionC, Quaternion rotation)
		{
			if (cached == false)
			{
				Cache(gameObject);
			}

			var seed = Random.Range(int.MinValue, int.MaxValue);

			for (var i = 0; i < hitTriangles.Count; i++)
			{
				hitTriangles[i].HandleHitTriangle(preview, priority, pressure, seed, positionA, positionB, positionC, rotation);
			}
		}

		public void InvokeQuad(GameObject gameObject, bool preview, int priority, float pressure, Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2, Quaternion rotation, bool clip)
		{
			if (cached == false)
			{
				Cache(gameObject);
			}

			var seed = Random.Range(int.MinValue, int.MaxValue);

			for (var i = 0; i < hitQuads.Count; i++)
			{
				hitQuads[i].HandleHitQuad(preview, priority, pressure, seed, position, endPosition, position2, endPosition2, rotation, clip);
			}
		}

		public void InvokeCoord(GameObject gameObject, bool preview, int priority, float pressure, P3dHit hit, Quaternion rotation)
		{
			if (cached == false)
			{
				Cache(gameObject);
			}

			var seed = Random.Range(int.MinValue, int.MaxValue);

			for (var i = 0; i < hitCoords.Count; i++)
			{
				hitCoords[i].HandleHitCoord(preview, priority, pressure, seed, hit, rotation);
			}
		}

		public void Clear()
		{
			cached = false;

			hitPoints.Clear();
			hitLines.Clear();
			hitTriangles.Clear();
			hitQuads.Clear();
			hitCoords.Clear();
		}

		private void Cache(GameObject gameObject)
		{
			cached = true;

			gameObject.GetComponentsInChildren(hits);

			hitPoints.Clear();
			hitLines.Clear();
			hitTriangles.Clear();
			hitQuads.Clear();
			hitCoords.Clear();

			for (var i = 0; i < hits.Count; i++)
			{
				var hit = hits[i];

				var hitPoint = hit as IHitPoint; if (hitPoint != null) { hitPoints.Add(hitPoint); }

				var hitLine = hit as IHitLine; if (hitLine != null) { hitLines.Add(hitLine); }

				var hitTriangle = hit as IHitTriangle; if (hitTriangle != null) { hitTriangles.Add(hitTriangle); }

				var hitQuad = hit as IHitQuad; if (hitQuad != null) { hitQuads.Add(hitQuad); }

				var hitCoord = hit as IHitCoord; if (hitCoord != null) { hitCoords.Add(hitCoord); }
			}
		}
	}
}