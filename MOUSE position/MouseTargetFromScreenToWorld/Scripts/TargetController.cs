// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace MousePosition___MouseClick.MouseTargetFromScreenToWorld.Scripts
{
    /// <summary>
    /// How to Find Mouse position from screen to world in 3D with offset
    ///
    /// Access Set method of this script and can call the crossHair wherever you want.
    /// 
    /// Note : GOTTA BE LOOKING AT Z DIRECTION !!!! (In other words working on plane)
    /// 
    /// Ref : https://www.youtube.com/watch?v=5NTmxDSKj-Q&ab_channel=GameDevBeginner
    /// </summary>
    public class TargetController : MonoBehaviour
    {
        [SerializeField] private GameObject _crossHair;
        [SerializeField] private Vector2 _crossHairShiftingOffSet = new Vector2(0f, 0f);
        [SerializeField] private bool _useCameraNearClipDepth = false;
        
        /// <summary>
        /// If camera near clip depth didn't selected, this value will shift Z value of the crossHair.
        /// </summary>
        [SerializeField] private float _crossHairDepthValueOnZPlane;
        
        [Space]
        [Header("CrossHair Clamp Limits")] 
        [SerializeField] private Vector2 _xWorldPositionClampLimits = new Vector2(-99f, 99f);
        [SerializeField] private Vector2 _yWorldPositionClampLimits = new Vector2(-99f, 99f);

        /// <summary>
        /// If false: access set method of this script and can call the crossHair wherever you want.
        /// </summary>
        [SerializeField] private bool _activateWithMouseDown = true;
        
        private Vector3 _initialCrossHairLocation;
        private Camera _mainCamera;

        private float _crossHairDepthRespectToCamera = 0f;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _initialCrossHairLocation = _crossHair.transform.position;
            _crossHairDepthRespectToCamera = _crossHairDepthValueOnZPlane - _mainCamera.transform.position.z;
        }

        private void Start()
        {
            CrossHairSetter(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CrossHairSetter(true);       
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                CrossHairSetter(false);       
            }
            
            if(!IsCrossHairEnable()) return;

            if (Input.GetMouseButton(0))
            {
                GetDeltaMousePosition();    
            }
        }

        public void CrossHairSetter(bool status)
        {
            _crossHair.transform.position = _initialCrossHairLocation;
            _crossHair.SetActive(status);
        }
        
        private bool IsCrossHairEnable()
        {
            return _crossHair.activeInHierarchy;
        }
        
        public Vector3 GetCrossHairWorldPosition()
        {
            return _crossHair.transform.position;
        }

        private void GetDeltaMousePosition()
        {
            var screenPosition = Input.mousePosition;

            if (_useCameraNearClipDepth)
            {
                screenPosition.z = _mainCamera.nearClipPlane + _crossHair.transform.localScale.z;
            }
            else
            {
                screenPosition.z = _crossHairDepthRespectToCamera;
            }
            
            var worldPosition = _mainCamera.ScreenToWorldPoint(screenPosition);
            
            MoveCrossHair(worldPosition,_crossHairShiftingOffSet);
        }

        private void MoveCrossHair(Vector3 targetPosition,Vector2 offset)
        {
            var targetPositionX = Mathf.Clamp(targetPosition.x + offset.x,_xWorldPositionClampLimits.x,_xWorldPositionClampLimits.y); 
            var targetPositionY = Mathf.Clamp(targetPosition.y + offset.y,_yWorldPositionClampLimits.x,_yWorldPositionClampLimits.y);

            _crossHair.transform.position = new Vector3(targetPositionX, targetPositionY, targetPosition.z);
        }
    }
}
