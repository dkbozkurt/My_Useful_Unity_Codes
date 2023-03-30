// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using UnityEngine;

namespace Grid_System.Grid_System_Tarodev.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=kkAjpQAM-jE&ab_channel=Tarodev
    /// </summary>
    public class GridSystem_GridManager : MonoBehaviour
    {
        [SerializeField] private int _width = 9;
        [SerializeField] private int _height = 20;
 
        [SerializeField] private GridSystem_Tile _tilePrefab;

        private Dictionary<Vector2, GridSystem_Tile> _tiles;

        private Transform _cam;
        
        void Start() {
            _cam = Camera.main.transform;
            GenerateGrid();
        }
 
        void GenerateGrid() {
            _tiles = new Dictionary<Vector2, GridSystem_Tile>();
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}";
 
                    var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                    spawnedTile.Init(isOffset);
 
 
                    _tiles[new Vector2(x, y)] = spawnedTile;
                }
            }
 
            // _cam.transform.position = new Vector3((float)_width/2 -0.5f, (float)_height / 2 - 0.5f,-10);
        }
 
        public GridSystem_Tile GetTileAtPosition(Vector2 pos) {
            if (_tiles.TryGetValue(pos, out var tile)) return tile;
            return null;
        }
    
    }
}