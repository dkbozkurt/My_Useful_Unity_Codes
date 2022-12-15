// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;
using DG.Tweening;

namespace DoTween.Scripts
{
    /// <summary>
    /// 
    /// </summary>
    public class DoTweenTo : MonoBehaviour
    {
        public float InValue;
        public float EndValue;
        public float Duration;
        
        private void Start()
        {
            DoTo();
        }

        private void DoTo()
        {
            DOTween.To(()=> InValue, x=> InValue = x, EndValue, Duration).SetEase(Ease.Linear);
        }
    }
}
