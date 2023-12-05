using PlayableAdsKit.Scripts.PlaygroundConnections;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Editor
{
    public class CTA : KitButtonBase
    {
        CTA()
        {
            Name = "CTA Controller";
            DescriptionText = "Imports Playable Game Manager with CTA Controller that has connections to store link!";
        }

        protected override void RunAction()
        {
            GameObject playableGameManager;
            if (FindObjectOfType<CtaController>())
            {
                Debug.LogWarning("There is already a CtaController exist in the scene!");
                return;
            }
            if (GameObject.Find("PlayableGameManager") != null)
            {
                playableGameManager = GameObject.Find("PlayableGameManager");
            
                if (playableGameManager.TryGetComponent(out CtaController ctaController))
                {
                    return;
                }
                else
                {
                    playableGameManager.AddComponent<CtaController>();
                    playableGameManager.transform.position = Vector3.zero;
                }
                return;
            }

            playableGameManager = new GameObject("PlayableGameManager");
            playableGameManager.AddComponent<CtaController>();
            playableGameManager.transform.position = Vector3.zero;
            Debug.Log("Playable Game Manager successfully instantiated!");
        }
        
    }
}
