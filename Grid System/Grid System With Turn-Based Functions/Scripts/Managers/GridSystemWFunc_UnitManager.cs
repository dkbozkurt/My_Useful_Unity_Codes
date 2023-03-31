using System;
using System.Collections.Generic;
using System.Linq;
using Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Units;
using Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Units.Enemies;
using Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Units.Heroes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Random = UnityEngine.Random;

namespace Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Managers
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=f5pm29yhaTs&ab_channel=Tarodev
    /// </summary>
    public class GridSystemWFunc_UnitManager : MonoBehaviour
    {
        public static GridSystemWFunc_UnitManager Instance;

        public GridSystemWFunc_BaseHero SelectedHero;
        
        [SerializeField] private List<GridSystemWFunc_ScriptableUnit> _units;

        void Awake() {
            Instance = this;

            // var objectArray =
            //     AssetDatabase.LoadAllAssetsAtPath("Assets/Grid System/Grid System With Turn-Based Functions/Resources/Units");
            //
            // _units = objectArray.Cast<GridSystemWFunc_ScriptableUnit>().ToList();
        }
        
        public void SpawnHeroes() {
            var heroCount = 1;

            for (int i = 0; i < heroCount; i++) {
                var randomPrefab = GetRandomUnit<GridSystemWFunc_BaseHero>(Faction.Hero);
                var spawnedHero = Instantiate(randomPrefab);
                var randomSpawnTile = GridSystemWFunc_GridManager.Instance.GetHeroSpawnTile();

                randomSpawnTile.SetUnit(spawnedHero);
            }

            GridSystemWFunc_GameManager.Instance.ChangeState(GameState.SpawnEnemies);
        }

        public void SpawnEnemies()
        {
            var enemyCount = 1;

            for (int i = 0; i < enemyCount; i++)
            {
                var randomPrefab = GetRandomUnit<GridSystemWFunc_BaseEnemy>(Faction.Enemy);
                var spawnedEnemy = Instantiate(randomPrefab);
                var randomSpawnTile = GridSystemWFunc_GridManager.Instance.GetEnemySpawnTile();

                randomSpawnTile.SetUnit(spawnedEnemy);
            }

            GridSystemWFunc_GameManager.Instance.ChangeState(GameState.HeroesTurn);
        }

        private T GetRandomUnit<T>(Faction faction) where T : GridSystemWFunc_BaseUnit {
            return (T)_units.Where(u => u.Faction == faction).
                OrderBy(o => Random.value).First().UnitPrefab;
        }

        public void SetSelectedHero(GridSystemWFunc_BaseHero hero) {
            SelectedHero = hero;
            GridSystemWFunc_MenuManager.Instance.ShowSelectedHero(hero);
        }
    
    }
}