using System.Collections;
using RocketCoroutine;
using UnityEngine;

namespace Game.Scripts.Playable
{
    public class EndCardController : MonoBehaviour
    {
        [LunaPlaygroundField("Open store after end card shown", 0, "End card")] [SerializeField]
        private bool _openStoreAfterEndCard;
        
        private static Transform _transform;
        private static bool _openStore;

        private static bool _storeOpened;
        
        private void Awake()
        {
            _transform = transform;
            _openStore = _openStoreAfterEndCard;
            _storeOpened = false;
        }

        public static void Enable()
        {
            foreach (Transform child in _transform)
            {
                child.gameObject.SetActive(true);
            }
            
            
            if (_openStore)
            {
                if (!_storeOpened)
                {
                    CoroutineController.DoAfterGivenTime(1f,CtaController.OpenStoreStatic);
                    _storeOpened = true;
                }
            }
        }
    }
}