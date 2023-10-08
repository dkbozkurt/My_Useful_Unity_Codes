// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace DESIGN_PATTERNS.State_Machine_Pattern.BetterStateMachine
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=qsIiFsddGV4
    /// </summary>

    public class PlayerStateMachine : StateManager<PlayerStateMachine.PlayerState>
    {
        public enum PlayerState
        {
            Idle,
            Walk,
            Run
        }

        private void Awake()
        {
            CurrentState = States[PlayerState.Idle];
        }
    }
}
