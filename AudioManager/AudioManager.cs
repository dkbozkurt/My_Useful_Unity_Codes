// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine.Audio;    // Dont forget to import
using UnityEngine;

/// <summary>
/// Attach this script into a empty GameObject.
///
/// Call in the other scripts by; AudioManager.Play("SoundName")
/// 
/// Use this script with "Sound.cs"
///
/// Ref : https://www.youtube.com/watch?v=6OT43pvUyfY 
/// </summary>

public class AudioManager : MonoBehaviour
{

    [SerializeField] private Sound[] sounds;

    public static AudioManager instance; // We want to make sure there is only one instance

    private void Awake()
    {
        if (instance == null) // Meaning that we dont have an AudioManager in our scene.
            instance = this;
        else // Meaning that we already have an instance in our scene, we want to remove this object.
        {
            Destroy(gameObject);
            return; // To make sure that no more code is called before we destroy the object.
        }
        
        DontDestroyOnLoad(gameObject); // We don't want to destroy our sound when transitioning between scenes
        SetAudios();
    }

    private void SetAudios()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.mute = s.mute;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " no found!");
            return;
        }
        s.source.Play();
    }
}
