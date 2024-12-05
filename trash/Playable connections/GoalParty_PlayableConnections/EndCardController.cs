using System.Collections;
using UnityEngine;
using System;

namespace Game.Playable.Scripts.PreScripts
{
    public class EndCardController : MonoBehaviour
    {
        [LunaPlaygroundField("Open store after end card shown", 0, "End card Settings")] [SerializeField]
        private bool _openStoreAfterEndCard;

        [LunaPlaygroundField("Show end card seconds after last input", 1, "End card Settings")] [SerializeField]
        private float _showEndCardAfterSeconds;

        [SerializeField] private GameObject aim;
        private float _timer = 0f;
        private bool _timerChecker = false;

        private static Transform _transform;

        private static bool _openStore;
        private static bool _storeOpened;
        
        private void Awake()
        {
            _transform = transform;
            _openStore = _openStoreAfterEndCard;
            _storeOpened = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _timer = 0f;
                _timerChecker = false;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _timerChecker = true;
            }

            if (_timerChecker)
            {
                _timer += Time.deltaTime;

                if (_timer >= _showEndCardAfterSeconds)
                {
                    OpenEndCard();
                }
            }
        }

        public void OpenEndCard()
        {
            if(GameObject.Find("Tutorial") != null) GameObject.Find("Tutorial").SetActive(false);
            if(GameObject.Find("GoalAndFaılTexts") != null) GameObject.Find("GoalAndFaılTexts").SetActive(false);
            
            aim.SetActive(false);
            
            foreach (Transform child in _transform)
            {
                child.gameObject.SetActive(true);
            }
            
            if (_openStore)
            {
                if (!_storeOpened)
                {
                    Luna.Unity.LifeCycle.GameEnded();
                    DoAfterSeconds(1f, CtaController.OpenStoreStatic);
                    _storeOpened = true;
                }
            }
        }

        private void DoAfterSeconds(float seconds, Action action)
        {
            StartCoroutine(Do());

            IEnumerator Do()
            {
                yield return new WaitForSeconds(seconds);
                action?.Invoke();
            }
        }

        public void CloseEndCard()
        {
            foreach (Transform child in _transform)
            {
                child.gameObject.SetActive(false);
            }
            
            _storeOpened = false;
        }
    }
}