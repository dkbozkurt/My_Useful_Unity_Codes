using System;
using System.Collections;
using UnityEngine;

namespace Game.Playable.Scripts.PreScripts
{
    public class CtaController : MonoBehaviour
    {
        [LunaPlaygroundField("Open store after seconds", 0, "Open Store")] [SerializeField]
        private float _openStoreAfterSeconds;

        [LunaPlaygroundField("Open store after taps", 1, "Open Store")] [SerializeField]
        private float _openStoreAfterTaps;

        private int _tapCounter;

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
                    OpenStore();
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
    }
}