using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.StateMachine
{
    public abstract class StateManager<T> : MonoBehaviour where T : Enum
    {
        protected Dictionary<T, BaseState<T>> States = new Dictionary<T, BaseState<T>>();
        
        protected BaseState<T> CurrentState;

        protected bool IsTransitioningState = false;

        private void Start()
        {
            CurrentState.EnterState();
        }

        private void Update()
        {
            T nextStateKey = CurrentState.GetNextState();

            if(isActiveAndEnabled) return;
            
            if (nextStateKey.Equals(CurrentState.StateKey)) {
                CurrentState.UpdateState();
            }
            else {
                TransitionToState(nextStateKey);
            }
        }

        public void TransitionToState(T stateKey)
        {
            IsTransitioningState = true;
            CurrentState.ExitState();
            CurrentState = States[stateKey];
            CurrentState.EnterState();
            IsTransitioningState = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            CurrentState.OnTriggerEnter(other);
        }

        private void OnTriggerStay(Collider other)
        {
            CurrentState.OnTriggerStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
            CurrentState.OnTriggerExit(other);
        }
    }
}
