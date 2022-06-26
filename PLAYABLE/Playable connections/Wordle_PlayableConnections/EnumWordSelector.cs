using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CpiTemplate.Game.Scripts.Behaviours;
using CpiTemplate.Game.Scripts.Controllers;
using CpiTemplate.Game.Scripts.Models;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnumWordSelector : MonoBehaviour
{
    public enum WordToSelect
    {
        BOOTY,
        DRINK,
        TRUMP,
        TWERK,
        CRANK,
        WIXEN,
        SWILL,
        ULTRA,
        FRAME,
        DRUNK,
        COULD
    }

    [SerializeField] private WordsData _wordsData;
    [SerializeField] private WordDisplayController _wordDisplayController;
    [SerializeField] private KeyboardController _keyboardController;
    [SerializeField] private GameObject[] firstLetterBoxes;

    [SerializeField] private GameObject[] secondLetterBoxes;

    //[SerializeField] private GameObject[] keyboardButtons;
    private string correctWord;


    [LunaPlaygroundField("Word to select", 0, "Correct Word Settings")] [SerializeField]
    private WordToSelect _wordToSelect;

    private List<int> textColorList = new List<int>();
    private void OnEnable()
    {
        correctWord = _wordToSelect.ToString().ToLower();
        setAssignedWord();
    }

    private void Start()
    {
        SetGameComponents();
        textColorList.Clear();
    }

    private void setAssignedWord()
    {
        correctWord = correctWord.ToLower();
        _wordsData.WordList.Add(correctWord);
        _wordsData.selectedWord = correctWord;
        _wordsData.TestThisWord();
    }

    private void SetGameComponents()
    {
        switch (_wordToSelect)
        {
            case WordToSelect.BOOTY:
                BoxTextSetter(firstLetterBoxes, "boost");
                BoxColorSetter(firstLetterBoxes, "11102");
                
                textColorList.AddRange(new int[]{1,1,1,0,2});
                BoxTextColorSetter(firstLetterBoxes,textColorList);
                
                KeyboardColorSetter("boost", "22231");
                BoxTextSetter(secondLetterBoxes, "empty");
                KeyboardColorSetter("empty", "33322");
                EnterLastLine("empty");

                break;

            case WordToSelect.DRINK:
                BoxTextSetter(firstLetterBoxes, "dreck");
                BoxColorSetter(firstLetterBoxes, "11001");
                
                textColorList.AddRange(new int[]{1,1,0,0,1});
                BoxTextColorSetter(firstLetterBoxes,textColorList);
                
                KeyboardColorSetter("dreck", "22332");
                BoxTextSetter(secondLetterBoxes, "drunk");
                KeyboardColorSetter("drunk", "22322");
                EnterLastLine("drunk");

                break;

            case WordToSelect.TRUMP:
                BoxTextSetter(firstLetterBoxes, "stamp");
                BoxColorSetter(firstLetterBoxes, "02011");
                
                textColorList.AddRange(new int[]{0,2,0,1,1});
                BoxTextColorSetter(firstLetterBoxes,textColorList);
                
                KeyboardColorSetter("stamp", "31322");
                BoxTextSetter(secondLetterBoxes, "track");
                KeyboardColorSetter("track", "22333");
                EnterLastLine("track");

                break;

            case WordToSelect.TWERK:
                BoxTextSetter(firstLetterBoxes, "twirl");
                BoxColorSetter(firstLetterBoxes, "11010");
                
                textColorList.AddRange(new int[]{1,1,0,1,0});
                BoxTextColorSetter(firstLetterBoxes,textColorList);
                
                KeyboardColorSetter("twirl", "22323");
                BoxTextSetter(secondLetterBoxes, "cheek");
                KeyboardColorSetter("cheek", "33222");
                EnterLastLine("cheek");

                break;

            case WordToSelect.CRANK:
                BoxTextSetter(firstLetterBoxes, "crack");
                BoxColorSetter(firstLetterBoxes, "11101");
                
                textColorList.AddRange(new int[]{1,1,1,0,1});
                BoxTextColorSetter(firstLetterBoxes,textColorList);
                
                KeyboardColorSetter("crack", "22232");
                BoxTextSetter(secondLetterBoxes, "crunk");
                KeyboardColorSetter("crunk", "22322");
                EnterLastLine("crunk");

                break;

            case WordToSelect.WIXEN:
                BoxTextSetter(firstLetterBoxes, "piked");
                BoxColorSetter(firstLetterBoxes, "01010");
                
                textColorList.AddRange(new int[]{0,1,0,1,0});
                BoxTextColorSetter(firstLetterBoxes,textColorList);
                
                KeyboardColorSetter("piked", "32323");
                BoxTextSetter(secondLetterBoxes, "fixes");
                KeyboardColorSetter("fixes", "32223");
                EnterLastLine("fixes");

                break;
            case WordToSelect.SWILL:
                BoxTextSetter(firstLetterBoxes, "scold");
                BoxColorSetter(firstLetterBoxes, "10010");
                
                textColorList.AddRange(new int[]{1,0,0,1,0});
                BoxTextColorSetter(firstLetterBoxes,textColorList);
                
                KeyboardColorSetter("scold", "23323");
                BoxTextSetter(secondLetterBoxes, "skill");
                KeyboardColorSetter("skill", "23222");
                EnterLastLine("skill");
                break;

            case WordToSelect.ULTRA:
                BoxTextSetter(firstLetterBoxes, "plane");
                BoxColorSetter(firstLetterBoxes, "01200");
                
                textColorList.AddRange(new int[]{0,1,2,0,0});
                BoxTextColorSetter(firstLetterBoxes,textColorList);

                KeyboardColorSetter("plane", "32133");
                BoxTextSetter(secondLetterBoxes, "aloft");
                KeyboardColorSetter("aloft", "12331");
                EnterLastLine("aloft");
                break;

            case WordToSelect.FRAME:
                BoxTextSetter(firstLetterBoxes, "snare");
                BoxColorSetter(firstLetterBoxes, "00121");
                
                textColorList.AddRange(new int[]{0,0,1,2,1});
                BoxTextColorSetter(firstLetterBoxes,textColorList);

                KeyboardColorSetter("snare", "33212");
                BoxTextSetter(secondLetterBoxes, "brake");
                KeyboardColorSetter("brake", "32232");
                EnterLastLine("brake");
                break;

            case WordToSelect.DRUNK:
                BoxTextSetter(firstLetterBoxes, "dreck");
                BoxColorSetter(firstLetterBoxes, "11001");
                
                textColorList.AddRange(new int[]{1,1,0,0,1});
                BoxTextColorSetter(firstLetterBoxes,textColorList);
                
                KeyboardColorSetter("dreck", "22332");
                BoxTextSetter(secondLetterBoxes, "drack");
                KeyboardColorSetter("drack", "22332");
                EnterLastLine("drack");

                break;

            case WordToSelect.COULD:
                BoxTextSetter(firstLetterBoxes, "hound");
                BoxColorSetter(firstLetterBoxes, "01101");
                
                textColorList.AddRange(new int[]{0,1,1,0,1});
                BoxTextColorSetter(firstLetterBoxes,textColorList);

                
                KeyboardColorSetter("hound", "32232");
                BoxTextSetter(secondLetterBoxes, "sound");
                KeyboardColorSetter("sound", "32232");
                EnterLastLine("sound");
                break;
        }
    }


    private void BoxColorSetter(GameObject[] letterBoxes, string colorCodes)
    {
        for (int i = 0; i < letterBoxes.Length; i++)
        {
            letterBoxes[i].transform.GetChild(0).gameObject.GetComponent<Image>().color =
                _wordDisplayController._colors[int.Parse(colorCodes[i].ToString())];
        }
    }

    private void BoxTextSetter(GameObject[] letterBoxes, string word)
    {
        for (int i = 0; i < letterBoxes.Length; i++)
        {
            letterBoxes[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
                word[i].ToString().ToUpper();
        }
    }

    private void BoxTextColorSetter(GameObject[] letterBoxes, List<int> colorCodes)
    {
        for (int i = 0; i < letterBoxes.Length; i++)
        {
            if (colorCodes[i] == 0)
            {
                letterBoxes[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
            }
        }
        colorCodes.Clear();
    }

    private void KeyboardColorSetter(string key, string colorCodes)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject.FindGameObjectWithTag(key[i].ToString().ToUpper()).GetComponent<Image>().color =
                _keyboardController._keyboardColors[int.Parse(colorCodes[i].ToString())];
        }
    }

    private void EnterLastLine(string word)
    {
        _wordDisplayController.OnEnterPressPlayableVersionTwo(word);
    }


    private void OnApplicationQuit()
    {
        _wordsData.WordList.Remove(correctWord);
    }
}