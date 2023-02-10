using UnityEngine;

namespace PaintIn3D
{
	[System.Serializable]
    public struct P3dHash
    {
		[SerializeField]
        private int v;

        public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return v.GetHashCode();
		}

		public P3dHash(int newValue)
		{
			v = newValue;
		}

		public static implicit operator int(P3dHash hash)
		{
			return hash.v;
		}

		public static implicit operator P3dHash(int index)
		{
			return new P3dHash(index);
		}

		public override string ToString()
		{
			return v.ToString();
		}
    }
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;

	[CustomPropertyDrawer(typeof(P3dHash))]
	public class ForgeSeedDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			property = property.FindPropertyRelative("v");

			var rect1 = position; rect1.xMax = position.xMax - 20;
			var rect2 = position; rect2.xMin = position.xMax - 18;

			EditorGUI.PropertyField(rect1, property, label);

			if (GUI.Button(rect2, "R") == true)
			{
				var path    = property.propertyPath;
				var objects = property.serializedObject.targetObjects;
				var context = property.serializedObject.context;

				for (var i = objects.Length - 1; i >= 0; i--)
				{
					var obj = new SerializedObject(objects[i], context);
					var pro = obj.FindProperty(path);

					pro.intValue = Random.Range(int.MinValue, int.MaxValue);

					obj.ApplyModifiedProperties();
				}
			}
		}
	}
}
#endif