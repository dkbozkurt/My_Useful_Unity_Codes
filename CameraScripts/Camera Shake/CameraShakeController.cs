// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace CameraScripts.Camera_Shake
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=JGTgLvh_DH4&ab_channel=SpeedTutor
    /// </summary>
    public class CameraShakeController : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineBasicMultiChannelPerlin _perlinNoise;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _perlinNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            ResetIntensity();
        }

        public void ShakeCamera(float shakeIntensity,float shakeDuration)
        {
            _perlinNoise.m_AmplitudeGain = shakeIntensity;
            StartCoroutine(WaitTimeCoroutine(shakeDuration));
        }
        
        public IEnumerator WaitTimeCoroutine(float shakeDuration)
        {
            yield return new WaitForSeconds(shakeDuration);
            ResetIntensity();
        }

        private void ResetIntensity()
        {
            _perlinNoise.m_AmplitudeGain = 0f;
        }
    }
}
