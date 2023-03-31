using Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Tiles;
using UnityEngine;

namespace Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Units
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=f5pm29yhaTs&ab_channel=Tarodev
    /// </summary>
    public class GridSystemWFunc_BaseUnit : MonoBehaviour
    {
        public string UnitName;
        public GridSystemWFunc_Tile OccupiedTile;
        public Faction Faction;
    
    }
}