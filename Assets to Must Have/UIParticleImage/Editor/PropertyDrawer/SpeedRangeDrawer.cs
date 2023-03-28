using UnityEditor;
using UnityEngine;

namespace AssetKits.ParticleImage.Editor
{
    [CustomPropertyDrawer(typeof(SpeedRange))]
    public class SpeedRangeDrawer : PropertyDrawer
    {
        private GUIContent _fromLabel = new GUIContent("From");
        private GUIContent _toLabel = new GUIContent("To");
        
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var from = new Rect(position.x, position.y, position.width/2, position.height);
            var to = new Rect(position.x + 6+position.width/2, position.y, position.width/2-6, position.height);

            EditorGUIUtility.labelWidth = 35;

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(from, property.FindPropertyRelative("from"), _fromLabel);
            EditorGUIUtility.labelWidth = 20;
            EditorGUI.PropertyField(to, property.FindPropertyRelative("to"), _toLabel);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
