using UnityEngine;

namespace PlayableAdsKit.Scripts.Editor
{
    public class PlayableCanvas : KitButtonBase
    {
        PlayableCanvas()
        {
            Name = "Playable Base Canvas";
            DescriptionText = "Imports the base canvas for playable ad components.";
        }

        protected override void RunAction()
        {
            if (GameObject.Find("Canvas") == null)
            {
                GenerateCanvasPack();
            }
        }
    }
}
