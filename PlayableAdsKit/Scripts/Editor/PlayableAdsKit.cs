using System;
using PlayableAdsKit.Scripts.Base;
using UnityEditor;
using UnityEngine;

namespace PlayableAdsKit.Scripts.Editor
{
    public class PlayableAdsKit : EditorWindow
    {
        private static readonly string _toolTitle = "PlayableAdsKit";
        private static readonly string _logoPath_128x32 = "Assets/PlayableAdsKit/Textures/Editor/logo_128x32.png";
        private static readonly string _logoPath_64x16 = "Assets/PlayableAdsKit/Textures/Editor/logo_64x16.png";
        private static readonly string _footerText = "©️Justdice";
        private static readonly string _helperurl =
            "https://justdice.atlassian.net/wiki/spaces/DT/pages/36372513/Welcome+Design";
        
        private static PlayableAdsKit _window;
        private static Texture2D _logo_128x32;
        private static Texture2D _logo_64x16;
        private static readonly Vector2 _minWindowSize = new Vector2(400, 600);
        private static readonly float _windowMargin = 5f;
        private static readonly float _seperatorWidth = 2f;
        private static readonly float _seperatorMargin = 20f;
        private static readonly float _buttonAreaSizeMultiplier = 0.4f;
        private static float _descriptionAreaSizeMultiplier => 1f - _buttonAreaSizeMultiplier;
        private static readonly string _defaultDescriptionTitleText = "Playable Ads Kit";
        private static readonly string _defaultDescriptionSubTitleText = "Assistant for Unity Developers in Playable Ad Creation. Designed to efficiently enhance and expedite the creation of engaging playable ads."; 

        private KitButtonBase[] _kitButtons;
        private KitButtonBase _selectedKitButton;

        [MenuItem("Tools/JustDice/PlayableAdsKit %F1",priority = 300)]
        public static void ShowWindow()
        {
            _window = GetWindow<PlayableAdsKit>(typeof(SceneView));
            _window.titleContent = new GUIContent();
            SetEditorIconAndTitle();
            _window.Show();
        }

        private void OnEnable()
        {
            minSize = _minWindowSize;
            _logo_128x32 = AssetDatabase.LoadAssetAtPath<Texture2D>(_logoPath_128x32);
            _logo_64x16 = AssetDatabase.LoadAssetAtPath<Texture2D>(_logoPath_64x16);
            SetButtons();   
        }
        
        private static void SetEditorIconAndTitle()
        {
            _window.titleContent = new GUIContent(_toolTitle,_logo_64x16, 
                "Helpful tool for developing playable ads by using LunaLabs");
        }

        private void OnDisable()
        {
            DestroyButtonPanels();    
        }
        
        private void SetButtons()
        {
            _kitButtons = new KitButtonBase[10];
            _kitButtons[0] = ScriptableObject.CreateInstance<DefaultFolderHierarchy>();
            _kitButtons[1] = ScriptableObject.CreateInstance<CTA>();
            _kitButtons[2] = ScriptableObject.CreateInstance<PlayableCanvas>();
            _kitButtons[3] = ScriptableObject.CreateInstance<Banner>();
            _kitButtons[4] = ScriptableObject.CreateInstance<EndCard>();
            _kitButtons[5] = ScriptableObject.CreateInstance<Tutorial>();
            _kitButtons[6] = ScriptableObject.CreateInstance<Audio>();
            _kitButtons[7] = ScriptableObject.CreateInstance<Timer>();
            _kitButtons[8] = ScriptableObject.CreateInstance<ObjectPool>();
            _kitButtons[9] = ScriptableObject.CreateInstance<UnityPackages>();
        }
        
        private void DestroyButtonPanels()
        {
            foreach (var button in _kitButtons)
            {
                if (button != null)
                {
                    ScriptableObject.DestroyImmediate(button);
                }
            }
        }
        
