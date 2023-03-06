// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Custom_Tools_And_Editor_Window.EasyEditorWindowWithSerializedProperties.Editor
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=c_3DXBrH-Is&ab_channel=GameDevGuide
    /// </summary>
    [CustomEditor(typeof(GameDataObject))]
    public class GameDataCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                GameDataObjectEditorWindow.Open((GameDataObject) target);
            }
        }
    }

    public class AssetHandler
    {
        [OnOpenAsset()]
        public static bool OpenEditor(int instanceId, int line)
        {
            GameDataObject obj = EditorUtility.InstanceIDToObject(instanceId) as GameDataObject;
            if (obj != null)
            {
                GameDataObjectEditorWindow.Open(obj);
                return true;
            }
            return false;
        }
        
    }
}
