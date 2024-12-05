using System;
using DG.Tweening;
using DkbozkurtPlayableAdsTool.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace Hybrid.Game.Playable.Scripts.Managers
{
    public class TutorialManager : SingletonBehaviour<TutorialManager>
    {
        protected override void OnAwake() { }

        [LunaPlaygroundField("Tutorial Texts", 0, "Tutorial Settings")] [SerializeField]
        private string[] _tutorialTexts = new string[] { };

        [LunaPlaygroundField("Zone Texts", 1, "Tutorial Settings")] [SerializeField]
        private bool _zoneTextsAreActive = true;
        
        [Space]
        [SerializeField] private GameObject _tutorialHandParent;
        [SerializeField] private GameObject _tutorialTextParent;
        [SerializeField] private TextMeshProUGUI _tutorialText;
        [SerializeField] private Transform _tutorialArrowParent;
        [SerializeField] private GameObject[] _zoneTexts;

        [Space]
        [SerializeField] private Vector3[] _tutorialArrowPositions;
        
        [Space]
        [SerializeField] private float _durationToDeactivateTutorial = 0.75f;

        private float _timer = 0f;
        private bool _startTimer;
        private bool _isTutorialOn = true;

        private void Awake()
        {
            SetZoneTexts(_zoneTextsAreActive);
        }

        private void SetZoneTexts(bool status)
        {
            foreach (var child in _zoneTexts)
            {
                child.SetActive(status);
            }
        }

        private void OnEnable()
        {
            TutorialTextSetter(true);
            TutorialArrowSetter(true);
            TutorialHandSetter(true);
            AnimateTutorialText();
        }

        private void Update()
        {
            if (_isTutorialOn && Input.GetMouseButton(0))
            {
                _isTutorialOn = false;
                _timer = 0f;
                _startTimer = true;
            }

            if (_startTimer)
            {
                _timer += Time.deltaTime;
                if (_timer >= _durationToDeactivateTutorial)
                {
                    CloseUI();
                }
            }
        }

        private void TutorialTextSetter(bool status,int index = 0)
        {
            _tutorialTextParent.SetActive(status);
            
            _isTutorialOn = status;

            if (!status)
            {
                _startTimer = false;
                _timer = 0f;   
                return;
            }
            
            _tutorialText.text = _tutorialTexts[index];
        }

        private void TutorialArrowSetter(bool status,int index = 0)
        {
            _tutorialArrowParent.gameObject.SetActive(status);

            if (!status)
            {
                transform.DOKill();
                return;
            }

            _tutorialArrowParent.transform.position = _tutorialArrowPositions[index];
        }

        private void TutorialHandSetter(bool status)
        {
            _tutorialHandParent.SetActive(status);
        }

        private void AnimateTutorialText()
        {
            _tutorialTextParent.transform.DOScale(Vector3.one * 0.8f, 2f).SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void CloseUI()
        {
            TutorialArrowSetter(false);
            TutorialHandSetter(false);
            TutorialTextSetter(false);
        }
    }
}
