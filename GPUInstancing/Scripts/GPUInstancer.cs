using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPUInstancing.Scripts
{
    public class GPUInstancer : MonoBehaviour
    {
        public int Instances;
        public Mesh Mesh;
        public Material[] Materials;

        private List<List<Matrix4x4>> _batches = new List<List<Matrix4x4>>();
        
        private void Start()
        {
            CreateBatches();
        }

        private void Update()
        {
            RenderBatches();
        }

        private void CreateBatches()
        {
            int addedMatricies = 0;
            
            _batches.Add(new List<Matrix4x4>());
            
            for (int i = 0; i < Instances; i++)
            {
                if (addedMatricies < 1000 && _batches.Count != 0)
                {
                    _batches[_batches.Count - 1].Add(Matrix4x4.TRS(
                            new Vector3(x: Random.Range(-20, 20), y: Random.Range(-10, 10), z: Random.Range(0, 300)),
                            Random.rotation,
                            new Vector3(x: Random.Range(1, 3), y: Random.Range(1, 3), z: Random.Range(1, 3))));
                    addedMatricies += 1;
                }
                else
                {
                    _batches.Add(new List<Matrix4x4>());
                    addedMatricies = 0;
                }
            }
        }
        
        private void RenderBatches()
        {
            foreach (var batch in _batches)
            {
                for (int i = 0; i < Mesh.subMeshCount; i++)
                {
                    Graphics.DrawMeshInstanced(Mesh,i,Materials[i],batch);
                }
            }
        }
    }
}
