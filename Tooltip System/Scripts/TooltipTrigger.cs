// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tooltip_System.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=HXFoUGw7eKk&ab_channel=GameDevGuide
    /// </summary>
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] private string _header;
        
        [TextArea(3,8)]
        [SerializeField] private string _content;

        private UnityEngine.Coroutine _activeCoroutine;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _activeCoroutine =
                StartCoroutine(DoAfterTimeCoroutine(0.5f, () =>
                {
                    TooltipSystem.Instance.Show(_content, _header);
                }));

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopCoroutine(_activeCoroutine);
            TooltipSystem.Instance.Hide();
        }

        private IEnumerator DoAfterTimeCoroutine(float duration, Action action = null)
        {
            yield return new WaitForSeconds(duration);
            action?.Invoke();
        }
    }
}