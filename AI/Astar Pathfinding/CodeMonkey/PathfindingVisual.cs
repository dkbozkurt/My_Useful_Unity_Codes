using System;
using UnityEngine;

namespace AI.Astar_Pathfinding.CodeMonkey
{
    [RequireComponent(typeof(MeshFilter))]
    public class PathfindingVisual : MonoBehaviour
    {
        private Grid<PathNode> _grid;
        private Mesh _mesh;
        private bool _updateMesh;

        private void Awake()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
        }

        public void SetGrid(Grid<PathNode> grid)
        {
            _grid = grid;
            UpdateVisual();

            _grid.OnGridObjectChanged += Grid_OnGridValueChanged;
        }

        private void Grid_OnGridValueChanged(object sender, Grid<PathNode>.OnGridObjectChangedEventArgs e)
        {
            _updateMesh = true;
        }

        private void LateUpdate()
        {
            if (_updateMesh)
            {
                _updateMesh = false;
                UpdateVisual();
            }
        }

        private void UpdateVisual()
        {
            // Add into utils.
            CreateEmptyMeshArrays(_grid.GetWidth() * _grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv,
                out int[] triangles);

            for (int x = 0; x < _grid.GetWidth(); x++)
            {
                for (int y = 0; y < _grid.GetHeight(); y++)
                {
                    int index = x * _grid.GetHeight() + y;
                    Vector3 quadSize = new Vector3(1, 1) * _grid.GetCellSize();

                    PathNode pathNode = _grid.GetGridObject(x, y);
                    
                    if(pathNode.IsWalkable) quadSize = Vector3.zero;
                    
                    AddToMeshArrays(vertices,uv,triangles,index,_grid.GetWorldPosition(x,y) + quadSize * .5f,
                        0f,quadSize,Vector2.zero, Vector2.zero);
                }
            }
            
            _mesh.vertices = vertices;
            _mesh.uv = uv;
            _mesh.triangles = triangles;
        }

        #region MeshUtils

        private static Quaternion[] cachedQuaternionEulerArr;
        
        public static void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles) {
            vertices = new Vector3[4 * quadCount];
            uvs = new Vector2[4 * quadCount];
            triangles = new int[6 * quadCount];
        }

        public static void AddToMeshArrays(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11) {
            //Relocate vertices
            int vIndex = index*4;
            int vIndex0 = vIndex;
            int vIndex1 = vIndex+1;
            int vIndex2 = vIndex+2;
            int vIndex3 = vIndex+3;

            baseSize *= .5f;

            bool skewed = baseSize.x != baseSize.y;
            if (skewed) {
                vertices[vIndex0] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x,  baseSize.y);
                vertices[vIndex1] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x, -baseSize.y);
                vertices[vIndex2] = pos+GetQuaternionEuler(rot)*new Vector3( baseSize.x, -baseSize.y);
                vertices[vIndex3] = pos+GetQuaternionEuler(rot)*baseSize;
            } else {
                vertices[vIndex0] = pos+GetQuaternionEuler(rot-270)*baseSize;
                vertices[vIndex1] = pos+GetQuaternionEuler(rot-180)*baseSize;
                vertices[vIndex2] = pos+GetQuaternionEuler(rot- 90)*baseSize;
                vertices[vIndex3] = pos+GetQuaternionEuler(rot-  0)*baseSize;
            }
		
            //Relocate UVs
            uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
            uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
            uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
            uvs[vIndex3] = new Vector2(uv11.x, uv11.y);
		
            //Create triangles
            int tIndex = index*6;
		
            triangles[tIndex+0] = vIndex0;
            triangles[tIndex+1] = vIndex3;
            triangles[tIndex+2] = vIndex1;
		
            triangles[tIndex+3] = vIndex1;
            triangles[tIndex+4] = vIndex3;
            triangles[tIndex+5] = vIndex2;
        }
        
        private static Quaternion GetQuaternionEuler(float rotFloat) {
            int rot = Mathf.RoundToInt(rotFloat);
            rot = rot % 360;
            if (rot < 0) rot += 360;
            //if (rot >= 360) rot -= 360;
            if (cachedQuaternionEulerArr == null) CacheQuaternionEuler();
            return cachedQuaternionEulerArr[rot];
        }
        
        private static void CacheQuaternionEuler() {
            if (cachedQuaternionEulerArr != null) return;
            cachedQuaternionEulerArr = new Quaternion[360];
            for (int i=0; i<360; i++) {
                cachedQuaternionEulerArr[i] = Quaternion.Euler(0,0,i);
            }
        }
        #endregion
        
    }
}
