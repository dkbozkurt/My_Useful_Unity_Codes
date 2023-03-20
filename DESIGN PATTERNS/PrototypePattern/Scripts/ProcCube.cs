using System.Collections.Generic;
using UnityEngine;

namespace DESIGN_PATTERNS.PrototypePattern.Scripts
{
    public class ProcCube: Object
    {
        public enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };

        //
        
        private static GameObject cube;

        public static GameObject Clone(Vector3 initialPos)
        {
            if (cube == null)
            {
                CreateCube(Vector3.zero);
                cube.SetActive(false);
            }

            GameObject cubeClone = new GameObject();
            cubeClone.AddComponent<MeshFilter>();
            cubeClone.AddComponent<MeshRenderer>();
            cubeClone.GetComponent<MeshFilter>().mesh = cube.GetComponent<MeshFilter>().mesh;
            MeshRenderer rend = cubeClone.GetComponent<MeshRenderer>();
            rend.material = cube.GetComponent<MeshRenderer>().material;
            cubeClone.AddComponent<Rigidbody>();
            cubeClone.AddComponent<BoxCollider>();
            cubeClone.name = "Cube(Clone)";
            cubeClone.gameObject.SetActive(true);
            cubeClone.transform.position = initialPos;
            return cubeClone;
        }
        
        
        //
        public static void CreateQuad(Cubeside side, GameObject parent)
        {
            Mesh mesh = new Mesh();
            mesh.name = "ScriptedMesh" + side.ToString();

            Vector3[] vertices = new Vector3[4];
            Vector3[] normals = new Vector3[4];
            Vector2[] uvs = new Vector2[4];
            int[] triangles = new int[6];

            //all possible UVs
            Vector2 uv00 = new Vector2(0f, 0f);
            Vector2 uv10 = new Vector2(1f, 0f);
            Vector2 uv01 = new Vector2(0f, 1f);
            Vector2 uv11 = new Vector2(1f, 1f);

            //all possible vertices 
            Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
            Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
            Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
            Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
            Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
            Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
            Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
            Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

            switch (side) //**
            {
                case Cubeside.BOTTOM:
                    vertices = new Vector3[] { p0, p1, p2, p3 };
                    normals = new Vector3[] {Vector3.down, Vector3.down,
                        Vector3.down, Vector3.down};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
                case Cubeside.TOP:
                    vertices = new Vector3[] { p7, p6, p5, p4 };
                    normals = new Vector3[] {Vector3.up, Vector3.up,
                        Vector3.up, Vector3.up};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
                case Cubeside.LEFT:
                    vertices = new Vector3[] { p7, p4, p0, p3 };
                    normals = new Vector3[] {Vector3.left, Vector3.left,
                        Vector3.left, Vector3.left};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
                case Cubeside.RIGHT:
                    vertices = new Vector3[] { p5, p6, p2, p1 };
                    normals = new Vector3[] {Vector3.right, Vector3.right,
                        Vector3.right, Vector3.right};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
                case Cubeside.FRONT:
                    vertices = new Vector3[] { p4, p5, p1, p0 };
                    normals = new Vector3[] {Vector3.forward, Vector3.forward,
                        Vector3.forward, Vector3.forward};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
                case Cubeside.BACK:
                    vertices = new Vector3[] { p6, p7, p3, p2 };
                    normals = new Vector3[] {Vector3.back, Vector3.back,
                        Vector3.back, Vector3.back};
                    uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                    triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                    break;
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            mesh.RecalculateBounds();

            GameObject quad = new GameObject("Quad");
            quad.transform.parent = parent.transform;
            MeshFilter meshFilter = quad.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }

        public static void CreateCube(Vector3 pos)
        {
            cube = new GameObject();
            cube.AddComponent<MeshFilter>();
            cube.AddComponent<MeshRenderer>();
            CreateQuad(Cubeside.FRONT, cube);
            CreateQuad(Cubeside.BACK, cube);
            CreateQuad(Cubeside.TOP, cube);
            CreateQuad(Cubeside.BOTTOM, cube);
            CreateQuad(Cubeside.LEFT, cube);
            CreateQuad(Cubeside.RIGHT, cube);

            MeshFilter[] meshFilters = cube.GetComponentsInChildren<MeshFilter>();


            //remove any nulls
            List<MeshFilter> filters = new List<MeshFilter>();
            int i = 0;
            while (i < meshFilters.Length)
            {
                if (meshFilters[i].sharedMesh != null)
                {
                    filters.Add(meshFilters[i]);
                }
                i++;
            }

            CombineInstance[] combine = new CombineInstance[filters.Count];
            i = 0;
            foreach(MeshFilter m in filters)
            {
                combine[i].mesh = m.mesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                m.gameObject.SetActive(false);
                i++; 
            }
            
            //

            MeshFilter cubeMeshFilter = cube.GetComponent<MeshFilter>(); 
            cubeMeshFilter.mesh = new Mesh();
            cubeMeshFilter.mesh.CombineMeshes(combine);
            cubeMeshFilter.mesh.name = "CreatedCube" + Time.realtimeSinceStartup;

            MeshRenderer rend = cube.GetComponent<MeshRenderer>();
            rend.material = new Material(Shader.Find("Holistic/Plasma"));
            
            cube.AddComponent<Rigidbody>();
            cube.AddComponent<BoxCollider>();
            cube.name = "Cube";
            
            cube.gameObject.SetActive(true);
            cube.transform.position = pos;
        }
    }
}
