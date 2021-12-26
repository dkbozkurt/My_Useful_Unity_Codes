//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

// Animation Controls of the main game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    #region Animators
    [SerializeField] private Animator whiteBoard;
    [SerializeField] private Animator humanBody;
    [SerializeField] private Animator doctor;
    #endregion

    #region Doctor sprite animations
    [SerializeField] private GameObject idle;
    [SerializeField] private GameObject thinking;    
    [SerializeField] private GameObject correct;    
    [SerializeField] private GameObject wrong;    
    #endregion

    public void whiteBoardZoom(bool zoom)
    {
        whiteBoard.SetBool("Zoom", zoom); 
    }

    public void HumanBodyInOut(bool inOut)
    {
        humanBody.SetBool("in", inOut);
    }

    public void DoctorInOut(bool inOut)
    {
        doctor.SetBool("in", inOut);
    }

    public void DoctorSpriteController(int i)
    {
        switch (i)
        {
            case 0:
                idle.SetActive(true);
                thinking.SetActive(false);
                correct.SetActive(false);
                wrong.SetActive(false);
                break;
            case 1:
                idle.SetActive(false);
                thinking.SetActive(true);
                correct.SetActive(false);
                wrong.SetActive(false);
                break;
            case 2:
                idle.SetActive(false);
                thinking.SetActive(false);
                correct.SetActive(true);
                wrong.SetActive(false);
                break;
            case 3:
                idle.SetActive(false);
                thinking.SetActive(false);
                correct.SetActive(false);
                wrong.SetActive(true);
                break;
            default:
                idle.SetActive(false);
                thinking.SetActive(false);
                correct.SetActive(false);
                wrong.SetActive(false);

                break;
        }
            
    }


}
