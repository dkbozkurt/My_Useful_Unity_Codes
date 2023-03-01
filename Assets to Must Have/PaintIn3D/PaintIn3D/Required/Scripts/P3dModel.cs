using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component marks the current GameObject as being paintable, as long as this GameObject has a MeshFilter + MeshRenderer, or a SkinnedMeshRenderer.
	/// NOTE: To actually paint, the <b>P3dPaintableTexture</b> component must be on a different object.</summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Renderer))]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dModel")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Model")]
	public class P3dModel : MonoBehaviour
	{
		public enum UseMeshType
		{
			AsIs,
			AutoSeamFix
		}

		/// <summary>The paintable this separate paintable is associated with.</summary>
		public virtual P3dPaintable Paintable { set { paintable = value; } get { return paintable; } } [SerializeField] protected P3dPaintable paintable;

		/// <summary>Transform the mesh with its position, rotation, and scale? Some skinned mesh setups require this to be disabled.</summary>
		public virtual bool IncludeScale { set { includeScale = value; } get { return includeScale; } } [SerializeField] protected bool includeScale = true;

		/// <summary>This allows you to choose how the <b>Mesh</b> attached to the current <b>Renderer</b> is used when painting.
		/// AsIs = Use what is currently set in the renderer.
		/// AutoSeamFix = Use (or automatically generate) a seam-fixed version of the mesh currently set in the renderer.</summary>
		public UseMeshType UseMesh { set { useMesh = value; } get { return useMesh; } } [SerializeField] protected UseMeshType useMesh;

		/// <summary>The hash code for this model used for de/serialization of this instance.</summary>
		public P3dHash Hash { set { hash = value; P3dSerialization.TryRegister(this, hash); } get { return hash; } } [SerializeField] protected P3dHash hash;

		[System.NonSerialized]
		private Renderer cachedRenderer;

		[System.NonSerialized]
		private bool cachedRendererSet;

		[System.NonSerialized]
		private SkinnedMeshRenderer cachedSkinned;

		[System.NonSerialized]
		private MeshFilter cachedFilter;

		[System.NonSerialized]
		private bool cachedSkinnedSet;

		[System.NonSerialized]
		private Transform cachedTransform;

		[System.NonSerialized]
		private GameObject cachedGameObject;

		[System.NonSerialized]
		private Material[] materials;

		[System.NonSerialized]
		private bool materialsSet;

		[System.NonSerialized]
		private Mesh bakedMesh;

		[System.NonSerialized]
		private bool bakedMeshSet;

		[System.NonSerialized]
		protected bool prepared;

		[System.NonSerialized]
		private Mesh preparedMesh;

		[System.NonSerialized]
		private Matrix4x4 preparedMatrix;

		[System.NonSerialized]
		private int[] preparedTriangles;

		[System.NonSerialized]
		private Vector3[] preparedPositions;

		[System.NonSerialized]
		private Vector2[] preparedCoord0;

		[System.NonSerialized]
		private Vector2[] preparedCoord1;

		[System.NonSerialized]
		private static List<P3dModel> tempModels = new List<P3dModel>();

		/// <summary>This stores all active and enabled instances in the open scenes.</summary>
		public static LinkedList<P3dModel> Instances { get { return instances; } } private static LinkedList<P3dModel> instances = new LinkedList<P3dModel>(); private LinkedListNode<P3dModel> instancesNode;

		public bool Prepared
		{
			set
			{
				prepared = value;
			}

			get
			{
				return prepared;
			}
		}

		public Mesh PreparedMesh
		{
			get
			{
				return preparedMesh;
			}
		}

		public GameObject CachedGameObject
		{
			get
			{
				return cachedGameObject;
			}
		}

		public Renderer CachedRenderer
		{
			get
			{
				if (cachedRendererSet == false)
				{
					CacheRenderer();
				}

				return cachedRenderer;
			}
		}

		public Material[] Materials
		{
			get
			{
				if (materialsSet == false)
				{
					materials    = CachedRenderer.sharedMaterials;
					materialsSet = true;
				}

				return materials;
			}
		}

		public int GetMaterialIndex(Material material)
		{
			if (material != null)
			{
				var materials = Materials;

				for (var i = materials.Length - 1; i >= 0; i--)
				{
					if (materials[i] == material)
					{
						if (CachedRenderer.isPartOfStaticBatch == true)
						{
							var meshRenderer = CachedRenderer as MeshRenderer;
							
							if (meshRenderer != null)
							{
								return meshRenderer.subMeshStartIndex + i;
							}
						}

						return i;
					}
				}
			}

			return -1;
		}

		private void CacheRenderer()
		{
			cachedRenderer    = GetComponent<Renderer>();
			cachedRendererSet = true;

			if (cachedRenderer is SkinnedMeshRenderer)
			{
				cachedSkinned    = (SkinnedMeshRenderer)cachedRenderer;
				cachedSkinnedSet = true;
			}
			else
			{
				cachedFilter = GetComponent<MeshFilter>();
			}
		}

		/// <summary>Materials will give you a cached CachedRenderer.sharedMaterials array. If you have updated this array externally then call this to force the cache to update next them it's accessed.</summary>
		[ContextMenu("Dirty Materials")]
		public void DirtyMaterials()
		{
			materialsSet = false;
		}

		/// <summary>This will return a list of all paintables that overlap the specified bounds</summary>
		public static List<P3dModel> FindOverlap(Vector3 position, float radius, int layerMask)
		{
			tempModels.Clear();

			foreach (var instance in instances)
			{
				if (CwHelper.IndexInMask(instance.CachedGameObject.layer, layerMask) == true && instance.Paintable != null)
				{
					var bounds    = instance.CachedRenderer.bounds;
					var sqrRadius = radius + bounds.extents.magnitude; sqrRadius *= sqrRadius;

					if (Vector3.SqrMagnitude(position - bounds.center) < sqrRadius)
					{
						tempModels.Add(instance);

						if (instance.paintable.Activated == false)
						{
							instance.paintable.Activate();
						}
					}
				}
			}

			return tempModels;
		}

		public void GetPreparedPoints(int triangleIndex, ref Vector3 pointA, ref Vector3 pointB, ref Vector3 pointC)
		{
			if (prepared == true && preparedMesh != null)
			{
				if (preparedPositions == null) preparedPositions = preparedMesh.vertices;
				if (preparedTriangles == null) preparedTriangles = preparedMesh.triangles;

				pointA = preparedPositions[preparedTriangles[triangleIndex * 3 + 0]];
				pointB = preparedPositions[preparedTriangles[triangleIndex * 3 + 1]];
				pointC = preparedPositions[preparedTriangles[triangleIndex * 3 + 2]];
			}
		}

		public void GetPreparedCoords0(int triangleIndex, ref Vector2 coordA, ref Vector2 coordB, ref Vector2 coordC)
		{
			if (prepared == true && preparedMesh != null)
			{
				if (preparedTriangles == null) preparedTriangles = preparedMesh.triangles;
				if (preparedCoord0    == null) preparedCoord0    = preparedMesh.uv;

				coordA = preparedCoord0[preparedTriangles[triangleIndex * 3 + 0]];
				coordB = preparedCoord0[preparedTriangles[triangleIndex * 3 + 1]];
				coordC = preparedCoord0[preparedTriangles[triangleIndex * 3 + 2]];
			}
		}

		public void GetPreparedCoords1(int triangleIndex, ref Vector2 coordA, ref Vector2 coordB, ref Vector2 coordC)
		{
			if (prepared == true && preparedMesh != null)
			{
				if (preparedTriangles == null) preparedTriangles = preparedMesh.triangles;
				if (preparedCoord1    == null) preparedCoord1    = preparedMesh.uv;

				coordA = preparedCoord1[preparedTriangles[triangleIndex * 3 + 0]];
				coordB = preparedCoord1[preparedTriangles[triangleIndex * 3 + 1]];
				coordC = preparedCoord1[preparedTriangles[triangleIndex * 3 + 2]];
			}
		}

		public int GetSubmesh(P3dPaintableTexture paintableTexture)
		{
			var material = paintableTexture.Material;

			if (material == null)
			{
				paintableTexture.UpdateMaterial();

				material = paintableTexture.Material;
			}

			return Paintable.GetMaterialIndex(material);
		}

		public void GetPrepared(ref Mesh mesh, ref Matrix4x4 matrix, P3dCoord coord)
		{
			if (prepared == false)
			{
				prepared = true;

				if (cachedRendererSet == false)
				{
					CacheRenderer();
				}

				if (cachedSkinnedSet == true)
				{
					if (bakedMeshSet == false)
					{
						bakedMesh    = new Mesh();
						bakedMeshSet = true;
					}

					var lossyScale    = cachedTransform.lossyScale;
					var scaling       = new Vector3(CwHelper.Reciprocal(lossyScale.x), CwHelper.Reciprocal(lossyScale.y), CwHelper.Reciprocal(lossyScale.z));
					var oldLocalScale = cachedTransform.localScale;

					cachedTransform.localScale = Vector3.one;

					cachedSkinned.BakeMesh(bakedMesh);

					cachedTransform.localScale = oldLocalScale;

					preparedMesh   = bakedMesh;
					preparedMatrix = cachedRenderer.localToWorldMatrix;

					if (includeScale == true)
					{
						preparedMatrix *= Matrix4x4.Scale(scaling);
					}
				}
				else
				{
					preparedMesh   = cachedFilter.sharedMesh;
					preparedMatrix = cachedRenderer.localToWorldMatrix;

					if (useMesh == UseMeshType.AutoSeamFix)
					{
						preparedMesh = P3dSeamFixer.GetCachedMesh(preparedMesh, coord);
					}
				}
			}

			mesh   = preparedMesh;
			matrix = preparedMatrix;
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);

			cachedGameObject = gameObject;
			cachedTransform  = transform;

			P3dSerialization.TryRegister(this, hash);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode); instancesNode = null;
		}

		protected virtual void OnDestroy()
		{
			CwHelper.Destroy(bakedMesh);

			P3dSerialization.TryRegister(this, default(P3dHash));
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dModel;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dModel_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			BeginError(Any(tgts, t => t.Paintable == null));
				Draw("paintable", "The paintable this separate paintable is associated with.");
			EndError();
			Draw("includeScale", "Transform the mesh with its position, rotation, and scale? Some skinned mesh setups require this to be disabled.");
			Draw("useMesh", "This allows you to choose how the Mesh attached to the current Renderer is used when painting.\n\nAsIs = Use what is currently set in the renderer.\n\nAutoSeamFix = Use (or automatically generate) a seam-fixed version of the mesh currently set in the renderer.");
			Draw("hash", "The hash code for this model used for de/serialization of this instance.");

			Separator();

			if (Button("Analyze Mesh") == true)
			{
				P3dMeshAnalysis.OpenWith(tgt.gameObject, tgt.PreparedMesh);
			}

			var mesh = P3dCommon.GetMesh(tgt.gameObject, tgt.PreparedMesh);

			if (mesh != null && mesh.isReadable == false)
			{
				Error("You must set the Read/Write Enabled setting in this object's Mesh import settings.");
			}
		}
	}
}
#endif