// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine;

namespace DESIGN_PATTERNS.PrototypePattern.Scripts
{
    /// <summary>
    /// 
    /// </summary>

    public class ProcSphere : MonoBehaviour
    {
        public static GameObject sphere;

        public static GameObject Clone(Vector3 initialPos)
        {
            if (sphere == null)
            {
                CreateSphere(Vector3.zero);
                sphere.SetActive(false);
            }

            GameObject sphereClone = new GameObject();
            sphereClone.AddComponent<MeshFilter>();
            sphereClone.AddComponent<MeshRenderer>();
            sphereClone.GetComponent<MeshFilter>().mesh = sphere.GetComponent<MeshFilter>().mesh;
            MeshRenderer rend = sphereClone.GetComponent<MeshRenderer>();
            rend.material = sphere.GetComponent<MeshRenderer>().material;
            sphereClone.AddComponent<Rigidbody>();
            sphereClone.AddComponent<SphereCollider>();
            sphereClone.name = "Sphere(Clone)";
            sphereClone.gameObject.SetActive(true);
            sphereClone.transform.position = initialPos;
            return sphereClone;
        }
        
        public static void CreateSphere(Vector3 pos)
        {
            sphere = new GameObject();
            sphere.AddComponent<MeshFilter>();
            sphere.AddComponent<MeshRenderer>();
    
            //Construct Sphere Mesh =======================
            Mesh mesh = new Mesh();
            mesh.name = "Sphere_" + Time.realtimeSinceStartup.ToString();
            mesh.Clear();
    
            float radius = 1f; 
            int LONGITUDE = 20;
            int LATITUDE = 20;
    
            Vector3[] vertices = new Vector3[(LONGITUDE + 1) * LATITUDE + 2 * LONGITUDE];
            float PI2 = Mathf.PI * 2f;
    
            //NORTH POLE
            for (int v = 0; v <= LONGITUDE; v++)
            {
                vertices[v] = Vector3.up * radius;
            }
    
            for (int lat = 0; lat < LATITUDE; lat++)
            {
                float a1 = Mathf.PI * (float)(lat + 1) / (LATITUDE + 1);
                float sin1 = Mathf.Sin(a1);
                float cos1 = Mathf.Cos(a1);
    
                for (int lon = 0; lon <= LONGITUDE; lon++)
                {
                    float a2 = PI2 * (float)(lon == LONGITUDE ? 0 : lon) / LONGITUDE;
                    float sin2 = Mathf.Sin(a2);
                    float cos2 = Mathf.Cos(a2);
    
                    vertices[lon + lat * (LONGITUDE + 1) + LONGITUDE] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
                }
            }
    
            //SOUTH POLE
            for (int v = 1; v <= LONGITUDE; v++)
            {
                vertices[vertices.Length - v] = Vector3.up * -radius;   //last vertex - bottom most
            }
    
            Vector3[] normals = new Vector3[vertices.Length];
            for (int n = 0; n < vertices.Length; n++)
            {
                normals[n] = vertices[n].normalized;
            }
    
            Vector2[] uvs = new Vector2[vertices.Length];
            uvs[0] = Vector2.up;
            uvs[uvs.Length - 1] = Vector2.zero;
            for (int lat = 0; lat < LATITUDE; lat++)
                for (int lon = 0; lon <= LONGITUDE; lon++)
                {
                    int uvpos = lon + lat * (LONGITUDE + 1) + LONGITUDE;
                    uvs[uvpos] =
                            new Vector2((float)lon / LONGITUDE,
                                1f - (float)(lat + 1) / (LATITUDE + 1));
    
                }
    
            //top cap uvs
            float uvOffset = 1 / (float)LONGITUDE * 0.5f;
            for (int v = 0; v < LONGITUDE; v++)
            {
                uvs[v] = new Vector2(1 / (float)LONGITUDE * v + uvOffset, 1);
            }
    
            //bottom cap uvs
            int u = 0;
            for (int v = vertices.Length - LONGITUDE; v < vertices.Length; v++)
            {
                uvs[v] = new Vector2(1 / (float)LONGITUDE * u + uvOffset, 0);
                u++;
            }
    
            int totalFaces = vertices.Length;
            int totalTris = totalFaces * 2;
            int triIndexes = totalTris * 3;
            int[] triangles = new int[triIndexes];
    
            //Top Cap
            int i = 0;
            for (int lon = 0; lon <= LONGITUDE; lon++)
            {
                triangles[i++] = LONGITUDE + lon + 1;
                triangles[i++] = LONGITUDE + lon;
                triangles[i++] = lon;
            }
    
            //Middle
            for (int lat = 0; lat < LATITUDE - 1; lat++)
            {
                for (int lon = 0; lon < LONGITUDE; lon++)
                {
                    int current = lon + lat * (LONGITUDE + 1) + LONGITUDE;
                    int next = current + LONGITUDE + 1;
    
                    triangles[i++] = current;
                    triangles[i++] = current + 1;
                    triangles[i++] = next + 1;
    
                    triangles[i++] = current;
                    triangles[i++] = next + 1;
                    triangles[i++] = next;
                }
            }
    
            //Bottom Cap
            for (int lon = 0; lon <= LONGITUDE; lon++)
            {
                triangles[i++] = vertices.Length - 1 - lon;
                triangles[i++] = vertices.Length - LONGITUDE - (lon + 2);
                triangles[i++] = vertices.Length - LONGITUDE - (lon + 1);
            }
    
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
    
            mesh.RecalculateBounds();
            //=========================================================
    
            sphere.GetComponent<MeshFilter>().mesh = mesh;
    
            MeshRenderer rend = sphere.GetComponent<MeshRenderer>();
            rend.material = new Material(Shader.Find("Holistic/Plasma"));
            sphere.AddComponent<Rigidbody>();
            sphere.AddComponent<SphereCollider>();
            sphere.name = "Sphere";
            sphere.gameObject.SetActive(true);
            sphere.transform.position = pos;
        }

    }
}
