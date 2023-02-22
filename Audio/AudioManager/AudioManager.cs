// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using CpiTemplate.Game.Scripts;
using UnityEngine;

namespace Audio.AudioManager
{
    [Serializable]
    public enum AudioName
    {
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
    
    /// <summary>
    /// 
    /// </summary>
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        protected override void OnAwake() { }
        
        [SerializeField] private Sound[] _sounds = new Sound[] {};
        
        public void PlaySound(AudioName audioName)
        {
            for (int i = 0; i < _sounds.Length; i++)
            {
                if(_sounds[i].AudioName == audioName) _sounds[i].AudioSource.Play();
            }
        }
    }
}
