using PlayableAdsKit.Scripts.Base;
using UnityEditor;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Editor
{
    public class Audio : KitButtonBase
    {
        private readonly string _muteButtonPrefabPath = "Assets/PlayableAdsKit/Prefabs/MuteButtonCanvas.prefab"; 
        private bool _muteToggle = false;
        
        Audio()
        {
            Name = "Audio Manager";
            DescriptionText = "Imports an Audio Manager to edit game sounds and effects.";
        }

        protected override void RunAction()
        {
            GameObject playableAudioManager;
            if (FindObjectOfType<AudioManager>())
            {
                Debug.LogWarning("There is already a AudioManager exist in the scene!");
                return;
            }

            if (GameObject.Find("AudioManager") != null)
            {
                playableAudioManager = GameObject.Find("AudioManager");
                if (playableAudioManager.TryGetComponent(out AudioManager audioManager))
                {
                    return;
                }
                else
                {
                    playableAudioManager.AddComponent<AudioManager>();
                    playableAudioManager.transform.position = Vector3.zero;
                }
                return;
            }

            playableAudioManager = new GameObject("AudioManager");
            playableAudioManager.AddComponent<AudioManager>();
            playableAudioManager.transform.position = Vector3.zero;

            if (_muteToggle)
            {
                ImportPrefabIntoScene(_muteButtonPrefabPath);
            }
                
            Debug.Log("Playable Audio Manager successfully instantiated!");
        }

        protected override void DrawDescriptionBody()
        {
            GUILayout.BeginVertical("box");
            _muteToggle = EditorGUILayout.Toggle("Include Mute Button", _muteToggle);
            GUILayout.EndVertical();
        }
    }
}
