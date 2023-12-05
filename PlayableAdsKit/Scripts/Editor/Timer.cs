using PlayableAdsKit.Scripts.Base;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Editor
{
    public class Timer : KitButtonBase
    {
        private readonly string _prefabPath = "Assets/PlayableAdsKit/Prefabs/TimerController.prefab";
        Timer()
        {
            Name = "Timer Controller";
            DescriptionText = "Imports default time counter with controllable features.";
        }
        
        protected override void RunAction()
        {
            GameObject playableTimerController = null;
            
            if (FindObjectOfType<TimerController>())
            {
                Debug.LogWarning("There is already a TimerController exist in the scene!");
                return;
            }
            
            if (GameObject.Find("Canvas") == null)
            {
                GenerateCanvasPack();
            }
            else
            {
                PlayableParentCanvas = GameObject.Find("Canvas");
            }
            
            if (GameObject.Find("TimerController") != null)
            {
                playableTimerController = GameObject.Find("TimerController");
            
                if (playableTimerController.TryGetComponent(out TimerController timerController))
                {
                    return;
                }
                else
                {
                    playableTimerController.AddComponent<TimerController>();
                }
                return;
            }
            
            ImportPrefabIntoScene(_prefabPath,PlayableParentCanvas);
            
            Debug.Log("Timer Controller successfully instantiated!");
        }
    }
}
