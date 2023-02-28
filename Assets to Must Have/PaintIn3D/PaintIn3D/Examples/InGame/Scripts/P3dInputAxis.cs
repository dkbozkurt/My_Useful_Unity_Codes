using UnityEngine;
using UnityEngine.Events;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component allows you to convert input axis values to a boolean event. This can be used to map VR buttons to other components.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dInputAxis")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Input Axis")]
	public class P3dInputAxis : MonoBehaviour
	{
		[System.Serializable] public class BoolEvent : UnityEvent<bool> {}

		/// <summary>The name of the input axis in the Project Settings.</summary>
		public string AxisName { set { axisName = value; } get { return axisName; } } [SerializeField] private string axisName;

		/// <summary>The index of the input axis in the Project Settings.</summary>
		public int AxisIndex { set { axisIndex = value; } get { return axisIndex; } } [SerializeField] private int axisIndex;

		public BoolEvent OnValue { get { if (onValue == null) onValue = new BoolEvent(); return onValue; } } [SerializeField] private BoolEvent onValue;

		protected virtual void Update()
		{
			if (onValue != null)
			{
				onValue.Invoke(Input.GetAxisRaw(axisName) > 0.1f);
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dInputAxis;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dInputAxis_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("axisName", "The SimulatedOffset value will be offset by this when the simulated key is held.");
			Draw("axisIndex", "The index of the input axis in the Project Settings.");

	#if ENABLE_INPUT_SYSTEM
			Warning("You have enabled the new InputSystem. To use it with Paint in 3D's VR tools, replace this component with the PlayerInput component that comes with the InputSystem, bind it to your desired VR controls, and use the trigger/grip Hold events to set the same values from its event.");
	#else
			DrawMap(tgt);
	#endif

			Separator();

			Draw("onValue", "");
		}

		private void DrawMap(TARGET tgt)
		{
			var inputManagers = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset");

			if (inputManagers != null && inputManagers.Length > 0)
			{
				var inputManager = new SerializedObject(inputManagers[0]);
				var axes         = inputManager.FindProperty("m_Axes");

				if (axes != null)
				{
					var axis = FindAxis(axes, tgt.AxisName);

					if (axis == null)
					{
						Warning("This axis hasn't been mapped to your Project Settings yet, so you will not be able to use it.");
					}

					if (Button(axis == null ? "Create Input Axis" : "Update Input Axis") == true)
					{
						if (axis == null)
						{
							var index = axes.arraySize;

							axes.InsertArrayElementAtIndex(index);

							axis = axes.GetArrayElementAtIndex(index);
						}

						axis.FindPropertyRelative("m_Name").stringValue = tgt.AxisName;
						axis.FindPropertyRelative("axis").intValue = tgt.AxisIndex;
						axis.FindPropertyRelative("type").intValue = 2; // Axis
						axis.FindPropertyRelative("gravity").floatValue = 0;
						axis.FindPropertyRelative("sensitivity").floatValue = 1;

						inputManager.ApplyModifiedProperties();
					}
				}
			}
		}

		private static SerializedProperty FindAxis(SerializedProperty axes, string name)
		{
			for (var i = 0; i < axes.arraySize; i++)
			{
				var axis = axes.GetArrayElementAtIndex(i);

				if (axis.FindPropertyRelative("m_Name").stringValue == name)
				{
					return axis;
				}
			}

			return null;
		}
	}
}
#endif