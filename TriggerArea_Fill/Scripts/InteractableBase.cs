using System;
using UnityEngine;

namespace CpiTemplate.Game.Creative.Scripts.Behaviours
{
    public abstract class InteractableBase<T> : MonoBehaviour
    {
        [SerializeField] protected bool _triggerOneTime = false;
        [SerializeField] protected bool _destroyAfterTrigger = false;
        [SerializeField] private bool _searchParentToo = false;
        
        protected bool _isTriggered = false;

        public void ResetInteractable()
        {
            _isTriggered = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if(_isTriggered ) return;

            T t = other.GetComponent<T>();

            if (t == null)
            {
                if(!_searchParentToo) return;

                t = other.GetComponentInParent<T>();
                if(t == null) return;
            }

            if (_triggerOneTime) _isTriggered = true;
            
            TriggerEnter(t);
            
            if(!_destroyAfterTrigger) return;

            _isTriggered = true;
            Destroy(gameObject);
        }

        public void OnTriggerExit(Collider other)
        {
            T t = other.GetComponent<T>();
            if (t == null)
            {
                if (!_searchParentToo)
                    return;

                t = other.GetComponentInParent<T>();
                if (t == null)
                    return;
            }

            TriggerExit(t);
        }
        
        public abstract void TriggerEnter(T t);
        public abstract void TriggerExit(T t);
        
    }
}
