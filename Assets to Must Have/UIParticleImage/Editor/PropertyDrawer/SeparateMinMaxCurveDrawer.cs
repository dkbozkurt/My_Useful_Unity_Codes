using UnityEditor;
using UnityEngine;

namespace AssetKits.ParticleImage.Editor
{
    [CustomPropertyDrawer(typeof(SeparatedMinMaxCurve))]
    public class SeparateMinMaxCurveDrawer : PropertyDrawer
    {
        private GUIContent _separateAxesContent;
        
        private GUIContent separateAxesContent
        {
            get
            {
                if (_separateAxesContent == null)
                {
                    _separateAxesContent = new GUIContent(EditorGUIUtility.IconContent("d_AvatarPivot").image, "Separate Axes");
                }

                return _separateAxesContent;
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.FindPropertyRelative("separated").boolValue)
            {
                return 66;
            }
            else
            {
                return base.GetPropertyHeight(property, label);
            }
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool separable = property.FindPropertyRelative("separable").boolValue;
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var separated = new Rect(position.x, position.y, 21, 20);
            var main = new Rect(separable ? position.x + 21 : position.x, position.y, separable ? position.width-21 : position.width, 20);
            var x = new Rect(separable ? position.x + 21 : position.x, position.y, separable ? position.width-21 : position.width, 20);
            var y = new Rect(separable ? position.x + 21 : position.x, position.y + 22, separable ? position.width-21 : position.width, 20);
            var z = new Rect(separable ? position.x + 21 : position.x, position.y + 22*2, separable ? position.width-21 : position.width, 20);

            if (separable)
            {
                if (GUI.Button(separated, separateAxesContent, GUIStyle.none))
                {
                    property.FindPropertyRelative("separated").boolValue =
                        !property.FindPropertyRelative("separated").boolValue;
                }
            }
            
            if (property.FindPropertyRelative("separated").boolValue)
            {
                EditorGUIUtility.labelWidth = 10;
                EditorGUI.PropertyField(x, property.FindPropertyRelative("xCurve"), new GUIContent("X"));
                EditorGUI.PropertyField(y, property.FindPropertyRelative("yCurve"), new GUIContent("Y"));
                EditorGUI.PropertyField(z, property.FindPropertyRelative("zCurve"), new GUIContent("Z"));
            }
            else
            {
                EditorGUIUtility.labelWidth = 10;
                EditorGUI.PropertyField(main, property.FindPropertyRelative("mainCurve"), GUIContent.none);
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
