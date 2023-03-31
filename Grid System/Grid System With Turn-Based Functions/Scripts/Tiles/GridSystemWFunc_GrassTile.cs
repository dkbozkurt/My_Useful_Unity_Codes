using UnityEngine;

namespace Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Tiles
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=f5pm29yhaTs&ab_channel=Tarodev
    /// </summary>
    public class GridSystemWFunc_GrassTile : GridSystemWFunc_Tile
    {
        [SerializeField] private Color _baseColor;
        [SerializeField] private Color _offsetColor;

        public override void Init(int x, int y)
        {
            var isOffset = (x + y) % 2 == 1;
            _renderer.color = isOffset ? _offsetColor : _baseColor;
        }
    }
}