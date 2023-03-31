using UnityEngine;

namespace Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Units
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=f5pm29yhaTs&ab_channel=Tarodev
    /// </summary>
    [CreateAssetMenu(fileName = "New Unit",menuName = "Scriptable Unit")]
    public class GridSystemWFunc_ScriptableUnit : ScriptableObject
    {
        public Faction Faction;
        public GridSystemWFunc_BaseUnit UnitPrefab;
    }
    
    public enum Faction {
        Hero = 0,
        Enemy = 1
    }
}