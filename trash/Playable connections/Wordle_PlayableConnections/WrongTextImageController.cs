using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class WrongTextImageController : MonoBehaviour
{
    [SerializeField] private GameObject textImageParent;
    [SerializeField] private GameObject[] textImages;

    [LunaPlaygroundField("Wrong Text Images", 0, "Pop Up Settings")] [SerializeField]
    private bool textImagesOn;
    
    private int textImageIndex=0;
    private bool firstWrongImageCall=true;
    private Animator _textImageAnimator;
    private void Awake()
    {
        textImageParent.SetActive(textImagesOn);
        _textImageAnimator = textImageParent.GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (firstWrongImageCall && Input.GetMouseButtonDown(0))
        {
            firstWrongImageCall = false;
        }
    }
    
    public void GetRandomWrongImage()
    {
        if (!firstWrongImageCall)
        {
            for (int i = 0; i < textImages.Length ; i++)
            {
                textImages[i].SetActive(false);
            }
            textImages[textImageIndex].SetActive(true);
            textImageIndex++;
            if (textImageIndex > textImages.Length-2) textImageIndex = 2;
            WrongImageAnimationTrigger();
        }
        
    }

    private void WrongImageAnimationTrigger()
    {
        _textImageAnimator.SetTrigger("wrong");
    }
}
