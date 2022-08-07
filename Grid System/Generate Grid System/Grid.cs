// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using TMPro;
using UnityEngine;

namespace Grid_System.Generate_Grid_System
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=waEsGu--9P8
    /// </summary>
    public class Grid
    {
        #region Proporties

        private int _width, _height;
        private float _cellSize;
        private Vector3 _originPosition;
        
        // Defining multi dimensional array
        private int[,] _gridArray;

        // Only for Debug
        private TextMesh[,] _debugTextArray;
        
        #endregion
        
        // Constructor
        public Grid(int width, int height, float cellSize, Vector3 originPosition)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _originPosition = originPosition;
            
            _gridArray = new int[width, height];
            // Only for Debug
            _debugTextArray = new TextMesh[width, height];


            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    _debugTextArray[x, y] = CreateWorldText(_gridArray[x, y].ToString(), null,
                        GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            SetValue(2, 1, 56);
        }

        #region Create World Text

        public TextMesh CreateWorldText(string text,
            Transform parent, Vector3 localPosition,
            int fontSize, Color color, TextAnchor textAnchor = TextAnchor.MiddleCenter,
            TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 0)
        {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color) color, textAnchor, textAlignment,
                sortingOrder);
        }

        public TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color,
            TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMeshPro));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        #endregion

        public void SetValue(int x, int y, int value)
        {
            // Ignore un-valid values
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y] = value;
                _debugTextArray[x, y].text = _gridArray[x, y].ToString();
            }
        }

        public void SetValue(Vector3 worldPosition, int value)
        {
            int x, y;
            GetXY(worldPosition,out x,out y);
            SetValue(x,y, value);
        }

        public int GetValue(int x, int y)
        {
            // Ignore un-valid values
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                return _gridArray[x, y];
            }
            else
            {
                return 0;
            }
        } 
        
        public int GetValue(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x,out y);
            return GetValue(x, y);
        }
        
        private void GetXY(Vector3 worldPosition, out int x,out int y) // Or you could use Vector2Int to return 2 int values
        {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
            
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize + _originPosition;
        }
    }
}