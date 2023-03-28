using UnityEditor;
using UnityEngine;

namespace AssetKits.ParticleImage.Editor
{
    [CustomPropertyDrawer(typeof(Module))]
    public class ModuleDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            // Calculate rects
            var tick = new Rect(position.x, position.y, 22, position.height);
            var text = new Rect(position.x + 20, position.y, position.width - 20, position.height);

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(tick, property.FindPropertyRelative("enabled"), GUIContent.none);
            EditorGUI.LabelField(text, label);

            EditorGUI.EndProperty();
        }
    }
}
