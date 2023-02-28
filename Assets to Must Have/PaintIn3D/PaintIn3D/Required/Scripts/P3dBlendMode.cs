using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This defines the blending mode used by a painting operation.</summary>
	[System.Serializable]
	public struct P3dBlendMode
	{
		public static P3dBlendMode AlphaBlend(Vector4 channels) { return new P3dBlendMode() { Index = ALPHA_BLEND, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode AlphaBlendInverse(Vector4 channels) { return new P3dBlendMode() { Index = ALPHA_BLEND_INVERSE, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode Premultiplied(Vector4 channels) { return new P3dBlendMode() { Index = PREMULTIPLIED, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode Additive(Vector4 channels) { return new P3dBlendMode() { Index = ADDITIVE, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode AdditiveSoft(Vector4 channels) { return new P3dBlendMode() { Index = ADDITIVE_SOFT, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode Subtractive(Vector4 channels) { return new P3dBlendMode() { Index = SUBTRACTIVE, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode SubtractiveSoft(Vector4 channels) { return new P3dBlendMode() { Index = SUBTRACTIVE_SOFT, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode Replace(Vector4 channels) { return new P3dBlendMode() { Index = REPLACE, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode ReplaceOriginal(Vector4 channels) { return new P3dBlendMode() { Index = REPLACE_ORIGINAL, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode ReplaceCustom(Color color, Texture texture, Vector4 channels) { return new P3dBlendMode() { Index = REPLACE_CUSTOM, Color = color, Texture = texture, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode MultiplyInverseRGB(Vector4 channels) { return new P3dBlendMode() { Index = MULTIPLY_INVERSE_RGB, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode Blur(float kernel, Vector4 channels) { return new P3dBlendMode() { Index = BLUR, Color = Color.white, Kernel = kernel, Channels = channels }; }
		public static P3dBlendMode NormalBlend(Vector4 channels) { return new P3dBlendMode() { Index = NORMAL_BLEND, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode NormalReplace(Vector4 channels) { return new P3dBlendMode() { Index = NORMAL_REPLACE, Color = Color.white, Kernel = 1.0f, Channels = channels }; }
		public static P3dBlendMode Flow(float kernel, Vector4 channels) { return new P3dBlendMode() { Index = FLOW, Color = Color.gray, Kernel = kernel, Channels = channels }; }

		// Indices of all blending modes
		public const int ALPHA_BLEND          = 0;
		public const int ALPHA_BLEND_INVERSE  = 1;
		public const int PREMULTIPLIED        = 2;
		public const int ADDITIVE             = 3;
		public const int ADDITIVE_SOFT        = 4;
		public const int SUBTRACTIVE          = 5;
		public const int SUBTRACTIVE_SOFT     = 6;
		public const int REPLACE              = 7;
		public const int REPLACE_ORIGINAL     = 8;
		public const int REPLACE_CUSTOM       = 9;
		public const int MULTIPLY_INVERSE_RGB = 10;
		public const int BLUR                 = 11;
		public const int NORMAL_BLEND         = 12;
		public const int NORMAL_REPLACE       = 13;
		public const int FLOW                 = 14;
		public const int COUNT                = 15;

		// Pretty names of all blending modes
		public static readonly string[] NAMES =
			{
				"Alpha Blend",
				"Alpha Blend Inverse",
				"Premultiplied",
				"Additive",
				"Additive Soft",
				"Subtractive",
				"Subtractive Soft",
				"Replace",
				"Replace Original",
				"Replace Custom",
				"Multiply RGB Inverse",
				"Blur",
				"Normal Blend",
				"Normal Replace",
				"Flow",
			};

		/// <summary>This is the index of the currently selected blending mode.</summary>
		public int Index;

		/// <summary>When using the <b>ReplaceCustom</b> blending mode, this allows you to specify the replacement color.</summary>
		public Color Color;

		/// <summary>When using the <b>ReplaceCustom</b> blending mode, this allows you to specify the replacement texture.</summary>
		public Texture Texture;

		/// <summary>When using the <b>Blur</b> or <b>Flow</b> blending modes, this allows you to set the maximum pixel distance of samples.</summary>
		public float Kernel;

		/// <summary>This allows you to control which channels will be modified by this blending mode.
		/// 1,1,1,1 = All channels will be modified.
		/// 1,0,0,0 = Only red will be modified.</summary>
		public Vector4 Channels;

		private static int _Channels           = Shader.PropertyToID("_Channels");
		private static int _ReplaceColor       = Shader.PropertyToID("_ReplaceColor");
		private static int _ReplaceTexture     = Shader.PropertyToID("_ReplaceTexture");
		private static int _ReplaceTextureSize = Shader.PropertyToID("_ReplaceTextureSize");
		private static int _Kernel             = Shader.PropertyToID("_Kernel");

		public void Apply(Material material)
		{
			material.SetVector(_Channels, Channels);

			if (Index == REPLACE_CUSTOM || Index == REPLACE_ORIGINAL)
			{
				material.SetColor(_ReplaceColor, CwHelper.ToLinear(Color));
				material.SetTexture(_ReplaceTexture, Texture);

				if (Texture != null)
				{
					material.SetVector(_ReplaceTextureSize, new Vector2(Texture.width, Texture.height));
				}
			}
			else if (Index == BLUR || Index == FLOW)
			{
				material.SetFloat(_Kernel, Kernel);
			}
		}

		public static string GetName(int index)
		{
			if (index >= 0 && index < COUNT)
			{
				return NAMES[index];
			}

			return default(string);
		}

		public static bool operator == (P3dBlendMode a, int b)
		{
			return a.Index == b;
		}

		public static bool operator != (P3dBlendMode a, int b)
		{
			return a.Index != b;
		}

		public static implicit operator int (P3dBlendMode a)
		{
			return a.Index;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;

	[CustomPropertyDrawer(typeof(P3dBlendMode))]
	public class P3dBlendMode_Drawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var index  = property.FindPropertyRelative("Index").intValue;
			var sCha   = property.FindPropertyRelative("Channels");
			var height = base.GetPropertyHeight(property, label);
			var step   = height + 2;

			height += EditorGUI.GetPropertyHeight(sCha, label) + 2;

			if (index == P3dBlendMode.REPLACE_CUSTOM)
			{
				height += step * 2;
			}

			if (index == P3dBlendMode.BLUR || index == P3dBlendMode.FLOW)
			{
				height += step * 1;
			}

			return height;
		}

		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			var sObj   = property.serializedObject;
			var sIdx   = property.FindPropertyRelative("Index");
			var sCol   = property.FindPropertyRelative("Color");
			var sTex   = property.FindPropertyRelative("Texture");
			var sKer   = property.FindPropertyRelative("Kernel");
			var sCha   = property.FindPropertyRelative("Channels");
			var height = base.GetPropertyHeight(property, label);

			rect.height = height;

			var right = rect; right.xMin += EditorGUIUtility.labelWidth;

			EditorGUI.LabelField(rect, label);

			if (GUI.Button(right, P3dBlendMode.GetName(sIdx.intValue), EditorStyles.popup) == true)
			{
				var menu = new GenericMenu();

				for (var i = 0; i < P3dBlendMode.COUNT; i++)
				{
					var index   = i;
					var content = new GUIContent(P3dBlendMode.GetName(index));
					var on      = index == sIdx.intValue;

					menu.AddItem(content, on, () => { sIdx.intValue = index; sObj.ApplyModifiedProperties(); });
				}

				menu.DropDown(right);
			}

			EditorGUI.indentLevel++;
				rect.y += rect.height + 2; sCha.vector4Value = EditorGUI.Vector4Field(rect, new GUIContent(sCha.displayName, sCha.tooltip), sCha.vector4Value);
				if (sIdx.intValue == P3dBlendMode.REPLACE_CUSTOM)
				{
					rect.y += rect.height + 2; EditorGUI.PropertyField(rect, sCol);
					rect.y += rect.height + 2; EditorGUI.PropertyField(rect, sTex);
				}
				if (sIdx.intValue == P3dBlendMode.BLUR || sIdx.intValue == P3dBlendMode.FLOW)
				{
					rect.y += rect.height + 2; EditorGUI.PropertyField(rect, sKer);
				}
			EditorGUI.indentLevel--;
		}

		private void DrawObjectProperty<T>(ref Rect rect, SerializedProperty property, string title)
			where T : Object
		{
			var propertyObject = property.FindPropertyRelative("Object");
			var oldValue       = propertyObject.objectReferenceValue as T;
			var mixed          = EditorGUI.showMixedValue; EditorGUI.showMixedValue = propertyObject.hasMultipleDifferentValues;
				var newValue = EditorGUI.ObjectField(rect, title, oldValue, typeof(T), true);
			EditorGUI.showMixedValue = mixed;

			if (oldValue != newValue)
			{
				propertyObject.objectReferenceValue = newValue;
			}
		}

		private void DrawProperty(ref Rect rect, SerializedProperty property, GUIContent label, string childName, string overrideName = null, string overrideTooltip = null)
		{
			var childProperty = property.FindPropertyRelative(childName);

			label.text = string.IsNullOrEmpty(overrideName) == false ? overrideName : childProperty.displayName;

			label.tooltip = string.IsNullOrEmpty(overrideTooltip) == false ? overrideTooltip : childProperty.tooltip;

			EditorGUI.PropertyField(rect, childProperty, label);
		}
	}
}
#endif