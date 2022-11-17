// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Astar_Pathfinding.CodeMonkey
{
    /// <summary>
    /// A* Pathfinding
    ///
    /// Last min for the video 17.45
    /// 
    /// Ref : https://www.youtube.com/watch?v=alU04hvz6L4&ab_channel=CodeMonkey
    /// </summary>
    public class PathfindingTest : MonoBehaviour
    {
        [Header("Grid Properties")]
        [SerializeField] private int _gridWidth = 10;
        [SerializeField] private int _gridHeight = 10;
        [SerializeField] private int _gridCellSize = 10;

        [SerializeField] private PathfindingVisual _pathfindingVisual;
        [SerializeField] private CharacterPathfindingMovementHandler _characterPathfinding;
        private Pathfinding _pathfinding = null;
        
        private void Start()
        {
            _pathfinding = new Pathfinding(_gridWidth, _gridHeight,_gridCellSize);
            _pathfindingVisual.SetGrid(_pathfinding.GetGrid());
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPosition = GetMouseWorldPosition();
                
                _pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
                List<PathNode> path = _pathfinding.FindPath(0, 0, x, y);
                if (path != null)
                {
                    for (int i = 0; i < path.Count-1; i++)
                    {
                        Debug.DrawLine(new Vector3(path[i].x,path[i].y) * 10f + Vector3.one * 5f,
                            new Vector3(path[i+1].x,path[i+1].y) * 10f + Vector3.one * 5f, Color.green,2,false);
                    }
                }
                _characterPathfinding.SetTargetPosition(mouseWorldPosition);
                
            }
            
            // Make grid un walkable
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mouseWorldPosition = GetMouseWorldPosition();
                _pathfinding.GetGrid().GetXY(mouseWorldPosition,out int x,out int y);
                _pathfinding.GetNode(x,y).SetIsWalkable(!_pathfinding.GetNode(x,y).IsWalkable);
            }
        }

        #region Get Mouse Position in World with Z = 0f

        public Vector3 GetMouseWorldPosition()
        {
            Vector3 vector = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vector.z = 0f;
            return vector;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition,Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        #endregion
    }
}