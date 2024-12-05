using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject tutorialObject;
    private bool tutorialOn;

    [LunaPlaygroundField("Tutorial Text", 0, "Tutorial Settings")] [SerializeField]
    private string tutorialText;
    
    void Start()
    {
        tutorialOn = true;
        tutorialObject.SetActive(true);
        SetTutorialText();
    }

    private void Update()
    {
        if (tutorialOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                tutorialObject.SetActive(false);
                tutorialOn = false;
            }
        }
    }

    private void SetTutorialText()
    {
        tutorialObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = tutorialText;
    }
}
