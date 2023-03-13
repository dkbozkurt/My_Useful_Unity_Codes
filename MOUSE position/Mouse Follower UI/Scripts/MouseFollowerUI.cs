// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using DG.Tweening;
using UnityEngine;

namespace MousePosition___MouseClick.Mouse_Follower_UI.Scripts
{
    /// <summary>
    /// 
    /// </summary>
    public class MouseFollowerUI : MonoBehaviour
    {
        [SerializeField] [Multiline] private string _notes = "Space : Hand images activate toggle.\nQ : To Change hand image.";
        
        [Space]
        [SerializeField] private Transform _handImagesPivot;
        [SerializeField] private GameObject[] _mouseImages;

        private bool _isCursorOn = true;
        private int _lastActivatedMouseImageIndex = 0;

        private void Awake()
        {
            _handImagesPivot.gameObject.SetActive(false);

            foreach (var image in _mouseImages)
            {
                image.SetActive(false);
            }
        }

        private void Start()
        {
            CursorSettings();

            ChangeMouseImage();
        }

        private void Update()
        {
            FollowMouse();
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isCursorOn = !_isCursorOn;
                CursorSettings();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if(_isCursorOn) return;
                ChangeMouseImage();
            }

            if (Input.GetMouseButtonDown(0))
            {
                AnimateFollowerClicked(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                AnimateFollowerClicked(false);
            }

        }

        private void CursorSettings()
        {
            Cursor.visible = _isCursorOn;
            _handImagesPivot.gameObject.SetActive(!_isCursorOn);
        }

        private void AnimateFollowerClicked(bool status)
        {
            _handImagesPivot.DOKill();

            if (status)
            {
                _handImagesPivot.DOScale(Vector3.one * 0.8f, 0.1f).SetEase(Ease.Linear);
            }
            else
            {
                _handImagesPivot.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
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