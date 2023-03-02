// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using CpiTemplate.Playable.Scripts.Helpers;
using UnityEngine;

namespace Tooltip_System.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=HXFoUGw7eKk&ab_channel=GameDevGuide
    /// </summary>
    public class TooltipSystem : SingletonBehaviour<TooltipSystem>
    {
        protected override void OnAwake() { }

        [SerializeField] private Tooltip _tooltip;

        public void Show(string content, string header = "")
        {
            _tooltip.SetText(content,header);
            _tooltip.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _tooltip.gameObject.SetActive(false);
        }
    }
}
