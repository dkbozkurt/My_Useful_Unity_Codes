// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace AI.Astar_Pathfinding.CodeMonkey
{
    /// <summary>
    /// A* Pathfinding
    /// 
    /// Ref : https://www.youtube.com/watch?v=alU04hvz6L4&ab_channel=CodeMonkey
    /// </summary>
    public class PathNode
    {
        private Grid<PathNode> _grid;
        public int x;
        public int y;

        public int gCost;
        public int hCost;
        public int fCost;

        public PathNode cameFromNode;
        public bool IsWalkable;
        
        public PathNode(Grid<PathNode> grid, int x,int y)
        {
            _grid = grid;
            this.x = x;
            this.y = y;
            IsWalkable = true;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public void SetIsWalkable(bool isWalkable)
        {
            IsWalkable = isWalkable;
            _grid.TriggerGridObjectChanged(x,y);
        }
        
        public override string ToString()
        {
            return x + "," + y;
        }
    }
}