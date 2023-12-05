using PlayableAdsKit.Scripts.PlaygroundConnections;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Editor
{
    public class EndCard : KitButtonBase
    {
        private readonly string _prefabPath = "Assets/PlayableAdsKit/Prefabs/EndCardController.prefab";
        EndCard()
        {
            Name = "EndCard Controller";
            DescriptionText = "Imports the EndCard Controller, which directs players to the store and has CTA links.";
        }
        
        protected override void RunAction()
        {
            GameObject playableEndCardController = null;
            
            if (FindObjectOfType<EndCardController>())
            {
                Debug.LogWarning("There is already a EndCardController exist in the scene!");
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
            
            if (GameObject.Find("EndCardController") != null)
            {
                playableEndCardController = GameObject.Find("EndCardController");
            
                if (playableEndCardController.TryGetComponent(out EndCardController endCardController))
                {
                    return;
                }
                else
                {
                    playableEndCardController.AddComponent<EndCardController>();
                }
                return;
            }
            
            playableEndCardController = ImportPrefabIntoScene(_prefabPath,PlayableParentCanvas);

            SetComponentAsLastChild(playableEndCardController.GetComponent<RectTransform>());
            Debug.Log("EndCard Controller successfully instantiated!");
            
        }
        
    }
}
