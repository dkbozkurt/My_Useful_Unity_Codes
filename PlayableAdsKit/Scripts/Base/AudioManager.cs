using System;
using PlayableAdsKit.Scripts.Helpers;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Base
{
    [Serializable]
    public enum AudioName
    {
        Background,
        AudioName1,
        AudioName2,
        AudioName3
    }
    
    [Serializable]
    public struct Sound
    {
        public AudioName AudioName;
        public AudioSource AudioSource;
    }
    
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        protected override void OnAwake() { }
        
        [SerializeField] private Sound[] _sounds = new Sound[] {};

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        
        public void PlaySound(AudioName audioName)
        {
            FindSound(audioName).Play();
        }

        public void PauseSound(AudioName audioName)
        {
            FindSound(audioName).Pause();
        }
        
        public void StopSound(AudioName audioName)
        {
            FindSound(audioName).Stop();
        }

        private AudioSource FindSound(AudioName audioName)
        {
            for (int i = 0; i < _sounds.Length; i++)
            {
                if (_sounds[i].AudioName == audioName) return _sounds[i].AudioSource;
            }

            return null;
        }

    }
}