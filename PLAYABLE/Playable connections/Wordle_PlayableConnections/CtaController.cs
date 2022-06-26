using System;
using System.Collections;
using UnityEngine;

namespace Playable.Scripts
{
    public class CtaController : MonoBehaviour
    {
        // [LunaPlaygroundField("Open store after seconds", 0, "Open Store")] [SerializeField]
        // private float _openStoreAfterSeconds;

        [SerializeField] private GameObject _endCard;
        
        [LunaPlaygroundField("Open store after taps", 0, "Open Store")] [SerializeField]
        private float _openStoreAfterTaps;

        [LunaPlaygroundField("Open end card after taps", 1, "End card")] [SerializeField]
        private float _openEndCardAfterTaps;
        

        private int _tapCounter;

        private void Awake()
        {
            // DoAfterSeconds(_openStoreAfterSeconds, OpenStore);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _tapCounter++;

                if (_tapCounter == _openStoreAfterTaps)
                {
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
            _endCard.SetActive(true);
            // Added for Mintegral
            Luna.Unity.LifeCycle.GameEnded();
            
            _endCard.transform.parent.gameObject.GetComponent<EndCardController>().OpenStoreAfterEndCard();
        }
    } 
}
