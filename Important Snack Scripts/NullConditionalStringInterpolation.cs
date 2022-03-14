// Dogukan Kaan Bozkurt
//          github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

/// <summary>
/// Null Conditional & String interpolation
///
/// Null Conditional can be used like; x?.method, depends on the x's null condition related method will work or not.
/// 
/// By using '$' sign at the beginning of quotes it helps to replate values inside of '{ }' and it called string interpolation.
/// 
/// Ref: https://www.youtube.com/watch?v=KRq0-0KY6bU&ab_channel=JasonWeimann
/// </summary>

public class NullConditionalStringInterpolation : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private int _totalSpawned;

    private void Print()
    {
        // Instead of using this, use the following version
        // if (_text != null)
        // {
        //     _text.SetText($"Spawned a total of {_totalSpawned}");
        // }
        // else if (_text == null)
        // {
        //     // empty   
        // }
        
        // Null Conditional & String interpolation
        _text?.SetText($"Spawned a total of {_totalSpawned}");
        
    }
}
