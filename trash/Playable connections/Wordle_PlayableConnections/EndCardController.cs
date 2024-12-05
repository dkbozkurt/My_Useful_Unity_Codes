using System;
using System.Collections;
using UnityEngine;

namespace Playable.Scripts
{
    public class EndCardController : MonoBehaviour
    {
        [LunaPlaygroundField("Open store after end card shown", 0, "End card")] [SerializeField]
        private bool _openStoreAfterEndCard;
        
        private static bool _openStore;

        private static bool _storeOpened;
        
        private void Awake()
        {
            _openStore = _openStoreAfterEndCard;
            _storeOpened = false;
        }

        public void OpenStoreAfterEndCard()
        {
            if (_openStore)
            {
                if (!_storeOpened)
                {
                    DoAfterSeconds(1f,CtaController.OpenStoreStatic);
                    _storeOpened = true;
                    // Added for Mintegral
                    // Luna.Unity.LifeCycle.GameEnded();
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
    }
}