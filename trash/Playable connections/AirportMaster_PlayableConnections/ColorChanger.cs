using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Playable.Scripts.PreScripts
{
    public class ColorChanger : MonoBehaviour
    {
        [LunaPlaygroundField("First Main Car", 0, "Color settings")] [SerializeField]
        private Color _firstMainCarColor;

        [LunaPlaygroundField("Customer", 1, "Color settings")] [SerializeField]
        private Color _firstCustomerColor;

        [LunaPlaygroundField("Tutorial Text", 2, "Color settings")] [SerializeField]
        private Color _tutorialTextColor;

        [LunaPlaygroundField("End Card Background", 3, "Color settings")] [SerializeField]
        private Color _backgroundColor;

        [LunaPlaygroundField("Terrain", 4, "Color Settings")] [SerializeField]
        private Color _terrainColor;
        
        [SerializeField] private Material _firstMainCar;
        [SerializeField] private Material _customer1;

        [SerializeField] private TextMeshProUGUI tutorialText;
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Material _terrain;

        private void Awake()
        {
            _firstMainCar.color = _firstMainCarColor;

            _customer1.color = _firstCustomerColor;

            tutorialText.color = _tutorialTextColor;
            backgroundImg.color = _backgroundColor;
            _terrain.color = _terrainColor;
        }
    }
}