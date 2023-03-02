// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace MousePosition___MouseClick.MouseFollowerTrail
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=aJ3UPkEQeiw&ab_channel=CocoCode
    /// </summary>
    [RequireComponent(typeof(TrailRenderer))]
    public class MouseFollowerTrail : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        private void Update()
        {
            _trailRenderer.enabled = Input.GetMouseButton(0);
            TrailFollowingMouse();
        }

        private void TrailFollowingMouse()
        {
            var screenPosition = Input.mousePosition;
            screenPosition.z = _mainCamera.nearClipPlane + 0.1f;
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(screenPosition);
            transform.position = mousePosition;
        }
    }
}
