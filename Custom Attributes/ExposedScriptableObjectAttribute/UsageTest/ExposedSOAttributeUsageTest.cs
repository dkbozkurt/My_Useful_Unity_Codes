// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace Custom_Attributes.ExposedScriptableObjectAttribute
{
    /// <summary>
    /// Ref : https://forum.unity.com/threads/editor-tool-better-scriptableobject-inspector-editing.484393/
    /// </summary>
    public class ExposedSOAttributeUsageTest : MonoBehaviour
    {
        [ExposedScriptableObject]
        public ExposedSOAttributeTest _exposedSoAttributeTest;
    }
}
