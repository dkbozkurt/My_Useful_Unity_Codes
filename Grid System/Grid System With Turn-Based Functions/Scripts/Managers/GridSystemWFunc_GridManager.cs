// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using System.Linq;
using Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Managers
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=f5pm29yhaTs&ab_channel=Tarodev
    /// </summary>
    public class GridSystemWFunc_GridManager : MonoBehaviour
    {
        public static GridSystemWFunc_GridManager Instance;
        
        [SerializeField] private int _width = 9;
        [SerializeField] private int _height = 20;
 
        [SerializeField] private GridSystemWFunc_Tile _grassTile;
        [SerializeField] private GridSystemWFunc_Tile _mountainTile;

        private Dictionary<Vector2, GridSystemWFunc_Tile> _tiles;

        private void Awake()
        {
            Instance = this;
        }
        
        public void GenerateGrid() {
            _tiles = new Dictionary<Vector2, GridSystemWFunc_Tile>();
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++)
                {
                    var randomTile = Random.Range(0, 6) == 3 ? _mountainTile : _grassTile;
                    var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}";
 
                    spawnedTile.Init(x,y);
 
 
                    _tiles[new Vector2(x, y)] = spawnedTile;
                }
            }
 
            GridSystemWFunc_GameManager.Instance.ChangeState(GameState.SpawnHeroes);
        }
        public GridSystemWFunc_Tile GetHeroSpawnTile() {
            return _tiles.Where(t => t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
        }

        public GridSystemWFunc_Tile GetEnemySpawnTile()
        {
            return _tiles.Where(t => t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
        }
        
        public GridSystemWFunc_Tile GetTileAtPosition(Vector2 pos) {
            if (_tiles.TryGetValue(pos, out var tile)) return tile;
            return null;
        }
    
    }
}