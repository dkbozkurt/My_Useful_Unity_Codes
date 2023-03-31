using System;
using UnityEngine;

namespace Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Managers
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=f5pm29yhaTs&ab_channel=Tarodev
    /// </summary>
    public class GridSystemWFunc_GameManager : MonoBehaviour
    {
        public static GridSystemWFunc_GameManager Instance;
        public GameState GameState;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            ChangeState(GameState.GenerateGrid);
        }

        public void ChangeState(GameState newState)
        {
            GameState = newState;
            switch (newState)
            {
                case GameState.GenerateGrid:
                    GridSystemWFunc_GridManager.Instance.GenerateGrid();
                    break;
                case GameState.SpawnHeroes:
                    GridSystemWFunc_UnitManager.Instance.SpawnHeroes();
                    break;
                case GameState.SpawnEnemies:
                    GridSystemWFunc_UnitManager.Instance.SpawnEnemies();
                    break;
                case GameState.HeroesTurn:
                    break;
                case GameState.EnemiesTurn:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
    }

    public enum GameState
    {
        GenerateGrid = 0,
        SpawnHeroes = 1,
        SpawnEnemies = 2,
        HeroesTurn = 3,
        EnemiesTurn = 4
    }
    
}
