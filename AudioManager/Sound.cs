// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using UnityEngine.Audio;       // Dont forget to import
using UnityEngine;

/// <summary>
/// Use this script with "AudioManager.cs"
///
/// If we want custom class to appear in the inspector we need to use "[System.Serializable]"
///
/// Ref : https://www.youtube.com/watch?v=6OT43pvUyfY 
/// </summary>

[System.Serializable] // To see in the inspector of 
public class Sound // MonoBehaviour removed
{
    public string name;
    
    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;

    [Range(-3f,3f)]
    public float pitch;

    public bool mute;
    
    public bool loop;

    public bool playOnAwake;
    
    
    
    [HideInInspector] public AudioSource source;


}
