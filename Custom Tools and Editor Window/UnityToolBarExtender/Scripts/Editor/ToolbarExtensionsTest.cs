// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace Custom_Tools_And_Editor_Window.UnityToolBarExtender.Scripts.Editor
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=NBARPT-APCg&list=PLXD0wONGOSCKznviSz_iqLUz0Rl3HQbBC&index=4&ab_channel=GameDevGuide
    /// Ref : https://github.com/marijnz/unity-toolbar-extender
    /// </summary>
    [InitializeOnLoad]
    public class ToolbarExtensionsTest : MonoBehaviour
    {
        static ToolbarExtensionsTest()
        {
            ToolbarExtender.LeftToolbarGUI.Add(DrawLeftGUI);
            ToolbarExtender.LeftToolbarGUI.Add(DrawRightGUI);
        }

        static void DrawLeftGUI()
        {
            GUILayout.FlexibleSpace();
            GUILayout.Button("Left Button 1");
            GUILayout.Button("Left Button 2");
            
        }
        
        static void DrawRightGUI()
        {
            GUILayout.FlexibleSpace();
            GUILayout.Button("Right Button 1");
            GUILayout.Label("Right Label!!!");
        }
    }
}