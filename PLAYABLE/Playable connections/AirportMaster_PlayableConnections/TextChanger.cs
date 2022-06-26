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
        [LunaPlaygroundField("Tutorial text", 0, "Text settings")] [SerializeField]
        //private List<string> _tutorialText=new List<string>();
        private string _tutorialText = " ";

        [SerializeField] private GameObject _text;

        private int _tutorialTextIndex;

        private void Awake()
        {
            _tutorialTextIndex = 0;
            _text.GetComponent<TextMeshProUGUI>().text = _tutorialText;
            //TextAnimation(1f);
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