using System;
using DG.Tweening;
using PlayableAdsKit.Scripts.Helpers;
using PlayableAdsKit.Scripts.Utilities;
using UnityEngine;

namespace PlayableAdsKit.Scripts.PlaygroundConnections
{
    public class EndCardController : SingletonBehaviour<EndCardController>
    {
        public static event Action OnEndCardCalled;
        protected override void OnAwake()
        { }
        [LunaPlaygroundField("Open store after end card shown", 0, "EndCard Settings")]
        [SerializeField]
        private bool _openStoreAfterEndCard = true;
        
        [LunaPlaygroundField("Show end card seconds, after last input", 1, "EndCard Settings")]
        [SerializeField]
        private float _showEndCardSecondsAfterLastInput = 5.5f;
        [LunaPlaygroundField("Open end card after taps",2,"EndCard Settings")]
        [SerializeField]
        private int _openEndCardAfterTaps = 30;

        [LunaPlaygroundField("Open store after end card show delay",3,"EndCard Settings")]
        [SerializeField] private float _openStoreAfterEndCardCallDelay = 1.5f;

        [SerializeField] private GameObject _endCardCTAButton;
        [SerializeField] private RectTransform _shineEffect;
        [SerializeField] private RectTransform _base;
        [SerializeField] private RectTransform _claimButton;
        
        private int _tapCounter;
        private float _timer = 0f;
        private bool _timerChecker = false;
        
        private bool _storeOpened;
        private bool _isEndCardOpened;

        private CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            _storeOpened = false;
            _isEndCardOpened = false;
            _tapCounter = 0;

            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            
            _endCardCTAButton.SetActive(false);
            _base.localScale = Vector3.zero;
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _tapCounter++;
                _timer = 0f;
                _timerChecker = false;
        
                if (_tapCounter >= _openEndCardAfterTaps)
                {
                    OpenEndCard();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                _timerChecker = true;
            }
        
            if (_timerChecker)
            {
                _timer += Time.deltaTime;
        
                if (_timer >= _showEndCardSecondsAfterLastInput)
                {
                    OpenEndCard();
                }
            }
        }
        
        // Calls the EndCard
        public void OpenEndCard()
        {
            var obj = GameObject.Find("TutorialController");
            if(obj!= null) obj.SetActive(false);
            
            if(_isEndCardOpened) return;
            
            _isEndCardOpened = true;
            
            Luna.Unity.LifeCycle.GameEnded();
            AnimateEndCardCall();

            if (_openStoreAfterEndCard)
            {
                if (!_storeOpened)
                {
                    StartCoroutine(PlyAdsKitUtils.WaitForTimeCoroutine(_openStoreAfterEndCardCallDelay,
                        CtaController.Instance.OpenStore));
                    _storeOpened = true;
                }
            }
        }
        
        // Closes the EndCard
        public void CloseEndCard()
        {
            _isEndCardOpened = false;
            _endCardCTAButton.SetActive(false);
            _canvasGroup.alpha = 0f;
            _base.localScale = Vector3.zero;
            _timer = 0f;
            _tapCounter = 0;
        }

        public void CallStore()
        {
            Luna.Unity.Analytics.LogEvent("end-card_clicked",0);
            CtaController.Instance.OpenStore();
        }
        
        private void AnimateEndCardCall()
        {
            Luna.Unity.Analytics.LogEvent(Luna.Unity.Analytics.EventType.EndCardShown);
            OnEndCardCalled?.Invoke();
            
            _endCardCTAButton.SetActive(true);
            
            AnimateShineEffect();

            _canvasGroup.DOFade(1f, _openStoreAfterEndCardCallDelay / 2f).SetEase(Ease.Linear);
            _base.DOScale(Vector3.one, _openStoreAfterEndCardCallDelay/2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                AnimateClaimButton();    
            });
        }

        private void AnimateClaimButton()
        {
            _claimButton.DOScale(Vector3.one * 1.15f, 1f).SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void AnimateShineEffect()
        {
            _shineEffect.DOLocalRotate(Vector3.forward * 360f, 5f,RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }
    }
}