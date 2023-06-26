using CpiTemplate.Game.Playable.Scripts.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CpiTemplate.Game.Playable.Scripts.Managers
{
    public class JoystickManager : SingletonBehaviour<JoystickManager>,IDragHandler, IPointerDownHandler,IPointerUpHandler
    {
        public RectTransform joystickBackground;
        public RectTransform joystickHandle;
    
        private Vector2 _input = Vector2.zero;
        private Vector2 _joyPosition = Vector2.zero;

        private float _thresholdValue = 0.2f;
    
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 joyDirection = eventData.position - _joyPosition;
            _input = (joyDirection.magnitude > joystickBackground.sizeDelta.x / 2f)
                ? joyDirection.normalized
                : joyDirection / (joystickBackground.sizeDelta.x / 2f);
            joystickHandle.anchoredPosition = (_input * joystickBackground.sizeDelta.x / 2f) * 1f;

        }
        public void OnPointerDown(PointerEventData eventData)
        {
            joystickBackground.gameObject.SetActive(true);
            _input=Vector2.zero;
            //OnDrag(eventData);
            _joyPosition = eventData.position;
            joystickBackground.position = eventData.position;
            joystickHandle.anchoredPosition = Vector2.zero;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            joystickBackground.gameObject.SetActive(false);
            _input=Vector2.zero;
            joystickHandle.anchoredPosition = Vector2.zero;
        }
    

        public float InputHorizontal()
        {
        
            if (_input.x != 0)
            {
                if (_input.x > _thresholdValue || _input.x < -_thresholdValue) return _input.x;
                else return 0f;
            }
            else
            {
                _input.x = 0f;
                return Input.GetAxis("Horizontal");
            }
            
        }

        public float InputVertical()
        {
        
            if (_input.y!= 0)
            {
                if (_input.y > _thresholdValue || _input.y < -_thresholdValue) return _input.y;
                else return 0f;
            }
            else
            {
                _input.y = 0f;
                return Input.GetAxis("Vertical");
            }
        }

        protected override void OnAwake()
        {
        }
    }
}
