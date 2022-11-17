// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Astar_Pathfinding.CodeMonkey
{
    /// <summary>
    /// Generic Grid Class
    /// 
    /// Ref : https://www.youtube.com/watch?v=8jrAWtI8RXg&list=PLzDRvYVwl53uhO8yhqxcyjDImRjO9W722&ab_channel=CodeMonkey
    /// </summary>
    public class Grid<TGridObject>
    {
        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

        public class OnGridObjectChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }
        #region Proporties

        private int _width, _height;
        private float _cellSize;
        private Vector3 _originPosition;
        
        // Defining multi dimensional array
        private TGridObject[,] _gridArray;
        
        #endregion
        
        // Constructor
        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>,int, int,TGridObject> createGridObject)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _originPosition = originPosition;
            
            _gridArray = new TGridObject[width, height];

            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    _gridArray[x, y] = createGridObject(this,x,y);
                }
            }

            bool showDebug = true;

            // Only for Debug
            if (showDebug)
            {
                TextMesh[,] debugTextArray = new TextMesh[width, height];
                
                for (int x = 0; x < _gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < _gridArray.GetLength(1); y++)
                    {
                        debugTextArray[x, y] = CreateWorldText(_gridArray[x, y]?.ToString(), null,
                            GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f,false);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f,false);
                    }
                }

                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f,false);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f,false);

                OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
                {
                    debugTextArray[eventArgs.x, eventArgs.y].text = _gridArray[eventArgs.x, eventArgs.y]?.ToString();
                };

            }
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public float GetCellSize()
        {
            return _cellSize;
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
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
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

        public void SetGridObject(int x, int y, TGridObject value)
        {
            // Ignore un-valid values
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y] = value;
                // // For debug
                // debugTextArray[x, y].text = _gridArray[x, y].ToString();
                if (OnGridObjectChanged != null)
                    OnGridObjectChanged(this, new OnGridObjectChangedEventArgs {x = x, y = y});
            }
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, y;
            GetXY(worldPosition,out x,out y);
            SetGridObject(x,y, value);
        }

        public void TriggerGridObjectChanged(int x,int y)
        {
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x =x,y =y});
        }

        public TGridObject GetGridObject(int x, int y)
        {
            // Ignore un-valid values
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                return _gridArray[x, y];
            }
            else
            {
                return default(TGridObject);
            }
        } 
        
        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x,out y);
            return GetGridObject(x, y);
        }
        
        public void GetXY(Vector3 worldPosition, out int x,out int y) // Or you could use Vector2Int to return 2 int values
        {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize + _originPosition;
        }
    }
}