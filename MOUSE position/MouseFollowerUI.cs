using DG.Tweening;
using UnityEngine;

namespace Hybrid.Game.Creative.Scripts.Controllers
{
    public class MouseFollowerUI : MonoBehaviour
    {
        [SerializeField] private Transform _handsPivot;
        [SerializeField] private GameObject[] _mouseImages;

        private bool _mickeyHandIsOn = true;
        private bool _isCursorOn = true;
        private int _lastActivatedMouseImageIndex = 0;

        private void Awake()
        {
            _handsPivot.gameObject.SetActive(false);
        }

        private void Start()
        {
            CursorSettings();

            ChangeMouseImage();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isCursorOn = !_isCursorOn;
                CursorSettings();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                ChangeMouseImage();
            }

            if (Input.GetMouseButtonDown(0))
            {
                AnimateFollowerClicked(true);
            }

            if (Input.GetMouseButtonUp(0))
            {
                AnimateFollowerClicked(false);
            }

            FollowMouse();
        }

        private void CursorSettings()
        {
            Cursor.visible = _isCursorOn;
            _handsPivot.gameObject.SetActive(!_isCursorOn);
        }

        private void AnimateFollowerClicked(bool status)
        {
            _handsPivot.DOKill();

            if (status)
            {
                _handsPivot.DOScale(Vector3.one * 0.8f, 0.1f).SetEase(Ease.Linear);
            }
            else
            {
                _handsPivot.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
            }
        }

        private void ChangeMouseImage()
        {
            if (_lastActivatedMouseImageIndex >= _mouseImages.Length) { _lastActivatedMouseImageIndex = 0; }

            for (int i = 0; i < _mouseImages.Length; i++)
            {
                _mouseImages[i].SetActive(i == _lastActivatedMouseImageIndex);
            }

            _lastActivatedMouseImageIndex++;
        }

        private void FollowMouse()
        {
            transform.position = Input.mousePosition;
        }
    }
}