        private void OnGUI()
        {
            if (_kitButtons == null || _kitButtons.Length == 0) return;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            GUILayout.Label("Elements", GetTitleStyle());
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            DrawHelperButton();
            
            DrawButtonColumns();
            
            GUILayout.Space(5);
            
            GUILayout.Label("Description", GetTitleStyle());
            if (_selectedKitButton != null)
            {
                _selectedKitButton.ShowDescription();
            }
            else
            {
                DefaultDescription();
            }

            DrawFooter();
        }

        private void DrawButtonColumns()
        {
            float buttonWidth = position.width / 2;

            int halfLength = Mathf.CeilToInt(_kitButtons.Length / 2f);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            for (int i = 0; i < halfLength; i++)
            {
                var targetButton = _kitButtons[i];
                if (GUILayout.Button(targetButton.Name, GetButtonTextStyle(), GUILayout.Width(buttonWidth), GUILayout.Height(targetButton.ButtonHeight)))
                {
                    _selectedKitButton = _kitButtons[i];
                }
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            for (int i = halfLength; i < _kitButtons.Length; i++)
            {
                var targetButton = _kitButtons[i];
                if (GUILayout.Button(targetButton.Name, GetButtonTextStyle(), GUILayout.Width(buttonWidth), GUILayout.Height(targetButton.ButtonHeight)))
                {
                    _selectedKitButton = _kitButtons[i];
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }


        private void DrawHelperButton()
        {
            GUILayout.BeginArea(new Rect(position.width - 30, 0, 30, 30));
            GUIContent helpContent = EditorGUIUtility.IconContent("_Help");
            helpContent.tooltip = "Click here for help";
            if (GUILayout.Button(helpContent, GUILayout.Width(30), GUILayout.Height(30)))
            {
                Application.OpenURL(_helperurl);
                Debug.Log("Help icon clicked, opening URL: " + _helperurl);
            }
            GUILayout.EndArea();
        }

        private void DrawFooter()
        {
            GUILayout.FlexibleSpace();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            // GUILayout.Label(_signature, GetFooterSignatureStyle());
            GUILayout.Label(_footerText, GetFooterStyle());
            GUILayout.EndHorizontal();
        }

        private void DefaultDescription()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GetLogo();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(_defaultDescriptionTitleText, GetDefaultDescriptionTitleStyle());
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(_defaultDescriptionSubTitleText, GetDefaultDescriptionSubTitleStyle());
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }

        private void GetLogo(float targetWidth=128f, float targetHeight=32f)
        {
            GUIStyle textureStyle = new GUIStyle { normal = { background = _logo_128x32 } };
            GUILayout.Label(GUIContent.none, textureStyle, GUILayout.Width(targetWidth), GUILayout.Height(targetHeight));
        }

        #region GUI Text Styles

        private GUIStyle GetTitleStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = new Color(88f / 255, 88f / 255, 88f / 255) }, // #585858
                fontStyle = FontStyle.Bold,
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter
            };
        }
        
        private GUIStyle GetButtonTextStyle()
        {
            return new GUIStyle(GUI.skin.button)
            {
                wordWrap = true,
            };
        }

        private GUIStyle GetFooterStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = Color.gray }
            };
        }
        
        private GUIStyle GetDefaultDescriptionTitleStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                wordWrap = true,
                normal = { textColor = new Color(196f / 255, 196f / 255, 196f / 255) }, // #585858
                fontStyle = FontStyle.Bold,
                fontSize = 25,
                alignment = TextAnchor.MiddleCenter
            };
        }
        
        private GUIStyle GetDefaultDescriptionSubTitleStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                normal = { textColor = new Color(88f / 255, 88f / 255, 88f / 255) }, // #585858
            };
        }

        private static readonly string _signature = "@dkbozkurt";
        private GUIStyle GetFooterSignatureStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = new Color(56f / 255, 56f / 255, 56f / 255) } // #383838
            };
        }

        #endregion
        
    }
}
