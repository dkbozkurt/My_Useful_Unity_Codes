// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;

namespace MESH_Generation___Color
{
    /// <summary>
    /// - MESH GENERATION BASICS -
    ///
    /// Attach this script onto "Mesh Generator" new gameObject's inspector.
    ///
    /// 
    /// Ref : https://www.youtube.com/watch?v=eJEpeUH1EMg
    /// </summary>

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshGenerator : MonoBehaviour
    {
        private Mesh mesh;
        private Vector3[] vertices;
        private int[] triangles;

        private void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            //CreateTriangle();
            CreateQuad();
            UpdateMesh();
        }

        private void UpdateMesh()
        {
            mesh.Clear();

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }

        private void CreateTriangle()
        {
            vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 0)
            };

            triangles = new int[]
            {
                0, 1, 2
            };
        }
        
        private void CreateQuad()
        {
            vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 0),
                new Vector3(1,0,1)
            };

            triangles = new int[]
            {
                0, 1, 2,
                1, 3, 2
            };
        }
        
        
    }   
}