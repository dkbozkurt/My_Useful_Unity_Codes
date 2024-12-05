using System;
using System.Collections;
using System.Collections.Generic;
using CpiTemplate.Game.Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorController : MonoBehaviour
{
    public enum Theme
    {
        Default,
        Light,
        Dark
    }

    [SerializeField] private KeyboardController _keyboardController;
    [SerializeField] private WordDisplayController _wordDisplayController;
    
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject keyboardBackground;
    [SerializeField] private GameObject deleteButton;
    [SerializeField] private GameObject triangleParent;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject endcardBackground;

    [SerializeField] private GameObject[] lines;

    [LunaPlaygroundField("Theme Color", 0, "Color Settings")] [SerializeField]
    private Theme _theme;

    [LunaPlaygroundField("Correct Key Color", 1, "Color Settings")] [SerializeField]
    private Color _correctKey;

    [LunaPlaygroundField("Wrong Key Color", 2, "Color Settings")] [SerializeField]
    private Color _drawKey;

    [LunaPlaygroundField("End Card Background Color", 3, "Color Settings")] [SerializeField]
    private Color _endCardBg;

    private void Awake()
    {
        endcardBackground.GetComponent<Image>().color = _endCardBg;
        DecideTheme(_theme);
        setCorrectKeyColor(_correctKey);
        setDrawKeyColor(_drawKey);
    }

    private void DecideTheme(Theme theme)
    {
        switch (theme)
        {
            case Theme.Default:
                background.GetComponent<Image>().color = new Color(49f/255f, 48f/255f, 51f/255f,1);
                keyboardBackground.GetComponent<Image>().color = new Color(64f / 255f, 61f / 255f, 65f / 255f, 1);
                deleteButton.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1);
                _keyboardController._keyboardColors[0] = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1);
                _wordDisplayController._colors[3] = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1);
                title.GetComponent<TextMeshProUGUI>().color = Color.white;
                break;
            
            case Theme.Light:
                background.GetComponent<Image>().color = new Color(255f/255f, 255f/255f, 255f/255f,1);
                keyboardBackground.GetComponent<Image>().color = new Color(255f/255f, 255f/255f, 255f/255f,1);
                deleteButton.GetComponent<Image>().color =  new Color(217f / 255f, 217f / 255f, 217f / 255f, 1);
                _keyboardController._keyboardColors[0] = new Color(217f / 255f, 217f / 255f, 217f / 255f, 1);
                _wordDisplayController._colors[3] = new Color(217f / 255f, 217f / 255f, 217f / 255f, 1);
                title.GetComponent<TextMeshProUGUI>().color = Color.black;
                ChangeTriangleColor(Color.black);
                break;
            
            case Theme.Dark:
                background.GetComponent<Image>().color = new Color(20f/255f, 20f/255f, 20f/255f,1);
                keyboardBackground.GetComponent<Image>().color = new Color(64f / 255f, 61f / 255f, 65f / 255f, 1);
                deleteButton.GetComponent<Image>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f, 1);
                _keyboardController._keyboardColors[0] = new Color(200f / 255f, 200f / 255f, 200f / 255f, 1);
                _keyboardController._keyboardColors[3] = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1);
                _wordDisplayController._colors[3] = new Color(135f / 255f, 135f / 255f, 135f / 255f, 1);
                title.GetComponent<TextMeshProUGUI>().color = Color.white;
                ChangeTriangleColor(Color.white);
                break;
        }
    }

    private void setCorrectKeyColor(Color correctColor)
    {
        _keyboardController._keyboardColors[2] = correctColor;
        _wordDisplayController._colors[1] = correctColor;
    }

    private void setDrawKeyColor(Color drawColor)
    {
        _keyboardController._keyboardColors[1] = drawColor;
        _wordDisplayController._colors[2] = drawColor;
    }

    private void ChangeTriangleColor(Color TargetColor)
    {
        triangleParent.transform.GetChild(0).gameObject.GetComponent<Image>().color = TargetColor;
        
        triangleParent.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
        triangleParent.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        
        triangleParent.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().color = TargetColor;

        // for (int i = 0; i < lines.Length; i++)
        // {
        //     for (int j = 0; j < 5; j++)
        //     {
        //         lines[i].transform.GetChild(j).GetChild(1).GetComponent<TextMeshProUGUI>().color = TargetColor;    
        //     }
        // }
    }
}
