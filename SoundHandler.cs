//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
//GEFEASOFT

/* Basic AudioHandler for Unity.
 * Plays correct or wrong audio
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] private AudioSource correct;
    [SerializeField] private AudioSource wrong;

    //Correct Sound 
    public void PlayCorrect()
    {
        correct.Play();
    }

    //Wrong Sound 
    public void PlayWrong()
    {
        wrong.Play();
    }
}
