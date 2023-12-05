using System;
using PlayableAdsKit.Scripts.Utilities;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Base
{
    public class MuteButtonBase : MonoBehaviour
    {
        [SerializeField] private GameObject _soundOnImage;
        [SerializeField] private GameObject _soundOffImage;

        [SerializeField] private bool _startMuted = false;

        private bool _isMuted = false;

        private AudioListener _audioListener;
        
        private void Start()
        {
            _audioListener = PlyAdsKitUtils.Camera.GetComponent<AudioListener>();
            
            _isMuted = _startMuted;
            SetStatus();
        }

        public void Toggle()
        {
            _isMuted = !_isMuted;
            
            SetStatus();
        }

        private void SetStatus()
        {
            _soundOnImage.SetActive(!_isMuted);
            _soundOffImage.SetActive(_isMuted);

            _audioListener.enabled = !_isMuted;
        }
    }
}
