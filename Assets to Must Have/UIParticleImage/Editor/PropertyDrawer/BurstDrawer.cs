using UnityEditor;
using UnityEngine;

namespace AssetKits.ParticleImage.Editor
{
    [CustomPropertyDrawer(typeof(Burst))]
    public class IngredientDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var time = new Rect(position.x, position.y, position.width/2-5, position.height);
            var count = new Rect(position.x + 5+position.width/2, position.y, position.width/2-5, position.height);
            
            EditorGUIUtility.labelWidth = 45;

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(time, property.FindPropertyRelative("time"), new GUIContent("Time"));
            EditorGUI.PropertyField(count, property.FindPropertyRelative("count"), new GUIContent("Count"));
            
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
