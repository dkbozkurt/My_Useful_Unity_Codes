// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace ScrollView.InfiniteScroll.Scripts
{
    public enum ScroolViewOrientation
    {
        Horizontal,
        Vertical
    }
    /// <summary>
    /// Ref : https://github.com/renellc/Unity3D-InfinitelyScrollingScrollRect
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class ScrollContent : MonoBehaviour
    {
        public float ItemSpacing { get { return _itemSpacing; } }
        public float HorizontalMargin { get { return _horizontalMargin; } }
        public float VerticalMargin { get { return _verticalMargin; } }
        public float Width { get { return _width; } }
        public float Height { get { return _height; } }
        public float ChildWidth { get { return _childWidth; } }
        public float ChildHeight { get { return _childHeight; } }

        public bool IsHorizontalOrientation =>_scroolViewOrientation == ScroolViewOrientation.Horizontal; 
        public bool IsVerticalOrientation => _scroolViewOrientation == ScroolViewOrientation.Vertical;
        
        [SerializeField] private float _itemSpacing;
        [SerializeField] private float _horizontalMargin;
        [SerializeField] private float _verticalMargin;
        [SerializeField] private ScroolViewOrientation _scroolViewOrientation = ScroolViewOrientation.Vertical;
        
        private RectTransform _rectTransform;
        private RectTransform[] _rectTransformChildren;
        private float _width;
        private float _height;
        private float _childWidth;
        private float _childHeight;
        
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransformChildren = new RectTransform[_rectTransform.childCount];

            for (int i = 0; i < _rectTransform.childCount; i++)
            {
                _rectTransformChildren[i] = _rectTransform.GetChild(i) as RectTransform;
            }

            _width = _rectTransform.rect.width - (2 * _horizontalMargin);
            _height = _rectTransform.rect.height - (2 * _verticalMargin);

            _childWidth = _rectTransformChildren[0].rect.width;
            _childHeight = _rectTransformChildren[0].rect.height;

            DecideOrientation();
        }

        private void DecideOrientation()
        {
            switch (_scroolViewOrientation)
            {
                case ScroolViewOrientation.Vertical:
                    InitializeContentVertical();    
                    break;
                case ScroolViewOrientation.Horizontal:
                    InitializeContentHorizontal();
                    break;
                default:
                    break;
            }
        }
        
        private void InitializeContentHorizontal()
        {
            float originX = 0 - (_width * 0.5f);
            float posOffset = _childWidth * 0.5f;
            for (int i = 0; i < _rectTransformChildren.Length; i++)
            {
                Vector2 childPos = _rectTransformChildren[i].localPosition;
                childPos.x = originX + posOffset + i * (_childWidth + _itemSpacing);
                _rectTransformChildren[i].localPosition = childPos;
            }
        }
        
        private void InitializeContentVertical()
        {
            float originY = 0 + (_height * 0.5f);
            float posOffset = _childHeight * 0.5f;
            for (int i = 0; i < _rectTransformChildren.Length; i++)
            {
                Vector2 childPos = _rectTransformChildren[i].localPosition;
                childPos.y = originY - posOffset - i * (_childHeight + _itemSpacing);
                _rectTransformChildren[i].localPosition = childPos;
            }
        }
    }
}