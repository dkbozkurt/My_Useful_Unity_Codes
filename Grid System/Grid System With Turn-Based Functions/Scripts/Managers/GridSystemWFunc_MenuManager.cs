using Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Tiles;
using Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Units.Heroes;
using TMPro;
using UnityEngine;

namespace Grid_System.Grid_System_With_Turn_Based_Functions.Scripts.Managers
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=f5pm29yhaTs&ab_channel=Tarodev
    /// </summary>
    public class GridSystemWFunc_MenuManager : MonoBehaviour
    {
        public static GridSystemWFunc_MenuManager Instance;

        [SerializeField] private GameObject _selectedHeroObject,_tileObject,_tileUnitObject;

        void Awake() {
            Instance = this;
        }

        public void ShowTileInfo(GridSystemWFunc_Tile tile) {

            if (tile == null)
            {
                _tileObject.SetActive(false);
                _tileUnitObject.SetActive(false);
                return;
            }

            _tileObject.GetComponentInChildren<TextMeshProUGUI>().text = tile.TileName;
            _tileObject.SetActive(true);

            if (tile.OccupiedUnit) {
                _tileUnitObject.GetComponentInChildren<TextMeshProUGUI>().text = tile.OccupiedUnit.UnitName;
                _tileUnitObject.SetActive(true);
            }
        }

        public void ShowSelectedHero(GridSystemWFunc_BaseHero hero) {
            if (hero == null) {
                _selectedHeroObject.SetActive(false);
                return;
            }

            _selectedHeroObject.GetComponentInChildren<TextMeshProUGUI>().text = hero.UnitName;
            _selectedHeroObject.SetActive(true);
        }
    
    }
}