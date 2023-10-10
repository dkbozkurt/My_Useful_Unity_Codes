// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace DESIGN_PATTERNS.State_Machine_Pattern.BetterStateMachine
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=qsIiFsddGV4
    /// </summary>

    public abstract class BaseState<EState> where EState : Enum
    {
        public BaseState(EState key)
        {
            StateKey = key;
        }
        
        public EState StateKey { get; private set; }
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
        public abstract EState GetNextState();
        public abstract void OnTriggerEnter(Collider other);
        public abstract void OnTriggerStay(Collider other);
        public abstract void OnTriggerExit(Collider other);
    }
}
