// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScrollView.InfiniteScroll.Scripts
{
    /// <summary>
    /// Ref : https://github.com/renellc/Unity3D-InfinitelyScrollingScrollRect
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public class InfiniteScroll : MonoBehaviour, IBeginDragHandler,IDragHandler,IScrollHandler
    {
        [SerializeField] private ScrollContent _scrollContent;
        [SerializeField] private float _outOfBoundsThreshold;

        private ScrollRect _scrollRect;
        private Vector2 _lastDragPosition;
        private bool _positiveDrag;
        
        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _scrollRect.horizontal = _scrollContent.IsHorizontalOrientation;
            _scrollRect.vertical = _scrollContent.IsVerticalOrientation;
            _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _lastDragPosition = eventData.position;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_scrollContent.IsVerticalOrientation)
            {
                _positiveDrag = eventData.position.y > _lastDragPosition.y;
            }
            else if (_scrollContent.IsHorizontalOrientation)
            {
                _positiveDrag = eventData.position.x > _lastDragPosition.x;
            }

            _lastDragPosition = eventData.position;
        }
        
        public void OnScroll(PointerEventData eventData)
        {
            if (_scrollContent.IsVerticalOrientation)
            {
                _positiveDrag = eventData.scrollDelta.y < 0;
            }
            else
            {
                // Scrolling up on the mouse wheel is considered a negative scroll, but I defined
                // scrolling downwards (scrolls right in a horizontal view) as the positive direciton,
                // so I check if the if scrollDelta.y is less than zero to check for a positive drag.
                _positiveDrag = eventData.scrollDelta.y < 0;
            }
        }
        
        public void OnViewScroll()
        {
            if (_scrollContent.IsVerticalOrientation)
            {
                HandleVerticalScroll();
            }
            else
                HandleHorizontalScroll();
        }
        
        private void HandleVerticalScroll()
        {
            int currItemIndex = _positiveDrag ? 0 : _scrollRect.content.childCount - 1;
            var currItem = _scrollRect.content.GetChild(currItemIndex);

            if (!ReachedThreshold(currItem))
            {
                return;
            }

            int endItemIndex = _positiveDrag ? _scrollRect.content.childCount - 1 : 0;
            Transform endItem = _scrollRect.content.GetChild(endItemIndex);
            Vector2 newPos = endItem.localPosition;

            if (_positiveDrag)
            {
                newPos.y = endItem.localPosition.y - _scrollContent.ChildHeight * 1f - _scrollContent.ItemSpacing;
            }
            else
            {
                newPos.y = endItem.localPosition.y + _scrollContent.ChildHeight * 1f + _scrollContent.ItemSpacing;
            }

            currItem.localPosition = newPos;
            currItem.SetSiblingIndex(endItemIndex);
        }

        private void HandleHorizontalScroll()
        {
            int currItemIndex = _positiveDrag ? _scrollRect.content.childCount - 1 : 0;
            var currItem = _scrollRect.content.GetChild(currItemIndex);
            if (!ReachedThreshold(currItem))
            {
                return;
            }

            int endItemIndex = _positiveDrag ? 0 : _scrollRect.content.childCount - 1;
            Transform endItem = _scrollRect.content.GetChild(endItemIndex);
            Vector2 newPos = endItem.localPosition;

            if (_positiveDrag)
            {
                newPos.x = endItem.localPosition.x - _scrollContent.ChildWidth * 1f - _scrollContent.ItemSpacing;
            }
            else
            {
                newPos.x = endItem.localPosition.x + _scrollContent.ChildWidth * 1f + _scrollContent.ItemSpacing;
            }

            currItem.localPosition = newPos;
            currItem.SetSiblingIndex(endItemIndex);
        }
        
        private bool ReachedThreshold(Transform item)
        {
            if (_scrollContent.IsVerticalOrientation)
            {
                float posYThreshold = transform.position.y + _scrollContent.Height * 0.5f + _outOfBoundsThreshold;
                float negYThreshold = transform.position.y - _scrollContent.Height * 0.5f - _outOfBoundsThreshold;
                return _positiveDrag ? item.position.y - _scrollContent.ChildWidth * 0.5f > posYThreshold :
                    item.position.y + _scrollContent.ChildWidth * 0.5f < negYThreshold;
            }
            else
            {
                float posXThreshold = transform.position.x + _scrollContent.Width * 0.5f + _outOfBoundsThreshold;
                float negXThreshold = transform.position.x - _scrollContent.Width * 0.5f - _outOfBoundsThreshold;
                return _positiveDrag ? item.position.x - _scrollContent.ChildWidth * 0.5f > posXThreshold :
                    item.position.x + _scrollContent.ChildWidth * 0.5f < negXThreshold;
            }
        }
    }
}