using PlayableAdsKit.Scripts.PlaygroundConnections;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Editor
{
    public class Tutorial : KitButtonBase
    {
        private readonly string _prefabPath = "Assets/PlayableAdsKit/Prefabs/TutorialController.prefab";
        Tutorial()
        {
            Name = "Tutorial Controller";
            DescriptionText =
                "Imports Tutorial Controller that makes it easier for players to interact with the playable ad.";
        }
        
        protected override void RunAction()
        {
            GameObject playableTutorialController = null;
            
            if (FindObjectOfType<TutorialController>())
            {
                Debug.LogWarning("There is already a TutorialController exist in the scene!");
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
            
            if (GameObject.Find("TutorialController") != null)
            {
                playableTutorialController = GameObject.Find("TutorialController");
            
                if (playableTutorialController.TryGetComponent(out TutorialController tutorialController))
                {
                    return;
                }
                else
                {
                    playableTutorialController.AddComponent<TutorialController>();
                }
                return;
            }
            
            playableTutorialController = ImportPrefabIntoScene(_prefabPath,PlayableParentCanvas);

            SetComponentAsLastChild(playableTutorialController.GetComponent<RectTransform>());
            Debug.Log("Tutorial Controller successfully instantiated!");
            
        }
        
    }
}
