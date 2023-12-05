using PlayableAdsKit.Scripts.PlaygroundConnections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayableAdsKit.Scripts.Editor
{
    public class KitButtonBase : EditorWindow
    {
        public string Name = "Kit_Button";
        protected string RunButtonName = "Import";
        protected string DescriptionText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
        public readonly float ButtonHeight = 40f;
        public readonly float ButtonWidth = 160f;

        protected GameObject PlayableParentCanvas; 

        public void ShowDescription()
        {
            GUILayout.Label(Name, GetDefaultDescriptionTitleTextStyle());
            GUILayout.Space(10);
            DrawImportButton();
            GUILayout.Space(10);
            GUILayout.Label(DescriptionText, GetDefaultDescriptionTextStyle());
            GUILayout.Space(10);
            DrawDescriptionBody();
        }

        public virtual void DrawImportButton()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(RunButtonName, GetButtonTextStyle(),
                    GUILayout.Width(ButtonWidth),
                    GUILayout.Height(ButtonHeight)))
            {
                RunAction();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        protected virtual void DrawDescriptionBody()
        {
            // GUILayout.Label("More Info...", GetDefaultDescriptionBodyTextStyle());
        }

        protected virtual void RunAction(){ }

        private GUIStyle GetDefaultDescriptionTitleTextStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                wordWrap = true,
                normal = { textColor = new Color(196f / 255, 196f / 255, 196f / 255) }, // #585858
                fontStyle = FontStyle.Bold,
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter
            };
            
        }
        private GUIStyle GetDefaultDescriptionTextStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                normal = { textColor = new Color(88f / 255, 88f / 255, 88f / 255) }, // #585858
            };
        }
        
        private GUIStyle GetDefaultDescriptionBodyTextStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                wordWrap = true,
                alignment = TextAnchor.MiddleLeft,
                fontSize = 15,
                normal = { textColor = Color.gray }
            };
        }
        
        private GUIStyle GetButtonTextStyle()
        {
            return new GUIStyle(GUI.skin.button)
            {
                wordWrap = true,
            };
        }

        #region Helpers

        protected void GenerateCanvasPack()
        {
            PlayableParentCanvas = new GameObject("Canvas");
            PlayableParentCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var canvasScaler = PlayableParentCanvas.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1125, 2436);
            canvasScaler.matchWidthOrHeight =1f;
            PlayableParentCanvas.AddComponent<GraphicRaycaster>();
            Debug.Log("Playable Canvas successfully instantiated!");

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
            CheckForCTAComponent();
            
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, new UnityAction(CtaController.Instance.OpenStore));
#endif
            // In run time, unity event listeners can be added by following lines. 
            // button.onClick.AddListener(()=>CtaController.Instance.OpenStore());
            // button.onClick.AddListener(delegate { CtaController.Instance.OpenStore(); });
        }

        private bool IsGameObjectAlreadyExistInScene(string gameObjectName)
        {
            if (!GameObject.Find(gameObjectName)) return false;
            
            Debug.LogWarning(gameObjectName + " already exist in the scene!");
            return true;
        }
        
        protected void SetComponentAsLastChild(RectTransform focusObj)
        {
            if(PlayableParentCanvas == null) return;

            focusObj.SetAsLastSibling();
        }

        protected void SetComponentAsFirstChild(RectTransform focusObj)
        {
            if(PlayableParentCanvas == null) return;
            
            focusObj.SetAsFirstSibling();
        }

        protected void CheckForCTAComponent()
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
            }

            playableGameManager = new GameObject("PlayableGameManager");
            playableGameManager.AddComponent<CtaController>();
            playableGameManager.transform.position = Vector3.zero;
            Debug.Log("Playable Game Manager successfully instantiated!");
        }
        
        protected GameObject ImportPrefabIntoScene(string prefabPath, GameObject parent = null)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab == null)
            {
                Debug.LogError("Prefab not found at path: " + prefabPath);
                return null;
            }

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            if (instance == null)
            {
                Debug.LogError("Failed to instantiate prefab.");
                return null;
            }
            
            if (parent != null)
            {
                instance.transform.SetParent(parent.transform,false);
            }
            
            return instance;
        }

        #endregion

    }
}
