// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this script into a gameObject.
/// Then reference the scriptable object from the project window.
/// 
/// Ref : https://www.youtube.com/watch?v=aPXvoWVabPY&ab_channel=Brackeys
/// </summary>

public class ScriptableCardDisplay : MonoBehaviour
{
    // Reference the scriptable object from the project window
    [SerializeField] private ScriptableCard _scriptableCard;

    [Header("ObjectVariables")]
    [SerializeField] private Text nameText = null;
    [SerializeField] private Text descriptionText= null;
    [SerializeField] private Image artworkImage= null;
    [SerializeField] private Text manaText= null;
    [SerializeField] private Text attackText= null;
    [SerializeField] private Text healthText= null;

    private void Start()
    {
        _scriptableCard.Print();
        
        // We can also assign properties of an object from scriptable object variables
        AssignValues();
    }

    private void AssignValues()
    {
        nameText.text = _scriptableCard.name;
        descriptionText.text = _scriptableCard.description;
        artworkImage.sprite = _scriptableCard.artwork;
        manaText.text = _scriptableCard.manaCost.ToString();
        attackText.text = _scriptableCard.attack.ToString();
        healthText.text = _scriptableCard.health.ToString();
    }
    
    // Additional information: Instantiating a scriptable object

    // [SerializeField] private ScriptableCard _scriptableCard2;
    // private void Start()
    // {
    //     _scriptableCard2 = (ScriptableCard)ScriptableObject.CreateInstance(typeof(ScriptableCard));
    //     Debug.Log("Health: " + _scriptableCard2.health);
    //     // VERY IMPORTANT, DONT DESTROY THE SCRIPTABLE OBJECT, YOU WILL USE THAT SCRIPTABLE OBJECT WITH ITS VALUES.
    //     ScriptableObject.Destroy(_scriptableCard2,4f);
    // }
}
