// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace WorldSpaceToUI.Scripts
{
    /// <summary>
    /// Ref : https://forum.unity.com/threads/world-to-screen-point-with-canvas-set-to-scale-with-screen-size.853078/
    /// </summary>
    public class WorldSpaceToUI : MonoBehaviour
    {
        [SerializeField] private Transform _sourceObject;

        [SerializeField] private Transform _targetUI;

        private Camera _mainCam;
        private void Start()
        {
            _mainCam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ExecuteWorldSpaceToUI();                
            }
        }

        private void ExecuteWorldSpaceToUI()
        {
            Vector3 pos = _mainCam.WorldToScreenPoint(_sourceObject.transform.position);
            _targetUI.position = pos;
        }
    }
}
