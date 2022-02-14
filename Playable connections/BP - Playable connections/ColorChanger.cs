using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Playable.Scripts.PreScripts
{
    public class ColorChanger : MonoBehaviour
    {
        [LunaPlaygroundField("Platfrom Color", 0, "Color settings")] [SerializeField]
        private Color _platformColor;

        [LunaPlaygroundField("BallTrail Color", 1, "Color settings")] [SerializeField]
        private Color _ballTrailColor;

        [LunaPlaygroundField("Background Color", 2, "Color settings")] [SerializeField]
        private Color _backgroundColor;

        [LunaPlaygroundField("Text color", 3, "Color settings")] [SerializeField]
        private Color _textColor;

        [SerializeField] private Material _platfromMaterial;
        [SerializeField] private Material _ballTrail;
        [SerializeField] private Camera camera;
        [SerializeField] private Text text;

        private void Awake()
        {
            _platfromMaterial.color = _platformColor;
            _ballTrail.color = _ballTrailColor;
            camera.backgroundColor = _backgroundColor;
            text.color = _textColor;

        }
    }
}