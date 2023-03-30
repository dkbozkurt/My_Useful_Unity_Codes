// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Grid_System.Grid_System_Tarodev.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=kkAjpQAM-jE&ab_channel=Tarodev
    /// </summary>
    public class GridSystem_Tile : MonoBehaviour
    {
        [SerializeField] private Color _baseColor;
        [SerializeField] private Color _offsetColor;
        [SerializeField] private GameObject _highlight;

        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void Init(bool isOffset) {
            _renderer.color = isOffset ? _offsetColor : _baseColor;
        }
 
        void OnMouseEnter() {
            _highlight.SetActive(true);
        }

        private void OnMouseDown()
        {
            Debug.Log("Grid name: " + gameObject.name);
        }

        void OnMouseExit()
        {
            _highlight.SetActive(false);
        }
    
    }
}