// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEditor;
using UnityEngine;

namespace Custom_Tools_And_Editor_Window.EasyEditorWindowWithSerializedProperties.Editor
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=c_3DXBrH-Is&ab_channel=GameDevGuide
    /// </summary>
    public class GameDataObjectEditorWindow : ExtendedEditorWindow
    {
        private bool _integers = true;
        private bool _strings = false;
        public static void Open(GameDataObject dataObject)
        {
            GameDataObjectEditorWindow window = GetWindow<GameDataObjectEditorWindow>("Game Data Editor");
            window.serializedObject = new SerializedObject(dataObject);
        }

        private void OnGUI() 
        {
            currentProperty = serializedObject.FindProperty("GameData");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
            
            DrawSidebar(currentProperty);
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("box",GUILayout.ExpandHeight(true));

            if (selectedProperty != null)
            {
                DrawSelectedPropertiesPanel();
            }
            else
            {
                EditorGUILayout.LabelField("Select and item for the list");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            Apply();
        }

        private void DrawSelectedPropertiesPanel()
        {
            currentProperty = selectedProperty;
            
            // All will be drawn without Categorizing;
            // DrawProperties(currentProperty,true);

             EditorGUILayout.BeginHorizontal("box");
             DrawField("Header",true);
             EditorGUILayout.EndHorizontal();
             
             EditorGUILayout.BeginHorizontal("box");

             if (GUILayout.Button("Integers", EditorStyles.toolbarButton))
             {
                 _integers = true;
                 _strings = false;
             }
             if (GUILayout.Button("Strings", EditorStyles.toolbarButton))
             {
                 _integers = false;
                 _strings = true;
             }

             EditorGUILayout.EndHorizontal();

             if (_integers)
             {
                 EditorGUILayout.BeginVertical("box");
                 DrawField("TestInt1",true);
                 DrawField("TestInt2",true);
                 DrawField("TestInt3",true);
                 
                 EditorGUILayout.EndVertical();
             }

             if (_strings)
             {
                 EditorGUILayout.BeginVertical("box");
                 DrawField("TestString1",true);
                 DrawField("TestString2",true);
                 EditorGUILayout.EndVertical();
             }
        }
    }
}
