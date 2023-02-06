using System;
using CpiTemplate.Game.Playable.Scripts.Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CpiTemplate.Game.Creative.Scripts.Behaviours
{
    public class TriggerBehaviour : InteractableBase<CharacterMovementController>
    {
        public static event Action OnTriggerOver;

        [SerializeField] private Image _fillImage;
        [SerializeField] private float _fillDuration;
        [SerializeField] private Collider _collider;

        private Canvas _canvas;
        private bool _isPlayerTriggered = false;

        private void Awake()
        {
            _canvas = GetComponentInChildren<Canvas>();
            Toggle(true);
        }

        private void OnDisable()
        {
            _fillImage.DOKill();
        }

        public void Toggle(bool on)
        {
            _canvas.enabled = on;
            _collider.enabled = on;
        }

        public override void TriggerEnter(CharacterMovementController t)
        {
            if(_isPlayerTriggered) return;
            
            _isPlayerTriggered = true;
            
            _fillImage.DOKill();
            _fillImage.DOFillAmount(1, _fillDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                OnTriggerOver?.Invoke();
                Debug.Log("Trigger finished");
                ResetTrigger();
            });
        }

        public override void TriggerExit(CharacterMovementController t)
        {
            _isPlayerTriggered = false;
            RewindTrigger();
        }

        private void RewindTrigger()
        {
            _fillImage.DOKill();
            _fillImage.DOFillAmount(0, _fillDuration / 3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _fillImage.fillAmount = 0f;
            });
        }

        private void ResetTrigger()
        {
            _fillImage.DOKill();
            _fillImage.fillAmount = 0f;
        }
    }
}
