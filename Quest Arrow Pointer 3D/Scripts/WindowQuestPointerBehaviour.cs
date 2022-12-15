// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace Quest_Arrow_Pointer_3D.Scripts
{
    /// <summary>
    /// How to use :
    ///     Canvas
    ///         WindowQuestPointer (with WindowQuestPointerBehaviour.cs)
    ///             PointerBackgroundImage
    ///                 PointerIcon
    ///                 ArrowParent
    ///                     ArrowImage
    ///
    /// Important note : ArrowImage's pointing point must look at to +X axis on UI. (0 degree in real world) 
    /// 
    /// Ref : https://www.youtube.com/watch?v=dHzeHh-3bp4&ab_channel=CodeMonkey
    /// </summary>
    public class WindowQuestPointerBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _pointerRectTransform;
        [SerializeField] private RectTransform _rotatingArrowRectTransform;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Transform _playerTransform;

        [Header("Variables")]
        public bool CanShowTarget;
        [SerializeField] private float _borderSpacingToDetectOffScreen = 0f;
        [SerializeField] private float _borderSpacingMultiplierForWidth = 0.15f;
        [SerializeField] private float _borderSpacingMultiplierForHeight = 0.1f;

        [Space]
        [SerializeField] private bool _locateCrossIfTargetInScreen = true;
        [SerializeField] private RectTransform _crossRectTransform;

        [SerializeField] private float _deactivateCrossDistance=3f;
        private float _borderSizeWidth => Screen.width * _borderSpacingMultiplierForWidth;
        private float _borderSizeHeight => Screen.height * _borderSpacingMultiplierForHeight;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            CanShowTarget = true;
            if(!_locateCrossIfTargetInScreen) _crossRectTransform.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            // PlayerInteractionManager.OnPlayerGetCargo += (() => CanShowTarget = false);
            // PlayerInteractionManager.OnHouseObjectPlaced += (() => CanShowTarget = true);
        }

        private void OnDisable()
        {
            // PlayerInteractionManager.OnPlayerGetCargo -= (() => CanShowTarget = false);
            // PlayerInteractionManager.OnHouseObjectPlaced -= (() => CanShowTarget = true);
        }

        private void Update()
        {
            if (!CanShowTarget) return;
            PointTarget();
        }
        
        public void SetTargetTransform(Transform target)
        {
            _targetTransform = target;
        }
        
        private void PointTarget()
        {
            float borderSizeWidth = _borderSizeWidth;
            float borderSizeHeight = _borderSizeHeight;
            Vector3 targetPositionScreenPoint = _camera.WorldToScreenPoint(_targetTransform.position);
            bool isOffScreen = targetPositionScreenPoint.x <= _borderSpacingToDetectOffScreen ||
                               targetPositionScreenPoint.x >= Screen.width - _borderSpacingToDetectOffScreen ||
                               targetPositionScreenPoint.y <= _borderSpacingToDetectOffScreen ||
                               targetPositionScreenPoint.y >= Screen.height - _borderSpacingToDetectOffScreen;

            RotateArrow(isOffScreen);
            if (_locateCrossIfTargetInScreen)
            {
                _crossRectTransform.gameObject.SetActive(!isOffScreen);
            }

            _pointerRectTransform.gameObject.SetActive(isOffScreen);
            
            // Pointer will point the target object.
            if (isOffScreen)
            {
                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
                if (cappedTargetScreenPosition.x <= borderSizeWidth) cappedTargetScreenPosition.x = borderSizeWidth;
                if (cappedTargetScreenPosition.x >= Screen.width - borderSizeWidth)
                    cappedTargetScreenPosition.x = Screen.width - borderSizeWidth;
                if (cappedTargetScreenPosition.y <= borderSizeHeight) cappedTargetScreenPosition.y = borderSizeHeight;
                if (cappedTargetScreenPosition.y >= Screen.height - borderSizeHeight)
                    cappedTargetScreenPosition.y = Screen.height - borderSizeHeight;

                _pointerRectTransform.position = cappedTargetScreenPosition;
                _pointerRectTransform.localPosition = new Vector3(_pointerRectTransform.localPosition.x,
                    _pointerRectTransform.localPosition.y, 0f);
            }
            // Cross will locate itself on the target gameobject.
            else
            {
                CloseCrossIfTargetNearBy();
                
                _crossRectTransform.position = targetPositionScreenPoint;
                _crossRectTransform.localPosition = new Vector3(_crossRectTransform.localPosition.x,
                    _crossRectTransform.localPosition.y, 0f);
            }
        }
        
        private void RotateArrow(bool status)
        {
            _rotatingArrowRectTransform.gameObject.SetActive(status);
            if (!status) return;

            Vector3 toPosition = _targetTransform.position;
            Vector3 fromPosition = _playerTransform.position;
            
            toPosition.y = 0f;
            fromPosition.y = 0f;
            Vector3 dir = (toPosition - fromPosition).normalized;
            // Debug.Log("Dir : " + dir);
            float angle = GetAngleFromVectorFloat(dir);
            // Debug.Log("Angle : " + angle);
            _rotatingArrowRectTransform.eulerAngles = new Vector3(0, 0, angle);

        }
        
        private void CloseCrossIfTargetNearBy()
        {
            if(!_locateCrossIfTargetInScreen) return;
            _crossRectTransform.gameObject.SetActive(_deactivateCrossDistance <= Vector3.Distance(_playerTransform.position, _targetTransform.position));

        }

        private float GetAngleFromVectorFloat(Vector3 dir)
        {
            float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }
    }
}
