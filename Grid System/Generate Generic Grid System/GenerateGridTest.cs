// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace Grid_System.Generate_Generic_Grid_System
{
    /// <summary>
    /// Generic Grid Test Class
    /// 
    /// Ref : https://www.youtube.com/watch?v=8jrAWtI8RXg&list=PLzDRvYVwl53uhO8yhqxcyjDImRjO9W722&ab_channel=CodeMonkey
    /// </summary>
    public class GenerateGridTest : MonoBehaviour
    {
        [Header("Grid Properties")]
        [SerializeField] private int gridWidth;
        [SerializeField] private int gridHeight;
        [SerializeField] private int cellSize;
        [SerializeField] private Vector3 gridOriginPosition;

        private Grid<HeatMapGridObject> _grid;

        private void Start()
        {
            // Bool Grid Generation
            _grid = new Grid<HeatMapGridObject>(gridWidth, gridHeight, cellSize,gridOriginPosition, 
                (Grid<HeatMapGridObject> g,int x,int y)=>new HeatMapGridObject(g,x,y));
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 position = GetMouseWorldPosition();
                HeatMapGridObject heatMapGridObject = _grid.GetGridObject(position);
                if (heatMapGridObject != null)
                {
                    heatMapGridObject.AddValue(5);
                }
            }
        }

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

    public class HeatMapGridObject
    {
        private const int MIN = 0;
        private const int MAX = 100;

        private Grid<HeatMapGridObject> _grid;
        private int _x;
        private int _y;
        public int value;

        public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x,int y)
        {
            _grid = grid;
            _x = x;
            _y = y;
        }

        public void AddValue(int addValue)
        {
            value += addValue;
            Mathf.Clamp(value, MIN, MAX);
            _grid.TriggerGridObjectChanged(_x,_y);
        }

        public float GetValueNormalized()
        {
            return (float) value / MAX;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}