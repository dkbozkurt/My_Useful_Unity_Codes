using System;
using System.Collections;
using PlayableAdsKit.Scripts.Helpers;
using PlayableAdsKit.Scripts.Utilities;
using UnityEngine;

namespace PlayableAdsKit.Scripts.PlaygroundConnections
{
    public class CtaController : SingletonBehaviour<CtaController>
    {
        [LunaPlaygroundField("Open store after seconds", 0, "Store Settings")]
        [SerializeField] private float _openStoreAfterSeconds = 9999;

        [LunaPlaygroundField("Open store after taps", 1, "Store Settings")]
        [SerializeField] private int _openStoreAfterTaps = 9999;
        
        private int _tapCounter;

        [HideInInspector] public bool IsFirstInteraction = false; 
        
        private void Start()
        {
            StartCoroutine(PlyAdsKitUtils.WaitForTimeCoroutine(_openStoreAfterSeconds, OpenStoreWithGameEnded));
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsFirstInteraction) IsFirstInteraction = true;
                
                _tapCounter++;
        
                if (_tapCounter >= _openStoreAfterTaps)
                {
                    OpenStoreWithGameEnded();
                }
            }
        }

        // Call store with Game Ended event.
        public void OpenStoreWithGameEnded()
        {
            Luna.Unity.LifeCycle.GameEnded();
            OpenStore();
        }
        
        // Call store
        public void OpenStore()
        {
            Luna.Unity.Playable.InstallFullGame();
        }

        protected override void OnAwake() { }
    }
}