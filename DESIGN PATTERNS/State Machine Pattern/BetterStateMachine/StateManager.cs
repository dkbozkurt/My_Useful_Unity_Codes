// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DESIGN_PATTERNS.State_Machine_Pattern.BetterStateMachine
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=qsIiFsddGV4
    /// </summary>

    public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
    {
        protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

        protected BaseState<EState> CurrentState;

        protected bool IsTransitioningState = false; 
        private void Start()
        {
            CurrentState.EnterState();
        }

        private void Update()
        {
            EState nextStateKey = CurrentState.GetNextState();

            if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey)){
                CurrentState.UpdateState();
            }else if(!IsTransitioningState) {
                TransitionToState(nextStateKey);
            }
        }

        public void TransitionToState(EState stateKey)
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
