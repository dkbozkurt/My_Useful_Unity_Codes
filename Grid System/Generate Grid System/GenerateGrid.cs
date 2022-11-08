// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Grid_System.Generate_Grid_System
{
    /// <summary>
    /// Grid Test Class
    /// 
    /// Ref : https://www.youtube.com/watch?v=waEsGu--9P8
    /// </summary>
    public class GenerateGrid : MonoBehaviour
    {
        [Header("Grid Properties")]
        [SerializeField] private int gridWidth;
        [SerializeField] private int gridHeight;
        [SerializeField] private int cellSize;
        [SerializeField] private Vector3 gridOriginPosition;

        private Grid _grid;

        private void Start()
        {
            // Generate Grid
            _grid = new Grid(gridWidth, gridHeight, cellSize,gridOriginPosition);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClickAssignValue();
            }

            if (Input.GetMouseButtonDown(1))
            {
                OnClickReadValue();
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

        private void OnClickAssignValue()
        {
            _grid.SetValue(GetMouseWorldPosition(),SetARandomIntValue());
        }

        private void OnClickReadValue()
        {
            Debug.Log(_grid.GetValue(GetMouseWorldPosition()));
        }

        private int SetARandomIntValue()
        {
            return Random.Range(0, 100);
        }
        
    }
}