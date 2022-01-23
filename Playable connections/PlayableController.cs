// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using Game.Scripts.Behaviours;
using Game.Scripts.Controllers;
using RocketCoroutine;
using UnityEngine;

namespace Game.Scripts.Playable
{
    public class PlayableController : MonoBehaviour
    {
        [LunaPlaygroundField("Auto play after seconds", 0, "Game Settings")] [SerializeField]
        private int _autoPlayAfterSeconds;

        [SerializeField] private SlingBehaviour _slingBehaviour;
        
        private void Awake()
        {
            CoroutineController.DoAfterGivenTime(_autoPlayAfterSeconds,AutoPlay);
        }

        private void AutoPlay()
        {
            if (_slingBehaviour.Released) 
                return;
            
            _slingBehaviour.ReleaseSuccess();
            TutorialController.Disable();
        }
    }
}