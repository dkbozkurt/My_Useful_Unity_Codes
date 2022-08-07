/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using UnityEngine;

namespace Grid_System.Heatmap
{
    public class HeatMapVisual : MonoBehaviour {

        private Grid_HM grid;
        private Mesh mesh;
        private bool updateMesh;

        private void Awake() {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        public void SetGrid(Grid_HM grid) {
            this.grid = grid;
            UpdateHeatMapVisual();

            grid.OnGridValueChanged += Grid_OnGridValueChanged;
        }

        private void Grid_OnGridValueChanged(object sender, Grid_HM.OnGridValueChangedEventArgs e) {
            //UpdateHeatMapVisual();
            updateMesh = true;
        }

        private void LateUpdate() {
            if (updateMesh) {
                updateMesh = false;
                UpdateHeatMapVisual();
            }
        }

        private void UpdateHeatMapVisual() {
            MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

            for (int x = 0; x < grid.GetWidth(); x++) {
                for (int y = 0; y < grid.GetHeight(); y++) {
                    int index = x * grid.GetHeight() + y;
                    Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                    int gridValue = grid.GetValue(x, y);
                    float gridValueNormalized = (float)gridValue / Grid_HM.HEAT_MAP_MAX_VALUE;
                    Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);
                    MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValueUV, gridValueUV);
                }
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }

    }
}
