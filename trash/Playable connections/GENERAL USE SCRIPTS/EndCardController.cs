// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using CpiTemplate.Game.Playable.Scripts.Helpers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CpiTemplate.Game.Playable.Scripts.PlayableConnections
{
    public enum EndCarCallType
    {
        AllFadeInStatic,
        AllFadeInDynamicPlayButton,
        AllScaleUpStatic,
        AllInImagesSlideRightStatic,
        GatherInTheMiddleDynamicPlayButton,
    }

	/// <summary>
    /// IMPORTANT NOTES :
    ///     All end card components MUST be child of EndCardController object.
    ///     "_endCardBackground" object MUST have button property that connects to "CtaController.OpenStore".
    ///     "_endCardBackground" MUST be the parent of all other end card components, which also should be set active false.
    ///     "_endCardBackground" objects, child objects gotta be set active true
    ///     All other sub images MUST ignore "Raycast Target"
    ///     All end card components MUST have scale of 1 and alpha 1.
    ///
    ///     #####   #####   #####
    /// 
    ///     EndCard (with EndCardController.cs)
    ///         -> EndCardBackground (Set active false)
    ///             -> End Card Icon (Set active true)
    ///             -> End Card Text (Set active true)
    ///             -> End Card Play Button  (Set active true)
    /// 
    /// </summary>
    public class EndCardController : SingletonBehaviour<EndCardController>
    {
        public static Action OnGameEnd;
        protected override void OnAwake()
        { }
        
        [LunaPlaygroundField("Open store after end card shown", 0, "End card Settings")] [SerializeField]
        private bool _openStoreAfterEndCard = true;
        
        [LunaPlaygroundField("Show end card seconds, after last input", 1, "End card Settings")] [SerializeField]
        private float _showEndCardSecondsAfterLastInput = 5.5f;
        
        [LunaPlaygroundField("Open end card after taps",2,"End card Settings")][SerializeField]
        private int _openEndCardAfterTaps = 20;
        
        [LunaPlaygroundField("End card animation call type", 3, "End card Settings")] [SerializeField]
        private EndCarCallType _endCarCallType = EndCarCallType.AllFadeInStatic;
        
        [LunaPlaygroundAsset("Icon Texture", 4, "End card Settings")]
        public Texture2D IconTexture2D;
        
        public Image EndCardBackground;
        public Image EndCardIcon;
        public Image EndCardText;
        public Image EndCardPlayButton;
        
        private Image[] _allEndCardImages;
        
        private int _tapCounter;
        private float _timer = 0f;
        private bool _timerChecker = false;
        
        private bool _storeOpened;
        private bool _isEndCardOpened;
        
        private bool _isThereAnyInput;
        
        #region InitialPositionInformations
        
        private Vector3 _initialIconPosition,
                        _initialTextPosition,
                        _initialPlayButtonPosition = Vector3.zero;
        #endregion
        
        private void Awake()
        {
            _allEndCardImages = new Image[4] {EndCardBackground, EndCardIcon, EndCardText, EndCardPlayButton};
          
            _storeOpened = false;
            _isEndCardOpened = false;
            _isThereAnyInput = false;
            _tapCounter = 0;
            GetInitialPositions();
        
            if(IconTexture2D != null) SetIcon();
        }
        
        private void GetInitialPositions()
        {
            if(EndCardIcon != null) _initialIconPosition = EndCardIcon.transform.localPosition;
            if(EndCardText != null) _initialTextPosition = EndCardText.transform.localPosition;
            if(EndCardPlayButton != null) _initialPlayButtonPosition = EndCardPlayButton.transform.localPosition;
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
        
                if (!_isThereAnyInput)
                {
                    _isThereAnyInput = true;
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
        
        public void OpenEndCard()
        {
            var obj = GameObject.Find("TutorialManager");
            if(obj!= null) obj.SetActive(false);
            
            // foreach (Transform child in transform)
            // {
            //     child.gameObject.SetActive(true);
            // }
        
            if(_isEndCardOpened) return;
            
            _isEndCardOpened = true;

            DecideEndCardCall(_endCarCallType);
            
            Luna.Unity.Analytics.LogEvent(Luna.Unity.Analytics.EventType.EndCardShown);
            
            if (_openStoreAfterEndCard)
            {
                if (!_storeOpened)
                {
                    DOVirtual.DelayedCall(0.5f,()=>Luna.Unity.LifeCycle.GameEnded());
                    DOVirtual.DelayedCall(1f, ()=>OnGameEnd?.Invoke());
                    DoAfterSeconds(1f, CtaController.Instance.OpenStoreStatic);
                    _storeOpened = true;
                }
            }
        }
        
        public void CloseEndCard()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            
            _storeOpened = false;
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
        
        private void DecideEndCardCall(EndCarCallType callType)
        {
            switch (callType)
            {
                case EndCarCallType.AllFadeInStatic:
                    OpenEndCardWithFadeIn();
                    break;
                case EndCarCallType.AllFadeInDynamicPlayButton:
                    OpenEndCardWithAllFadeInDynamic();
                    break;
                case EndCarCallType.AllScaleUpStatic:
                    OpenEndCardWithScaleUp();
                    break;
                case EndCarCallType.AllInImagesSlideRightStatic:
                    OpenEndCardWithAllInImagesSlideRight();
                    break;
                case EndCarCallType.GatherInTheMiddleDynamicPlayButton:
                    OpenEndCardWithGatherInTheMiddle();
                    break;
                default:
                    goto case EndCarCallType.AllFadeInStatic;
            }
        }
        
        private void ActivateEndCardComponents()
        {
            EndCardBackground.gameObject.SetActive(true);
            EndCardIcon.gameObject.SetActive(true);
            EndCardText.gameObject.SetActive(true);
            EndCardPlayButton.gameObject.SetActive(true);
        }
        private void OpenEndCardWithFadeIn()
        {
            foreach (var image in _allEndCardImages)
            {
                SetImageAlpha(image,0f);
            }
        
            ActivateEndCardComponents();
            SetImageAlpha(EndCardBackground,0.7f,0.7f,Ease.Linear);
            SetImageAlpha(EndCardIcon,1f,0.7f,Ease.Linear);
            SetImageAlpha(EndCardText, 1f, 0.7f, Ease.Linear);
            SetImageAlpha(EndCardPlayButton, 1f, 0.7f, Ease.Linear);
        }
        
        private void OpenEndCardWithAllFadeInDynamic()
        {
            foreach (var image in _allEndCardImages)
            {
                SetImageAlpha(image,0f);
            }
            
            ActivateEndCardComponents();
            
            SetImageAlpha(EndCardBackground,0.7f,0.7f,Ease.Linear);
            SetImageAlpha(EndCardIcon,1f,0.7f,Ease.Linear);
            SetImageAlpha(EndCardText, 1f, 0.7f, Ease.Linear);
            SetImageAlpha(EndCardPlayButton, 1f, 0.7f, Ease.Linear).OnComplete(() =>
            {
                SetImageScale(EndCardPlayButton, Vector3.one * 1.2f, 0.5f,Ease.Linear).SetLoops(-1,LoopType.Yoyo);
            });
        }
        
        private void OpenEndCardWithScaleUp()
        {
            foreach (var image in _allEndCardImages)
            {
                SetImageScale(image,Vector3.zero);
            }
            
            ActivateEndCardComponents();
            
            SetImageAlpha(EndCardBackground,0.7f);
            
            SetImageScale(EndCardBackground,Vector3.one,0.7f);
            SetImageScale(EndCardIcon,Vector3.one,0.7f);
            SetImageScale(EndCardText, Vector3.one, 0.7f);
            SetImageScale(EndCardPlayButton,Vector3.one,0.7f);
        }
        
        private void OpenEndCardWithAllInImagesSlideRight()
        {
            SetImageAlpha(EndCardBackground,0f);
            SetImageScale(EndCardIcon, Vector3.zero);
            SetImageScale(EndCardText, Vector3.zero);
            SetImageScale(EndCardPlayButton, Vector3.zero);
        
            SetImagePosition(EndCardIcon, new Vector3(-2250,_initialIconPosition.y,0));
            SetImagePosition(EndCardText, new Vector3(-2250, _initialTextPosition.y, 0));
            SetImagePosition(EndCardPlayButton, new Vector3(-2250,_initialPlayButtonPosition.y, 0));
        
            ActivateEndCardComponents();
            
            SetImageAlpha(EndCardBackground, 0.7f, 0.3f, Ease.Linear);
            SetImageScale(EndCardIcon, Vector3.one,0.1f);
            SetImageScale(EndCardText, Vector3.one,0.1f);
            SetImageScale(EndCardPlayButton, Vector3.one,0.1f);
            
            SetImagePosition(EndCardIcon, _initialIconPosition,1f,Ease.OutBack);
            SetImagePosition(EndCardText, _initialTextPosition ,1f,Ease.OutBack);
            SetImagePosition(EndCardPlayButton, _initialPlayButtonPosition,1f,Ease.OutBack);
        
        }
        
        private void OpenEndCardWithGatherInTheMiddle()
        {
            SetImageAlpha(EndCardBackground,0f);
            SetImageScale(EndCardIcon, Vector3.zero);
            SetImageScale(EndCardText, Vector3.zero);
            SetImageScale(EndCardPlayButton, Vector3.zero);
        
            SetImagePosition(EndCardIcon, new Vector3(_initialIconPosition.x,1750,0));
            SetImagePosition(EndCardText, new Vector3(-2000,_initialTextPosition.y, 0));
            SetImagePosition(EndCardPlayButton, new Vector3(_initialPlayButtonPosition.x,-1750, 0));
        
            ActivateEndCardComponents();
            
            SetImageAlpha(EndCardBackground, 0.7f, 0.5f, Ease.Linear);
            SetImageScale(EndCardIcon, Vector3.one,0.1f);
            SetImageScale(EndCardText, Vector3.one,0.1f);
            SetImageScale(EndCardPlayButton, Vector3.one,0.1f);
            
            SetImagePosition(EndCardIcon, _initialIconPosition,0.6f,Ease.OutQuint);
            SetImagePosition(EndCardText, _initialTextPosition ,0.6f,Ease.OutQuint);
            SetImagePosition(EndCardPlayButton, _initialPlayButtonPosition,0.6f,Ease.OutQuint).OnComplete(() =>
            {
                SetImageScale(EndCardPlayButton, Vector3.one * 1.2f, 0.5f,Ease.Linear).SetLoops(-1,LoopType.Yoyo);
            });
        }
        
        private Tween SetImageAlpha(Image image,float targetAlphaValue,float alphaDuration=0,
            Ease easeForAlpha = Ease.OutQuad)
        {
            return image.DOFade(targetAlphaValue, alphaDuration).SetEase(easeForAlpha);
        }
        
        private Tween SetImageScale(Image image, Vector3 targetScaleValue, float scaleDuration = 0,
            Ease easeForScale = Ease.OutQuad)
        {
            return image.rectTransform.DOScale(targetScaleValue, scaleDuration).SetEase(easeForScale);
        }
        
        private Tween SetImagePosition(Image image, Vector3 targetPosition, float positionDuration = 0,
            Ease easeForPosition = Ease.OutQuad)
        {
            return image.rectTransform.DOLocalMove(targetPosition, positionDuration).SetEase(easeForPosition);
        }
        
        private void SetIcon()
        {
            Rect rec = new Rect(0, 0, IconTexture2D.width, IconTexture2D.height);
            EndCardIcon.sprite = Sprite.Create(IconTexture2D, rec, new Vector2(0, 0), 1);;
        }
    }
}