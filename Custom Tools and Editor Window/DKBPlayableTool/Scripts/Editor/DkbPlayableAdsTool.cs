// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using CpiTemplate.Game.Playable.Scripts.PlayableConnections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Important to import for tools

namespace Custom_Tools_And_Editor_Window.DKBPlayableTool.Scripts.Editor
{
    /// <summary>
    /// TOOLS OPTION'S FUNCTIONS HAS TO BE 'STATIC' !!!
    /// </summary>
    public class DkbPlayableAdsTool : EditorWindow
    {
        private static GameObject _playableGameManager;
        private static GameObject _endCardConnectionsObj;
        private static GameObject _endCardParentCanvas;

        private Button _endCardButton;

        [MenuItem("Tools/Dkbozkurt/PlayableAdsTool")]
        public static void ShowWindow()
        {
            GetWindow<DKBPlayableTools>("Dkbozkurt Playable Ads Tool");
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
            var endCardConnectionsRectTransform = _endCardConnectionsObj.GetComponent<RectTransform>();
            endCardConnectionsRectTransform.anchorMin = Vector2.zero;
            endCardConnectionsRectTransform.anchorMax = Vector2.one;
            endCardConnectionsRectTransform.pivot = new Vector2(0.5f,0.5f);
            endCardConnectionsRectTransform.offsetMax = new Vector2(0, 0);
            endCardConnectionsRectTransform.offsetMin = new Vector2(0, 0);
            
            #endregion

            #region EndCard Background

            GameObject endCardBackground = GenerateUIObject("EndCardBackground",_endCardConnectionsObj.transform);
            AddImageComponent(endCardBackground);
            var endCardBackgroundRectTransform = endCardBackground.GetComponent<RectTransform>();
            endCardBackgroundRectTransform.anchorMin = Vector2.zero;
            endCardBackgroundRectTransform.anchorMax = Vector2.one;
            endCardBackgroundRectTransform.pivot = new Vector2(0.5f,0.5f);
            endCardBackgroundRectTransform.offsetMax = new Vector2(0, 0);
            endCardBackgroundRectTransform.offsetMin = new Vector2(0, 0);

            var endCardBackgroundImage = endCardBackground.GetComponent<Image>();
            endCardBackgroundImage.raycastTarget = true;
            endCardBackgroundImage.color = new Color(0f, 0f, 0f, 0.75f);

            endCardBackground.AddComponent<Button>();
            _endCardButton = endCardBackground.GetComponent<Button>();
            _endCardButton.transition = Selectable.Transition.None;
            
            AddStoreConnectionOntoButton(_endCardButton);

            endCardController.EndCardBackground = endCardBackgroundImage;
            endCardBackground.SetActive(false);

            #endregion

            #region EndCard Icon

            GameObject endCardIcon = GenerateUIObject("EndCardIcon", endCardBackground.transform);
            AddImageComponent(endCardIcon);
            endCardController.EndCardIcon = endCardIcon.GetComponent<Image>();
            LocateRectTransform(endCardIcon.GetComponent<RectTransform>(), new Vector2(0f,650f),new Vector2(650f,650f));

            #endregion

            #region EndCard Text

            GameObject endCardText = GenerateUIObject("EndCardText", endCardBackground.transform);
            AddImageComponent(endCardText);
            endCardController.EndCardText = endCardText.GetComponent<Image>();
            LocateRectTransform(endCardText.GetComponent<RectTransform>(),new Vector2(0f,-150f),new Vector2(1000f,400f));

            #endregion

            #region EndCard PlayButton

            GameObject endCardPlayButton = GenerateUIObject("EndCardPlayButton", endCardBackground.transform);
            AddImageComponent(endCardPlayButton);
            endCardController.EndCardPlayButton = endCardPlayButton.GetComponent<Image>();
            LocateRectTransform(endCardPlayButton.GetComponent<RectTransform>(),new Vector2(0f,-800f),new Vector2(756f,300f));

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

        private void LocateRectTransform(RectTransform rectTransform,Vector2 location,Vector2 size)
        {
            rectTransform.anchoredPosition = location;
            rectTransform.sizeDelta = size;
        }

        private void AddStoreConnectionOntoButton(Button button)
        {
            CallCtaController();
            
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, new UnityAction(CtaController.Instance.OpenStore));
#endif
            // In run time, unity event listeners can be added by following lines. 
            // button.onClick.AddListener(()=>CtaController.Instance.OpenStore());
            // button.onClick.AddListener(delegate { CtaController.Instance.OpenStore(); });
        }
    }
}
