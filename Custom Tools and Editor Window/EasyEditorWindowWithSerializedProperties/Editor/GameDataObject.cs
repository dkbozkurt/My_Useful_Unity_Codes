// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Custom_Tools_And_Editor_Window.EasyEditorWindowWithSerializedProperties.Editor
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=c_3DXBrH-Is&ab_channel=GameDevGuide
    /// </summary>
    [CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
    public class GameDataObject : ScriptableObject
    {
        public GameData[] GameData;
        
        public GameDataObject()
        {
            GameData = new GameData[3];
        }
    }

    [Serializable]
    public class GameData
    {
        public string Header;
        
        public int TestInt1;
        public int TestInt2;
        public int TestInt3;
        
        public string TestString1;
        public string TestString2;
    }

    
}
