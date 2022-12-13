// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEditor;
using UnityEngine;

namespace Custom_Tools_And_Editor_Window.ColorizerTool.Scripts.Editor
{
    /// <summary>
    ///
    /// IMPORTANT: This script must sit under "Editor" named folder. To not include into the game.
    /// 
    /// Note:
    /// We use EditorGUILayout whenever we want to edit fields and properties.
    /// We use GUILayout whenever we want to create labels, spaces between properties and buttons.
    ///
    /// Pref: https://www.youtube.com/watch?v=491TSNwXTIg&ab_channel=Brackeys
    /// </summary>

// We have to inherit from "EditorWindow" instead of MonoBehaviour
    public class ColorizerTool : EditorWindow
    {
        // #region Variables
        //
        // private string myString = "Hello World!";
        //
        // #endregion
        //
        // // This attribute creates a menu item at the ("x") location.
        // [MenuItem("Window/Example Extension")]
        // public static void ShowWindow()
        // {
        //     // This method will open a window with a title for us, if it is not instantiated already. 
        //     EditorWindow.GetWindow<ExampleWindow>("Example Title");
        // }
        //
        // // Window Code
        // private void OnGUI()
        // {
        //     // A class that has functions for ui elements and automatically layout everything.
        //     GUILayout.Label("This is a Label.", EditorStyles.boldLabel);
        //
        //     myString = EditorGUILayout.TextField("Name: ", myString);
        //
        //     if (GUILayout.Button("Press Me"))
        //     {
        //         Debug.Log("Button is Pressed!");
        //     }
        // }
        ///////////////////////////////////////////////////////////////////////////////////////////////////
    
        #region Variables

        private Color _color;
    
        #endregion
    
        [MenuItem("Window/Colorizer")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ColorizerTool>("Colorizer");
        }
  
        private void OnGUI()
        {
            GUILayout.Space(5);
            GUILayout.Label("Color the selected Object!", EditorStyles.boldLabel);
            GUILayout.Space(10);
            _color = EditorGUILayout.ColorField("Color", _color);
            GUILayout.Space(5);
            if (GUILayout.Button("COLORIZE!"))
            {
                Colorize();
            }
        }

        private void Colorize()
        {
            // When we need to select object on scene we are using it.
            foreach (GameObject obj in Selection.gameObjects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();

                if (renderer != null)
                {
                    renderer.sharedMaterial.color = this._color;
                }
                // Also can be used as following.
                // try
                // {
                //     renderer.sharedMaterial.color = color;
                // }
                // catch (Exception e)
                // {
                //     throw new Exception($"{obj.name} is not having renderer on it!");
                // }
            }
        }
    }
}