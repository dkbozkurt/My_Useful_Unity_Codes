// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace Custom_Attributes.ExposedScriptableObjectAttribute
{
    /// <summary>
    /// Ref : https://forum.unity.com/threads/editor-tool-better-scriptableobject-inspector-editing.484393/
    /// </summary>
    [CreateAssetMenu(fileName = "ExposedSOTest", menuName = "ExposedSOTest")]
    public class ExposedSOAttributeTest : ScriptableObject
    {
        public string Name;
        public string Surname;
        public int Age;
    }
}
