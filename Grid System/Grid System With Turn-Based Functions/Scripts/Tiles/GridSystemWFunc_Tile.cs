// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Managers;
using Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Units;
using Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Units.Enemies;
using Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Units.Heroes;
using UnityEngine;

namespace Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Tiles
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=f5pm29yhaTs&ab_channel=Tarodev
    /// </summary>
    public abstract class GridSystemWFunc_Tile : MonoBehaviour
    {
        public string TileName;
        
        [SerializeField] private GameObject _highlight;
        [SerializeField] private bool _isWalkable;

        public GridSystemWFunc_BaseUnit OccupiedUnit;
        public bool Walkable => _isWalkable && OccupiedUnit == null;
        protected SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public virtual void Init(int x, int y)
        {
            
        }
 
        void OnMouseEnter() {
            _highlight.SetActive(true);
            GridSystemWFunc_MenuManager.Instance.ShowTileInfo(this);
        }

        private void OnMouseDown()
        {
            if(GridSystemWFunc_GameManager.Instance.GameState != GameState.HeroesTurn) return;

            if (OccupiedUnit != null) {
                if(OccupiedUnit.Faction == Faction.Hero) GridSystemWFunc_UnitManager.Instance.SetSelectedHero((GridSystemWFunc_BaseHero)OccupiedUnit);
                else {
                    if (GridSystemWFunc_UnitManager.Instance.SelectedHero != null) {
                        Debug.Log("Grid name: " + gameObject.name);
                        var enemy = (GridSystemWFunc_BaseEnemy) OccupiedUnit;
                        Destroy(enemy.gameObject);
                        GridSystemWFunc_UnitManager.Instance.SetSelectedHero(null);
                    }
                }
            }
            else {
                if (GridSystemWFunc_UnitManager.Instance.SelectedHero != null) {
                    Debug.Log("Grid name: " + gameObject.name);
                    
                    if(!Walkable) return;
                    SetUnit(GridSystemWFunc_UnitManager.Instance.SelectedHero);
                    GridSystemWFunc_UnitManager.Instance.SetSelectedHero(null);
                }
            }
        }

        void OnMouseExit()
        {
            _highlight.SetActive(false);
            GridSystemWFunc_MenuManager.Instance.ShowTileInfo(null);
        }
        
        public void SetUnit(GridSystemWFunc_BaseUnit unit) {
            if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
            unit.transform.position = transform.position;
            OccupiedUnit = unit;
            unit.OccupiedTile = this;
        }
    
    }
}