// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace ImportantSnackScripts.Animation_Transition
{
    /// <summary>
    /// CrossFade
    /// 
    /// Ref : https://www.youtube.com/watch?v=ZwLekxsSY3Y
    /// </summary>

    public class AnimationCrossFade : MonoBehaviour
    {
        private Animator _animator;
        
        #region int Hashing

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Attack= Animator.StringToHash("Attack");
        
        #endregion

        [SerializeField] private float attackAnimDuration;
        
        private bool _attacking;
        private bool _jumping;
        private bool _grounded;
        
        private int _currentState;
        private float _lockedTill;
        
        // Gotta come from controller.
        private Vector2 _input;
        
        private void Awake()
        {
            GetComponent<Animator>();
        }

        private void Update()
        {
            var state = GetState();
            
            if(state == _currentState) return;

            AnimateWithCrossFade(state, 0.3f, 0);
            _currentState = state;
        }

        private int GetState()
        {
            if (Time.time < _lockedTill) return _currentState;
            
            // Priorities
            if (_attacking) return LockState(Attack, attackAnimDuration);
            if (_jumping) return Jump;
            if (_grounded) return _input.x== 0 ? Idle : Walk;
            return Jump;
            
            int LockState(int s, float t)
            {
                _lockedTill = Time.time + t;
                return s;
            }
        }
        
        private void AnimateWithCrossFade(string stateName,float transitionTime,int animationLayer=0)
        {
            _animator.CrossFade(stateName,transitionTime,animationLayer);
        }
        
        private void AnimateWithCrossFade(int stateHash,float transitionTime,int animationLayer=0)
        {
            _animator.CrossFade(stateHash,transitionTime,animationLayer);
        }


    }
}
