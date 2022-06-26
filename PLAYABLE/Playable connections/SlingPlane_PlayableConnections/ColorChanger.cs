using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Playable
{
    public class ColorChanger : MonoBehaviour
    {
        [LunaPlaygroundField("End card color", 0, "Color settings")] [SerializeField]
        private Color _endCardColor;

        [LunaPlaygroundField("Rope color1", 1, "Color settings")] [SerializeField]
        private Color _ropeColor1;

        [LunaPlaygroundField("Rope color2", 2, "Color settings")] [SerializeField]
        private Color _ropeColor2;

        [SerializeField] private Image _endCardBG;
        [SerializeField] private Material _rope1;
        [SerializeField] private Material _rope2;
        
        // Added for Playable Version2
        //
        [LunaPlaygroundField("SlingShot color", 3, "Color settings")] [SerializeField]
        private Color _slingShotColor;

        [LunaPlaygroundField("Ground color", 4, "Color settings")] [SerializeField]
        private Color _groundColor;
        
        [LunaPlaygroundField("Tree color", 5, "Color settings")] [SerializeField]
        private Color _treeColor;
        
        [LunaPlaygroundField("Tunnel color1", 6, "Color settings")] [SerializeField]
        private Color _tunnelColor1;
        
        [LunaPlaygroundField("Tunnel color2", 7, "Color settings")] [SerializeField]
        private Color _tunnelColor2;
        
        [LunaPlaygroundField("Tunnel color3", 8, "Color settings")] [SerializeField]
        private Color _tunnelColor3;
        
        [LunaPlaygroundField("Arrow color", 9, "Color settings")] [SerializeField]
        private Color _arrowColor;
        
        
        [SerializeField] private Material _slingShot;
        [SerializeField] private Material _ground;
        [SerializeField] private Material _tree;
        [SerializeField] private Material _tunnel1;
        [SerializeField] private Material _tunnel2;
        [SerializeField] private Material _tunnel3;
        [SerializeField] private Material _arrow;
        
        //

        private void Awake()
        {
            _endCardBG.color = _endCardColor;
            _rope1.color = _ropeColor1;
            _rope2.color = _ropeColor2;
            
            // Added for Playable Version2
            //
            _slingShot.color = _slingShotColor;
            _ground.color = _groundColor;
            _tree.color = _treeColor;
            _tunnel1.color = _tunnelColor1;
            _tunnel2.color = _tunnelColor2;
            _tunnel3.color = _tunnelColor3;
            _arrow.color = _arrowColor;

            //
        }
    }
}