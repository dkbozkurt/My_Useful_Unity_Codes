using TMPro;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class BasicTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        private float _time = 0f;
        private bool _isTimerOn = false;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void Update()
        {
            if(!_isTimerOn) return;

            _time += Time.deltaTime;
            UpdateTimer(_time);
        }

        
        public void ToggleTimer()
        {
            _isTimerOn = !_isTimerOn;
        }
        
        public void ResetTimer()
        {
            _time = -1f;
        }

        private void UpdateTimer(float currentTime)
        {
            currentTime +=1;

            float minutes = Mathf.FloorToInt(currentTime / 60);
            float seconds = Mathf.FloorToInt(currentTime % 60);
            
            SetTimerText(minutes,seconds);
        }

        private void SetTimerText(float mins,float secs)
        {
            _timerText.text = string.Format("{0:00} : {1:00}", mins, secs);
        }

    }
}
