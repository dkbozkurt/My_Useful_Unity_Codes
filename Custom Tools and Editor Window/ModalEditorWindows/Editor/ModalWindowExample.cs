// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEditor;
using UnityEngine;

namespace Custom_Tools_And_Editor_Window.ModalEditorWindows.Editor
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=mCKeSNdO_S0&ab_channel=GameDevGuide
    /// </summary>
    public class ModalWindowExample : EditorWindow
    {
        public string choice = "Nothing";
        public static string Open()
        {
            ModalWindowExample example = CreateInstance<ModalWindowExample>();
            example.ShowModal();
            return example.choice;
        }

        public void OnGUI()
        {
            if (GUILayout.Button("Choice A"))
            {
                choice = "Choice A";
                Close();
            }
            
            if (GUILayout.Button("Choice B"))
            {
                choice = "Choice B";
                Close();
            }
            
            if (GUILayout.Button("Choice C"))
            {
                choice = "Choice C";
                Close();
            }
        }
    }
}
