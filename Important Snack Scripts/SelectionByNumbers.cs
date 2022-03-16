// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Transactions;
using PaintIn3D;
using UnityEditor;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Selecting the child object depends on the entered number from the user. 
/// </summary>

public class SelectionByNumbers : MonoBehaviour
{
    private List<GameObject> tools = new List<GameObject>();

    private string tempString;
    private int selectedToolNumber=0;
    
    [SerializeField]private MouseObjectController _mouseObjectController;
    private void Awake()
    {
        GetTools();
        ToolsOnOff(false);
        //tools[0].SetActive(true);
    }

    private void Update()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey))
            {
                // Debug.Log("Key pressed:"+vKey);
                ToolChanger(vKey);
                
            }    
        }

    }
    
    private void  GetTools()
    {
        foreach (Transform child in transform)
        {
            tools.Add(child.gameObject);
        }
        
    }
    private void ToolsOnOff(bool statue)
    {
        for (int i = 0; i < tools.Count; i++)
        {
            tools[i].SetActive(statue);
        }
    }

    private void ToolChanger(KeyCode vKey)
    {
        ConvertKeyCodeToString(vKey);

        if (CheckIfNumber())
        {
            ToolsOnOff(false);
            tools[selectedToolNumber].SetActive(true);
            Debug.Log(tools[selectedToolNumber].name+" is selected.");
            
            _mouseObjectController.ObjectSelector();
            
        }
    }

    private bool CheckIfNumber()
    {
        if (tempString.Substring(0, 5) == "Alpha")
        {
            selectedToolNumber = (int)Char.GetNumericValue(tempString[5]);
            // Debug.Log("Alpha :"+selectedToolNumber);
            return true;
        }
        if (tempString.Substring(0,6) =="Keypad")
        {
            selectedToolNumber = (int)Char.GetNumericValue(tempString[6]);
            // Debug.Log("Keypad :"+selectedToolNumber);
            return true;
        }

        return false;
    }
    

    private void ConvertKeyCodeToString(KeyCode vKey)
    {
        tempString = vKey.ToString();
    }

}
