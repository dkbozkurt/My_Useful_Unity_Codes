using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CpiTemplate.Game.Scripts.Behaviours;
using CpiTemplate.Game.Scripts.Models;

    public class SelectedWordController : MonoBehaviour
    {
        [SerializeField] private WordsData _wordsData;
        
        [LunaPlaygroundField("Generate Random Correct Word",0,"Correct Word Settings")][SerializeField]
        private bool selectRandomCorrectWord;
        
        [LunaPlaygroundField("Assigned Correct Word", 1, "Correct Word Settings")] [SerializeField]
        private string  correctWord;
        
        private void OnEnable()
        {
            if (selectRandomCorrectWord)
                _wordsData.SetRandomWord();
            else
                setAssignedWord();
        }
        private void setAssignedWord()
        {
            correctWord = correctWord.ToLower();
            _wordsData.WordList.Add(correctWord);
            _wordsData.selectedWord = correctWord;
            _wordsData.TestThisWord();
        }
        private void OnApplicationQuit()
        {
            if (!selectRandomCorrectWord)
            {
                _wordsData.WordList.Remove(correctWord);    
            }
        }
    }


