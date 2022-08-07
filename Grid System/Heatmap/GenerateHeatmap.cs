// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;


namespace Grid_System.Heatmap
{
    /// <summary>
    /// Create heatmap by using grids
    ///
    /// Use this script with "Grid_HM.cs" script
    /// 
    /// Ref : https://www.youtube.com/watch?v=mZzZXfySeFQ
    /// </summary>
    public class GenerateHeatmap : MonoBehaviour
    {
        [SerializeField] private HeatMapVisual heatMapVisual;
        private Grid_HM _grid;

        private void Start()
        {
            _grid = new Grid_HM(150, 100, 2f, Vector3.zero);
            
            heatMapVisual.SetGrid(_grid);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 position = GetMouseWorldPosition();
                _grid.AddValue(position,100,2,25);
            }
        }
        
        // Get Mouse Position in world with Z = 0f
        #region Get Mouse Position in World with Z = 0f

        public Vector3 GetMouseWorldPosition()
        {
            Vector3 vector = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vector.z = 0f;
            return vector;
        }

        public Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition,Camera.main);
        }

        public Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        #endregion
    }
}