using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Playable.Scripts.PreScripts
{
    public class TextChanger : MonoBehaviour
    {
        [SerializeField] private GameObject _text;
        [SerializeField] private Text leftPlayerName;
        [SerializeField] private Text mainPlayerName;
        [SerializeField] private Text rightPlayerName;

        [LunaPlaygroundField("Tutorial text", 0, "Text settings")] 
        [SerializeField] private string _tutorialText = " ";
        //private List<string> _tutorialText=new List<string>();
        
        [LunaPlaygroundField("Left Player Text", 1, "Text settings")] 
        [SerializeField] private string leftPlayerText = " ";
        
        [LunaPlaygroundField("Main Player Text", 2, "Text settings")] 
        [SerializeField] private string mainPlayerText = " ";
        
        [LunaPlaygroundField("Right Player Text", 3, "Text settings")] 
        [SerializeField] private string rightPlayerText = " ";
        

        private int _tutorialTextIndex;

        private void Awake()
        {
            _tutorialTextIndex = 0;
            _text.GetComponent<TextMeshProUGUI>().text = _tutorialText;
            //TextAnimation(1f);
            AssignPlayerNames();
        }

        private void AssignPlayerNames()
        {
            leftPlayerName.text = leftPlayerText;
            mainPlayerName.text = mainPlayerText;
            rightPlayerName.text = rightPlayerText;
        }

        // private void Update()
        // {
        //     if (_tutorialTextIndex == 0 && Input.GetMouseButtonDown(0))
        //     {
        //         DeactivateTutorialText(0.1f);
        //     }
        //     else if (_text.activeSelf && _tutorialTextIndex != 0)
        //     {
        //         DeactivateTutorialText(2f);
        //     }
        // }
        //
        // private void TextAnimation(float duration)
        // {
        //     _text.transform.DOScale(Vector3.one * 1.5f, duration).SetEase(Ease.Linear).OnComplete(() =>
        //     {
        //         _text.transform.DOScale(Vector3.one, duration).SetEase(Ease.Linear)
        //             .OnComplete(() => TextAnimation(duration));
        //     });
        // }
        //
        // public void TutorialTextChanger()
        // {
        //     _tutorialTextIndex++;
        //     if (_tutorialTextIndex < _tutorialText.Count)
        //     {
        //         _text.SetActive(true);
        //         _text.GetComponent<TextMeshProUGUI>().text = _tutorialText[_tutorialTextIndex];
        //     }
        // }
        //
        // private void DeactivateTutorialText(float time)
        // {
        //     StartCoroutine(Do());
        //     IEnumerator Do()
        //     {
        //         yield return new WaitForSeconds(time);
        //         _text.SetActive(false);
        //     }
        // }
    }
}