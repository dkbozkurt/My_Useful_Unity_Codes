using System;
using System.Collections;
using UnityEngine;

namespace Game.Playable.Scripts.PreScripts
{
    public class CtaController : MonoBehaviour
    {
        [LunaPlaygroundField("Open store after seconds", 0, "Store Settings")] [SerializeField]
        private float _openStoreAfterSeconds;

        [LunaPlaygroundField("Open store after taps", 1, "Store Settings")] [SerializeField]
        private float _openStoreAfterTaps;
        
        [LunaPlaygroundField("Open endcard after taps",2,"Store Settings")][SerializeField]
        private float _openEndCardAfterTaps;

        private int _tapCounter;

        [SerializeField] private GameObject _endCard;
        
        private void Awake()
        {
            DoAfterSeconds(_openStoreAfterSeconds, OpenStore);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _tapCounter++;

                if (_tapCounter == _openStoreAfterTaps)
                {
                    Luna.Unity.LifeCycle.GameEnded();
                    OpenStore();
                }

                if (_tapCounter == _openEndCardAfterTaps)
                {
                    OpenEndCardAfterTaps();
                }
                    
            }
        }

        public void OpenStore()
        {
            Luna.Unity.Playable.InstallFullGame();
        }

        public static void OpenStoreStatic()
        {
            Luna.Unity.Playable.InstallFullGame();
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

        public void OpenEndCardAfterTaps()
        {
            _endCard.GetComponent<EndCardController>().OpenEndCard();
        }
    }
}