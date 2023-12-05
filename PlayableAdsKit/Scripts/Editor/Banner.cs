using PlayableAdsKit.Scripts.PlaygroundConnections;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Editor
{
    public class Banner : KitButtonBase
    {
        private readonly string _prefabPath = "Assets/PlayableAdsKit/Prefabs/BannerController.prefab";
        Banner()
        {
            Name = "Banner Controller";
            DescriptionText = "Imports default banner";
        }
        
        protected override void RunAction()
        {
            GameObject playableBannerManager = null;
            
            if (FindObjectOfType<BannerController>())
            {
                Debug.LogWarning("There is already a BannerController exist in the scene!");
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
            
            if (GameObject.Find("BannerController") != null)
            {
                playableBannerManager = GameObject.Find("BannerController");
            
                if (playableBannerManager.TryGetComponent(out BannerController bannerController))
                {
                    return;
                }
                else
                {
                    playableBannerManager.AddComponent<BannerController>();
                }
                return;
            }
            
            playableBannerManager = ImportPrefabIntoScene(_prefabPath,PlayableParentCanvas);

            SetComponentAsLastChild(playableBannerManager.GetComponent<RectTransform>());
            Debug.Log("Banner Controller successfully instantiated!");
            
        }
    }
}
