using System;
using Game.Scripts.Behaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Playable
{
    public class TextChanger : MonoBehaviour
    {
        
        // Added for Playable Version2
        //
        [LunaPlaygroundField("Tutorial text1", 0, "Text settings")] [SerializeField]
        private string _firstText;
        
        [LunaPlaygroundField("Tutorial text2", 1, "Text settings")] [SerializeField]
        private string _secondText;

        [LunaPlaygroundField("Sound mute", 0, "Sound settings")] [SerializeField]
        private bool _muteOption;
       
        

        [SerializeField] private SlingBehaviour _slingBehaviour;
        [SerializeField] private PlaneBehaviour _planeBehaviour;
        [SerializeField] private SoundManager _soundManager;
        
        //

        private void Awake()
        {
            // Added for Playable Version2
            //
            _slingBehaviour.atStartText = _firstText;
            _planeBehaviour.onFlightText = _secondText;

            _soundManager.muted = _muteOption;
            //
        }
    }
}