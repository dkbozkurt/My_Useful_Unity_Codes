using System.Collections;
using UnityEngine;
using System;

namespace Game.Playable.Scripts.PreScripts
{
    public class EndCardController : MonoBehaviour
    {
        [LunaPlaygroundField("Open store after end card shown", 0, "End card Settings")] [SerializeField]
        private bool _openStoreAfterEndCard;

        [LunaPlaygroundField("Show end card after seconds", 1, "End card Settings")] [SerializeField]
        private float _showEndCardAfterSeconds;
        
        private static Transform _transform;
        private static bool _openStore;
        
        private static bool _storeOpened;
        
        private void Awake()
        {
            _transform = transform;
            _openStore = _openStoreAfterEndCard;
            _storeOpened = false;
            ShowEndCardTimer(_showEndCardAfterSeconds);
        }
        
        public void Enable()
        {
            
            foreach (Transform child in _transform)
            {
                child.gameObject.SetActive(true);
            }
            
            // For mintegral
            Luna.Unity.LifeCycle.GameEnded();
            
            if (_openStore)
            {
                if (!_storeOpened)
                {
                    DoAfterSeconds(1f,CtaController.OpenStoreStatic);
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
        
        public void ShowEndCardTimer(float seconds)
        {
            StartCoroutine(Do());

            IEnumerator Do()
            {
                yield return new WaitForSeconds(seconds);
                Enable();
                
            }
        }
    }
}