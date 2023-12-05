using DG.Tweening;
using PlayableAdsKit.Scripts.Helpers;
using UnityEngine;

namespace PlayableAdsKit.Scripts.PlaygroundConnections
{
    public class TutorialController : SingletonBehaviour<TutorialController>
    {
        protected override void OnAwake() { }
        
        [SerializeField] private RectTransform _tutorialTextParent;
        [SerializeField] private CanvasGroup _tutorialTextCanvasGroup;

        [Header("Tutorial Hand Properties")] 
        [SerializeField] private Animator _tutorialHandAnimator;
        [SerializeField] private CanvasGroup _tutorialHandCanvasGroup;

        private void Start()
        {
            Activate();
        }
        
        // Activates the Tutorial
        public void Activate()
        {
            TutorialTextSetter(true);
            AnimateTutorialText();
        }
        
        
        // Deactivates the Tutorial
        public void Deactivate()
        {
            TutorialTextSetter(false);
            TutorialHandSetter(false);
        }

        // Controls the tutorial text`s status
        public void TutorialTextSetter(bool status)
        {
            if (!status)
            {
                _tutorialTextCanvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
                {
                    _tutorialTextParent.gameObject.SetActive(false);
                });
                return;
            }

            _tutorialTextParent.gameObject.SetActive(true);
            _tutorialTextCanvasGroup.alpha = 1f;
        }
        
        // Controls the tutorial hand`s status
        public void TutorialHandSetter(bool status, string animName="", string animToSetFalse = "")
        {
            if (!status)
            {
                _tutorialHandCanvasGroup.DOFade(0f,0.2f).OnComplete(() =>
                {
                    _tutorialHandAnimator.gameObject.SetActive(false);
                });
                return;
            }

            if (animName == "")
            {
                Debug.LogError("Animation name can not be empty");
                return;
            }
            
            _tutorialHandAnimator.gameObject.SetActive(true);
            _tutorialHandCanvasGroup.alpha = 1f;
            
            if(animToSetFalse != "")
                _tutorialHandAnimator.SetBool(animToSetFalse, false);
            
            _tutorialHandAnimator.SetBool(animName, true);
        }

        private void AnimateTutorialText()
        {
            var initialScale = _tutorialTextParent.transform.localScale;
            var targetScale = initialScale * 0.85f;
            _tutorialTextParent.transform.DOScale(targetScale, 1.5f).SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}