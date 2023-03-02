// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tooltip_System.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=HXFoUGw7eKk&ab_channel=GameDevGuide
    /// </summary>
    [ExecuteInEditMode()]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private LayoutElement _layoutElement;
        [SerializeField] private TextMeshProUGUI _headerField;
        [SerializeField] private TextMeshProUGUI _contentField;

        [SerializeField] private int _characterWrapLimit = 80;

        [SerializeField] private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            FollowMouse();

            if(Application.isEditor) CheckTheLengthOfContentNFit();
        }

        public void SetText(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header))
            {
                _headerField.gameObject.SetActive(false);
            }
            else
            {
                _headerField.gameObject.SetActive(true);
                _headerField.text = header;
            }

            _contentField.text = content;

            CheckTheLengthOfContentNFit();
        }

        private void CheckTheLengthOfContentNFit()
        {
            int headerLength = _headerField.text.Length;
            int contentLength = _contentField.text.Length;

            _layoutElement.enabled = (headerLength > _characterWrapLimit || contentLength > _characterWrapLimit)
                ? true
                : false;
        }

        private void FollowMouse()
        {
            Vector2 mousePosition = Input.mousePosition;
            transform.position = mousePosition;
            
            DynamicallyAnchorTooltipPosition(mousePosition);
        }

        private void DynamicallyAnchorTooltipPosition(Vector2 mousePosition)
        {
            float pivotX = mousePosition.x / Screen.width;
            float pivotY = mousePosition.y / Screen.height;

            _rectTransform.pivot = new Vector2(pivotX, pivotY);
        }
    }
}