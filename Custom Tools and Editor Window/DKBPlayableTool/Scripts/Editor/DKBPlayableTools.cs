// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using CpiTemplate.Game.Playable.Scripts.PlayableConnections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Important to import for tools

namespace Custom_Tools_And_Editor_Window.DKBPlayableTool.Scripts.Editor
{
    /// <summary>
    /// TOOLS OPTION'S FUNCTIONS HAS TO BE 'STATIC' !!!
    /// </summary>
    public class DKBPlayableTools : EditorWindow
    {
        private static GameObject _playableGameManager;
        private static GameObject _endCardConnectionsObj;
        private static GameObject _endCardParentCanvas;

        [MenuItem("Tools/Dkbozkurt/PlayableTool")]
        public static void ShowWindow()
        {
            GetWindow<DKBPlayableTools>("Dkbozkurt Playable Tool");
        }

        private void OnGUI()
        {
            UIButtons();
        }

        private void UIButtons()
        {
            GUILayout.Label("CTA Controller",EditorStyles.boldLabel);
            if (GUILayout.Button("Import CtaController"))
            {
                
                CallCtaController();
            }

            GUILayout.Space(5);
            
            GUILayout.Label("EndCard Controller",EditorStyles.boldLabel);
            if (GUILayout.Button("Import EndCardController"))
            {
                CallEndCardController();
            }
        }

        private void CallCtaController()
        {
            if (GameObject.Find("PlayableGameManager") != null)
            {
                _playableGameManager = GameObject.Find("PlayableGameManager");

                if (_playableGameManager.TryGetComponent(out CtaController ctaController))
                {
                    return;
                }
                else
                {
                    _playableGameManager.AddComponent<CtaController>();
                    _playableGameManager.transform.position = Vector3.zero;
                }
                return;
            }
            
            _playableGameManager = new GameObject("PlayableGameManager");
            _playableGameManager.AddComponent<CtaController>();
            _playableGameManager.transform.position = Vector3.zero;
            Debug.Log("Playable Game Manager successfully instantiated!");
        }

        private void CallEndCardController()
        {
            if (GameObject.Find("EndCardController")) return;

            if (GameObject.Find("Canvas") == null)
            {
                GenerateCanvasPack();
            }
            else
            {
                _endCardParentCanvas = GameObject.Find("Canvas");
            }

            #region EndCardController

            _endCardConnectionsObj = GenerateUIObject("EndCardController",_endCardParentCanvas.transform);
            var endCardController = _endCardConnectionsObj.AddComponent<EndCardController>();
            _endCardConnectionsObj.GetComponent<RectTransform>().localPosition = Vector3.zero;

            #endregion

            #region EndCard Background

            GameObject endCardBackground = GenerateUIObject("EndCardBackground",_endCardConnectionsObj.transform);
            AddImageComponent(endCardBackground);
            endCardBackground.GetComponent<Image>().raycastTarget = true;
            var endCardButton = endCardBackground.AddComponent<Button>();
            endCardButton.transition = Selectable.Transition.None;
            // TODO burada cta controllerdan store a baglanmayi ayarlar.
            // TODO ayrica pivotlari tum ekrana kaplat.
            endCardController.EndCardBackground = endCardBackground.GetComponent<Image>();
            endCardBackground.SetActive(false);

            #endregion

            #region EndCard Icon

            GameObject endCardIcon = GenerateUIObject("EndCardIcon", endCardBackground.transform);
            AddImageComponent(endCardIcon);
            endCardController.EndCardIcon = endCardIcon.GetComponent<Image>();

            #endregion

            #region EndCard Text

            GameObject endCardText = GenerateUIObject("EndCardText", endCardBackground.transform);
            AddImageComponent(endCardText);
            endCardController.EndCardText = endCardText.GetComponent<Image>();

            #endregion

            #region EndCard PlayButton

            GameObject endCardPlayButton = GenerateUIObject("EndCardPlayButton", endCardBackground.transform);
            AddImageComponent(endCardPlayButton);
            endCardController.EndCardPlayButton = endCardPlayButton.GetComponent<Image>();

            #endregion
        }

        private void GenerateCanvasPack()
        {
            _endCardParentCanvas = new GameObject("Canvas");
            _endCardParentCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var canvasScaler = _endCardParentCanvas.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1125, 2436);
            canvasScaler.matchWidthOrHeight =1f;
            _endCardParentCanvas.AddComponent<GraphicRaycaster>();

            if(GameObject.Find("Event System")) return;
            
            var eventSystem = new GameObject("Event System");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        private GameObject GenerateUIObject(string name,Transform parent= null)
        {
            var obj = new GameObject(name);
            obj.AddComponent<RectTransform>();
            if(parent != null) obj.transform.SetParent(parent);
            return obj;
        }

        private void AddImageComponent(GameObject targetObj)
        {
            var image = targetObj.AddComponent<Image>();
            image.raycastTarget = false;
        }
        
    }
}
