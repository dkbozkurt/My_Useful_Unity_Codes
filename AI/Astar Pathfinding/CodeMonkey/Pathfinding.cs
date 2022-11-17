// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using DESIGN_PATTERNS.Singleton_Pattern.Singleton_Class;
using UnityEngine;

namespace AI.Astar_Pathfinding.CodeMonkey
{
    /// <summary>
    /// A* Pathfinding
    ///
    /// FOR SQUARE !!!
    /// 
    /// Ref : https://www.youtube.com/watch?v=alU04hvz6L4&ab_channel=CodeMonkey
    /// </summary>
    
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST =10;
        private const int MOVE_DIAGONAL_COST =14;
        
        public static Pathfinding Instance { get; private set; }
        
        private Grid<PathNode> _grid;
        private List<PathNode> _openList;
        private List<PathNode> _closedList;

        public Pathfinding(int width, int height, int cellSize)
        {
            Instance = this;
            // Instantiate grid
            _grid = new Grid<PathNode>(width, height, cellSize, Vector3.zero,
                (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        }

        public Grid<PathNode> GetGrid()
        {
            return _grid;
        }

        public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            _grid.GetXY(startWorldPosition,out int startX,out int startY);
            _grid.GetXY(startWorldPosition,out int endX,out int endY);

            List<PathNode> path = FindPath(startX, startY, endX, endY);

            if (path == null) return null;
            else
            {
                List<Vector3> vectorPath = new List<Vector3>();
                foreach (PathNode pathNode in path)
                {
                    vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * _grid.GetCellSize() + Vector3.one * _grid.GetCellSize() * .5f);
                }

                return vectorPath;
            }

        }
        
        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = _grid.GetGridObject(startX, startY);
            PathNode endNode = _grid.GetGridObject(endX, endY);
            
            _openList = new List<PathNode> { startNode };
            _closedList = new List<PathNode>();

            //
            // Starting data for algorithm
            for (int x = 0; x < _grid.GetWidth() ; x++)
            {
                for (int y = 0; y < _grid.GetHeight(); y++)
                {
                    PathNode pathNode = _grid.GetGridObject(x, y);
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();
            
            //

            while (_openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(_openList);
                
                // Reached final node
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                // Current node already searched
                _openList.Remove(currentNode);
                _closedList.Add(currentNode);
                
                // Cycle through neighbour nodes
                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if(_closedList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.IsWalkable)
                    {
                        _closedList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode,endNode);
                        neighbourNode.CalculateFCost();

                        if (!_openList.Contains(neighbourNode))
                        {
                            _openList.Add(neighbourNode);
                        }
                    }
                }
            }
            
            // Out of nodes on the openList
            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();
            
            if (currentNode.x - 1 >= 0)
            {
                // Left
                neighbourList.Add(GetNode(currentNode.x -1, currentNode.y));
                // Left Down
                if(currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x -1,currentNode.y -1));
                // Left Up
                if(currentNode.y +1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x -1,currentNode.y +1));
            }

            if (currentNode.x + 1 < _grid.GetWidth())
            {
                // Right
                neighbourList.Add(GetNode(currentNode.x +1,currentNode.y));
                // Right Down
                if(currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x +1,currentNode.y -1));
                // Right Up
                if(currentNode.y +1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x +1,currentNode.y +1));
            }
            
            // Down
            if(currentNode.y -1 >= 0) neighbourList.Add(GetNode(currentNode.x,currentNode.y -1));
            // Up
            if(currentNode.y +1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x,currentNode.y +1));

            return neighbourList;
        }

        public PathNode GetNode(int x, int y)
        {
            return _grid.GetGridObject(x, y);
        }
        
        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currentNode = endNode;

            while (currentNode.cameFromNode != null)
            {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            
            path.Reverse();
            return path;
        }
        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int remanining = Mathf.Abs(xDistance - yDistance);

            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remanining;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];
            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].fCost < lowestFCostNode.fCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }
            return lowestFCostNode;
        }
    }
}
