// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace Creative.Scripts.Alexa
{
    /// <summary>
    /// Ref : https://docs.unity3d.com/Manual/BlendShapes.html
    /// </summary>
    public enum AlexaBlendShapes
    {
        None,
        Smile,
        Sad,
        Suprize,
        Blink,
    }
    public class SetBlendShapeWeighExample : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera faceZoomCamera;
        private bool _isFaceZoomCameraEnabled=false;
        
        [Header("BlendShape Related")]
        [SerializeField] private SkinnedMeshRenderer alexaSkinnedMeshRenderer;
        [SerializeField] private float blendShapeSpeed = 1f;
        private AlexaBlendShapes _lastActivatedBlendShape = AlexaBlendShapes.None;

        private float _blendIncrease= 0f;
        private float _blendDecrease= 100f;
        
        private Dictionary<AlexaBlendShapes, int> _alexaBlendShapeDictionary = new Dictionary<AlexaBlendShapes, int>
        {
            {AlexaBlendShapes.None,0},
            {AlexaBlendShapes.Smile,1},
            {AlexaBlendShapes.Sad,2},
            {AlexaBlendShapes.Suprize,3},
            {AlexaBlendShapes.Blink,4}
        };
        

        private void Update()
        {
            #region Shortcuts

            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                _isFaceZoomCameraEnabled = !_isFaceZoomCameraEnabled;
                FaceZoomCameraSetter(_isFaceZoomCameraEnabled);
            }

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                IncreaseBlendShapeValue(_alexaBlendShapeDictionary[AlexaBlendShapes.Smile]);
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                IncreaseBlendShapeValue(_alexaBlendShapeDictionary[AlexaBlendShapes.Sad]);
            }
            
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                IncreaseBlendShapeValue(_alexaBlendShapeDictionary[AlexaBlendShapes.Suprize]);
            }
            
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                IncreaseBlendShapeValue(_alexaBlendShapeDictionary[AlexaBlendShapes.Blink]);
            }
            
            #endregion
            
        }

        private void FaceZoomCameraSetter(bool status)
        {
            faceZoomCamera.enabled = status;
        }
        
        private void IncreaseBlendShapeValue(int blendShapeIndex)
        {
            if (_lastActivatedBlendShape == _alexaBlendShapeDictionary.ElementAt(blendShapeIndex).Key)
            {
                DecreaseBlendShapeValue(_alexaBlendShapeDictionary[_lastActivatedBlendShape],true);
                return;
            }
            
            DecreaseBlendShapeValue(_alexaBlendShapeDictionary[_lastActivatedBlendShape]);
            
            _blendIncrease += blendShapeSpeed;
            alexaSkinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex,_blendIncrease);
            if (_blendIncrease < 100)
            {
                IncreaseBlendShapeValue(blendShapeIndex);
            }
            else
            {
                _lastActivatedBlendShape = _alexaBlendShapeDictionary.ElementAt(blendShapeIndex).Key;
                // Debug.Log($"Last activated blend shape: {_lastActivatedBlendShape}");
            }
            
        }

        private void DecreaseBlendShapeValue(int blendShapeIndex, bool isClear=false)
        {
            if( _lastActivatedBlendShape == AlexaBlendShapes.None) return;

            _blendDecrease -= blendShapeSpeed;
            alexaSkinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex,_blendDecrease);
            if (_blendDecrease > 0)
            {
                DecreaseBlendShapeValue(blendShapeIndex);
            }
            else if (isClear)
            {
                _lastActivatedBlendShape = AlexaBlendShapes.None;
                // Debug.Log($"Last activated blend shape: {_lastActivatedBlendShape}");
            }
            else 
            {
                _lastActivatedBlendShape = _alexaBlendShapeDictionary.ElementAt(blendShapeIndex).Key;
                // Debug.Log($"Last activated blend shape: {_lastActivatedBlendShape}");
            }
        }
        
    }
}
