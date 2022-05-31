// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using TMPro;
using UnityEngine;
namespace CpiTemplate.Game.Scripts.Behaviours.UI
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timer;

        private float timer;
        
        private bool _timerStart;
        
        public void Initialize()
        {
            timer = 0;
            _timerStart = true;
        }
        
        public void Update()
        {
            if (!_timerStart) return;

            _timer.text = ((int)(timer / 60)).ToString() + ":" + ((int)(timer % 60)).ToString("00");
            timer += Time.deltaTime;
        }

        public void Stop()
        {
            _timerStart = false;
        }
    }
}
