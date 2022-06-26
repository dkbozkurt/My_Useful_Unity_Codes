using System;
using Game.Scripts.Behaviours;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Playable.Scripts.PreScripts
{
    public class TextChanger : MonoBehaviour
    {
        [LunaPlaygroundField("Tutorial text", 0, "Text settings")] [SerializeField]
        private string _tutorialText;

        [SerializeField] private GameObject _text;
        
        private void Awake()
        {
            _text.GetComponent<Text>().text = _tutorialText;
        }
    }
}