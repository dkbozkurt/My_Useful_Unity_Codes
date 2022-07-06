// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zoom.Pinch_Zoom_and_Panning_on_Mobile
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=0G4vcH9N0gc&ab_channel=Waldo
    /// </summary>
    public class PanZoomMobile : MonoBehaviour
    {
        [SerializeField] private float zoomMinValue = 1;
        [SerializeField] private float zoomMaxValue = 8;
        [SerializeField] private float zoomSensitivity = 0.01f;
        private Vector3 _touchStart;
        private Vector2 _touchZeroPreviousPosition;
        private Vector2 _touchOnePreviousPosition;

        private float _prevMagnitude;
        private float _currentMagnitude;
        private float _difference;

        private void Update()
        {
            CheckIfZoom();
        }

        private void CheckIfZoom()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                _touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
                _touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;

                _prevMagnitude = (_touchZeroPreviousPosition - _touchOnePreviousPosition).magnitude;
                _currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                _difference = _currentMagnitude - _prevMagnitude;

                Zoom(_difference * zoomSensitivity);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector3 direction = _touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Camera.main.transform.position += direction;
            }
        }

        private void Zoom(float increment)
        {
            Camera.main.orthographicSize =
                Mathf.Clamp(Camera.main.orthographicSize - increment, zoomMinValue, zoomMaxValue);
        }
    }
}
