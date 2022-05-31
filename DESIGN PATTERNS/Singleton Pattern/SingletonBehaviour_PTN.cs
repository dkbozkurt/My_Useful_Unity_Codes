using UnityEngine;

namespace CpiTemplate.Game.Scripts
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected bool DrawDebugLabel;
        private static T _instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)FindObjectOfType(typeof(T));
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
                Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
            }
        }        

        protected abstract void OnAwake();

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (!_instance)
                    {
                        Debug.LogError("An instance of " + typeof(T) +
                                       " is needed in the scene, but there is none.");
                    }
                    else
                    {
                        SingletonBehaviour<T> instance = _instance as SingletonBehaviour<T>;
                        instance.OnAwake();
                    }
                }

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

#if ROC_DEBUG_MODE || ROC_LOG_MODE
        void OnGUI()
        {
            if (!DrawDebugLabel) return;

            string labelString = "Rocket Log";

#if ROC_DEBUG_MODE
            labelString = "Rocket Debug";
#endif

            DrawOutline(new Rect(35, Screen.height - 130, 300, 80), labelString, 2, new GUIStyle
            {
                fontSize = 24
            });
            GUI.Label(new Rect(35, Screen.height - 130, 300, 80), labelString, new GUIStyle
            {
                normal = { textColor = Color.white },
                fontSize = 24
            });
        }

        void DrawOutline(Rect r, string t, int strength, GUIStyle style)
        {
            GUI.color = new Color(0, 0, 0, 1);
            int i;
            for (i = -strength; i <= strength; i++)
            {
                GUI.Label(new Rect(r.x - strength, r.y + i, r.width, r.height), t, style);
                GUI.Label(new Rect(r.x + strength, r.y + i, r.width, r.height), t, style);
            }
            for (i = -strength + 1; i <= strength - 1; i++)
            {
                GUI.Label(new Rect(r.x + i, r.y - strength, r.width, r.height), t, style);
                GUI.Label(new Rect(r.x + i, r.y + strength, r.width, r.height), t, style);
            }
            GUI.color = new Color(1, 1, 1, 1);
        }
#endif
    }
}