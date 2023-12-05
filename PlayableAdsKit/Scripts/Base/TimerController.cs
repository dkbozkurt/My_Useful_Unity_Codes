using System;
using PlayableAdsKit.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Base
{
	public class TimerController : SingletonBehaviour<TimerController>
    {
        public static event Action OnTimerEnded;
        
#if UNITY_LUNA
		[LunaPlaygroundField("Game Duration In Seconds",0,"Game Settings")]
#endif
        [SerializeField] private float _timeLimitInSeconds = 15f;
        
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private bool _isCountUpTimer = false;
        
        private float _time = 0f;
        private bool _isTimerOn = false;
        
        private void Start()
        {
            _time = _isCountUpTimer ? 0f : _timeLimitInSeconds;
            UpdateTimer(_time);
        }
        
        private void Update()
        {
            if(!_isTimerOn) return;
            
            _time += Time.deltaTime * (_isCountUpTimer? 1 : -1);
            UpdateTimer(_time);
        }
        
        public void StartTimer()
        {
            _isTimerOn = true;
        }
        
        public void StopTimer()
        {
            _isTimerOn = false;
        }

        public void ToggleTimer()
        {
            _isTimerOn = !_isTimerOn;
        }

        public void SetTimer(float targetTime)
        {
            _time = targetTime;
            SetTimerText(_time);
        }
        
        public void ResetTimer()
        {
            if (_isCountUpTimer)
                _time = 0f;
            else
                _time = _timeLimitInSeconds;
        }

        private void UpdateTimer(float currentTime)
        {
            bool countUpCondition = currentTime >= _timeLimitInSeconds;
            bool countDownCondition = currentTime <= 0;

            if (_isCountUpTimer ? countUpCondition : countDownCondition)
            {
                _isTimerOn = false;
                // EndCardController.Instance.OpenEndCard();
                OnTimerEnded?.Invoke();
                return;
            }
            
            float minutes = Mathf.FloorToInt(currentTime / 60);
            float seconds = Mathf.FloorToInt(currentTime % 60);

            string targetMinString = minutes > 0 ? minutes.ToString()+" : " : ""; 
            string timerText = targetMinString + seconds.ToString("00");
            SetTimerText(timerText);
        }

        private void SetTimerText(float secs)
        {
            SetTimerText(secs.ToString());
        }

        private void SetTimerText(string time)
        {
            _timerText.text = time;
        }

        protected override void OnAwake() { }
    }
}