// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks; // Dont forget import !!!

/// <summary>
/// Basic async method example with await.
///
/// Call the async method from the button.
/// </summary>

public class AsyncMethodAndAwait : MonoBehaviour
{

    [SerializeField] private Button button;
    //[SerializeField] private Animation anim;
    private bool _marker;

    public async void AsyncDisableButton()
    {
        // anim.Play();
        // button.interactable = false;
        // while (anim.isPlaying == true)
        // {
        //     await Task.Yield();
        // }
        // button.interactable = true;
        
        _marker = false; 
        button.interactable = false;
        WaitForTime();
        while (_marker == false)
        {
            await Task.Yield();
        }

        button.interactable = true;
    }

    private void WaitForTime()
    {
        StartCoroutine(Do());
        
        IEnumerator Do()
        {
            yield return new WaitForSeconds(2f);
            _marker = true;
        }
    }
    
}
