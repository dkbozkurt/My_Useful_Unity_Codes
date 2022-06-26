using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource keySound;
    [SerializeField] private AudioSource enterSound;
    [SerializeField] private AudioSource correctSound;
    [SerializeField] private AudioSource[] wrongAudioSources;

    [LunaPlaygroundField("Voice Over Audios", 0, "Sound Settings")] 
    [SerializeField] private bool openAudios;

    private bool firstSoundCall =true;
    private int voiceIndex=0;
    
    private void Awake()
    {
        VoiceOverSetting(openAudios);
    }

    private void Update()
    {
        if (firstSoundCall && Input.GetMouseButtonDown(0))
        {
            firstSoundCall = false;
        }
    }

    public void PlayKeySound()
    {
        keySound.Play();
    }
    
    public void EnterKeySound()
    {
        enterSound.Play();
    }

    public void WrongSound()
    {
        StartCoroutine(Do());
        IEnumerator Do()
        {
            yield return new WaitForSeconds(0.3f);
            
            if (!firstSoundCall)
            {
                wrongAudioSources[voiceIndex].Play();
                voiceIndex++;
                if (voiceIndex > wrongAudioSources.Length-1) voiceIndex = 2;
            }
        }
        
    }

    public void CorrectSound()
    {
        correctSound.Play();   
    }

    private void VoiceOverSetting(bool status)
    {
        correctSound.enabled = status;

        foreach (AudioSource child in wrongAudioSources)
        {
            child.enabled = status;
        }
    }
    
}
