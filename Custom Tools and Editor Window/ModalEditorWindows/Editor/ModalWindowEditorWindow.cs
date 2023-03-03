// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEditor;
using UnityEngine;

namespace Custom_Tools_And_Editor_Window.ModalEditorWindows.Editor
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=mCKeSNdO_S0&ab_channel=GameDevGuide
    /// </summary>
    public class ModalWindowEditorWindow : EditorWindow
    {
        public string choiceResult = "No Choice";
        
        [MenuItem("Tools/Modal Window Editor")]
        public static void Init()
        {
            ModalWindowEditorWindow window = GetWindow<ModalWindowEditorWindow>(typeof(SceneView));
            window.titleContent = new GUIContent("Modal Window Test");
        }

        public void OnGUI()
        {
            if (GUILayout.Button(new GUIContent("Get The Test Results")))
            {
                choiceResult = ModalWindowExample.Open();
            }
            
            EditorGUILayout.LabelField(choiceResult);
        }
        
    }
}
