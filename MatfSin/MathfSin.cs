// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
using System;
using UnityEngine;

namespace MatfSin
{
    /// <summary>
    /// https://www.youtube.com/watch?v=7JcmpBUx4ag&ab_channel=Tarodev
    /// </summary>
    public class MathfSin : MonoBehaviour
    {
        [SerializeField] private float _startOffSet = 0;
        [SerializeField] private float _waveAmplitude = 2f;
        [SerializeField] private float _waveFrequency = 1f;

        private float _initialYValue;
        
        private void Start()
        {
            _initialYValue = transform.position.y;
        }

        private void Update()
        {
            SinWaveMove();
        }

        private void SinWaveMove()
        {
            var targetValue = Mathf.Sin(_startOffSet + _waveFrequency * Time.time) * _waveAmplitude;

            transform.position = new Vector3(transform.position.x,
                _initialYValue + targetValue, transform.position.z);
        }
    }
}
