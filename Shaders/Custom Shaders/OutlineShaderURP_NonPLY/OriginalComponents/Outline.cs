using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Runtime.Extensions.UpdateManager;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
namespace Game.Scripts.Runtime.Helpers
{
    [DisallowMultipleComponent]
    public class Outline : BaseMonoBehaviour
    {
        private static readonly HashSet<Mesh> _registeredMeshes = new HashSet<Mesh>();

        public enum Mode
        {
            OutlineAll,
            OutlineVisible,
            OutlineHidden,
            OutlineAndSilhouette,
            SilhouetteOnly
        }
       
        [Serializable]
        private class ListVector3
        {
            public List<Vector3> Data;
        }

        [SerializeField] private Mode _outlineMode = Mode.OutlineAll;
        [SerializeField] private Color _outlineColor = Color.white;

        [Header("Optional")]
        [SerializeField, Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. "
             + "Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
        private bool _precomputeOutline;

        [SerializeField, HideInInspector]
        private List<Mesh> _bakeKeys = new List<Mesh>();

        [SerializeField, HideInInspector]
        private List<ListVector3> _bakeValues = new List<ListVector3>();

        [SerializeField] private Material _outlineMaskMaterial;
        [SerializeField] private Material _outlineFillMaterial;
        
        [SerializeField] private Renderer[] _renderers;
        [SerializeField] private MeshFilter[] _meshes;

        private float _outlineWidth = 1f;
        private static readonly int _color = Shader.PropertyToID("_OutlineColor");
        private static readonly int _zTest = Shader.PropertyToID("_ZTest");
        private static readonly int _width = Shader.PropertyToID("_OutlineWidth");

        [Button]
        private void Setup()
        {
            _renderers = transform.GetComponentsInChildren<Renderer>(true);
            _meshes = transform.GetComponentsInChildren<MeshFilter>(true);
        }

        private void Awake()
        {
            if (_renderers.IsNullOrEmpty())
                return;

            // Retrieve or generate smooth normals
            LoadSmoothNormals();
        }

        public void OnValidate()
        {
            UpdateMaterialProperties();

            // Clear cache when baking is disabled or corrupted
            if (!_precomputeOutline && _bakeKeys.Count != 0 || _bakeKeys.Count != _bakeValues.Count)
            {
                _bakeKeys.Clear();
                _bakeValues.Clear();
            }

            // Generate smooth normals when baking is enabled
            if (_precomputeOutline && _bakeKeys.Count == 0)
            {
                Bake();
            }
        }
        
        protected override void RegisterEvents()
        {
            foreach (var renderer in _renderers)
            {
                // Append outline shaders
                var materials = renderer.sharedMaterials.ToList();

                materials.Add(_outlineMaskMaterial);
                materials.Add(_outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }
        }

        protected override void UnregisterEvents()
        {
            foreach (var renderer in _renderers)
            {
                // Remove outline shaders
                var materials = renderer.sharedMaterials.ToList();

                materials.Remove(_outlineMaskMaterial);
                materials.Remove(_outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }
        }

        private void Bake()
        {
            // Generate smooth normals for each mesh
            var bakedMeshes = new HashSet<Mesh>();

            foreach (var meshFilter in _meshes)
            {
                // Skip duplicates
                if (!bakedMeshes.Add(meshFilter.sharedMesh))
                {
                    continue;
                }

                // Serialize smooth normals
                var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

                _bakeKeys.Add(meshFilter.sharedMesh);
                _bakeValues.Add(new ListVector3()
                {
                    Data = smoothNormals
                });
            }
        }

        private void LoadSmoothNormals()
        {
            // Retrieve or generate smooth normals
            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
            {

                // Skip if smooth normals have already been adopted
                if (!_registeredMeshes.Add(meshFilter.sharedMesh))
                {
                    continue;
                }

                // Retrieve or generate smooth normals
                var index = _bakeKeys.IndexOf(meshFilter.sharedMesh);
                var smoothNormals = (index >= 0) ? _bakeValues[index].Data : SmoothNormals(meshFilter.sharedMesh);

                // Store smooth normals in UV3
                meshFilter.sharedMesh.SetUVs(3, smoothNormals);

                // Combine submeshes
                var renderer = meshFilter.GetComponent<Renderer>();

                if (renderer != null)
                {
                    CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
                }
            }

            // Clear UV3 on skinned mesh renderers
            foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {

                // Skip if UV3 has already been reset
                if (!_registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
                {
                    continue;
                }

                // Clear UV3
                var sharedMesh = skinnedMeshRenderer.sharedMesh;
                sharedMesh.uv4 = new Vector2[sharedMesh.vertexCount];

                // Combine submeshes
                CombineSubmeshes(sharedMesh, skinnedMeshRenderer.sharedMaterials);
            }
        }

        private static List<Vector3> SmoothNormals(Mesh mesh)
        {
            // Group vertices by location
            var groups = mesh.vertices.Select((vertex, index) =>
                new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

            // Copy normals to a new list
            var smoothNormals = new List<Vector3>(mesh.normals);

            // Average normals for grouped vertices
            foreach (var group in groups)
            {

                // Skip single vertices
                if (group.Count() == 1)
                {
                    continue;
                }

                // Calculate the average normal
                var smoothNormal = Vector3.zero;

                foreach (var pair in group)
                {
                    smoothNormal += smoothNormals[pair.Value];
                }

                smoothNormal.Normalize();

                // Assign smooth normal to each vertex
                foreach (var pair in group)
                {
                    smoothNormals[pair.Value] = smoothNormal;
                }
            }

            return smoothNormals;
        }

        private static void CombineSubmeshes(Mesh mesh, Material[] materials)
        {

            // Skip meshes with a single submesh
            if (mesh.subMeshCount == 1)
            {
                return;
            }

            // Skip if submesh count exceeds material count
            if (mesh.subMeshCount > materials.Length)
            {
                return;
            }

            // Append combined submesh
            mesh.subMeshCount++;
            mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
        }

        [Button]
        private void UpdateMaterialProperties()
        {
            // Apply properties according to mode
            _outlineFillMaterial.SetColor(_color, _outlineColor);

            switch (_outlineMode)
            {
                case Mode.OutlineAll:
                    _outlineMaskMaterial.SetFloat(_zTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat(_zTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat(_width, _outlineWidth);
                    break;

                case Mode.OutlineVisible:
                    _outlineMaskMaterial.SetFloat(_zTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat(_zTest, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    _outlineFillMaterial.SetFloat(_width, _outlineWidth);
                    break;

                case Mode.OutlineHidden:
                    _outlineMaskMaterial.SetFloat(_zTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat(_zTest, (float)UnityEngine.Rendering.CompareFunction.Greater);
                    _outlineFillMaterial.SetFloat(_width, _outlineWidth);
                    break;

                case Mode.OutlineAndSilhouette:
                    _outlineMaskMaterial.SetFloat(_zTest, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    _outlineFillMaterial.SetFloat(_zTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                    _outlineFillMaterial.SetFloat(_width, _outlineWidth);
                    break;

                case Mode.SilhouetteOnly:
                    _outlineMaskMaterial.SetFloat(_zTest, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    _outlineFillMaterial.SetFloat(_zTest, (float)UnityEngine.Rendering.CompareFunction.Greater);
                    _outlineFillMaterial.SetFloat(_width, 0f);
                    break;
            }
        }
    }
}
