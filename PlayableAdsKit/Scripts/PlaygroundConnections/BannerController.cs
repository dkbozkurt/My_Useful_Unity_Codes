using DG.Tweening;
using UnityEngine;

namespace PlayableAdsKit.Scripts.PlaygroundConnections
{
    public class BannerController : MonoBehaviour
    {
        [SerializeField] private RectTransform _logoShineEffect;
        [SerializeField] private RectTransform _button;

        private void Start()
        {
            AnimateLogoShineEffect();
            AnimateButton();
        }

        public void CallStore()
        {
#if UNITY_LUNA            
            Luna.Unity.Analytics.LogEvent("banner_clicked",0);
#endif
            CtaController.Instance.OpenStore();
        }

        private void AnimateLogoShineEffect()
        {
            _logoShineEffect.DOLocalRotate(Vector3.forward * 360f, 3f,RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

        private void AnimateButton()
        {
            _button.DOLocalMoveY(0f, 0.75f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _button.DOLocalMoveY(25f, 0.75f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    AnimateButton();
                });
            });
        }
    }
}
