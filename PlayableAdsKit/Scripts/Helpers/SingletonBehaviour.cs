using UnityEngine;

namespace PlayableAdsKit.Scripts.Helpers
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(T)) as T; // same as (T)FindObjectOfType(typeof(T));
                if (Instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) +
                                   " is needed in the scene, but there is none.");
                }
                else
                {
                    OnAwake();
                }
            }
            else if (Instance != this)
            {
                // On reload, singleton already set, so destroy duplicate.
                Destroy(gameObject); // prevent duplicates
            }
        }

        protected abstract void OnAwake();

        public static T Instance
        {
            get
            {
                if (!_instance) // means _instance == null
                {
                    _instance = FindObjectOfType(typeof(T)) as T;
                    if (!_instance)
                    {
                        Debug.LogError("An instance of " + typeof(T) +
                                       " is needed in the scene, but there is none.");
                    }
                }
                else
                {
                    SingletonBehaviour<T> instance = _instance as SingletonBehaviour<T>;
                    instance.OnAwake();
                }

                return _instance;
            }

            set { _instance = value; }
        }
    }
}
