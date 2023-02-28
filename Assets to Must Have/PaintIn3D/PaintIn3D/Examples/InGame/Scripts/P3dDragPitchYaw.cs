using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component adds basic Pitch/Yaw controls to the current GameObject (e.g. camera) using mouse or touch controls.</summary>
	[ExecuteInEditMode]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dDragPitchYaw")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Drag Pitch Yaw")]
	public class P3dDragPitchYaw : MonoBehaviour
	{
		/// <summary>Rotation will be active if all of these tools are deactivated.</summary>
		public Transform Tools { set { tools = value; } get { return tools; } } [SerializeField] private Transform tools;

		/// <summary>The key that must be held for this component to activate on desktop platforms.
		/// None = Any mouse button.</summary>
		public KeyCode Key { set { key = value; } get { return key; } } [SerializeField] private KeyCode key = KeyCode.Mouse1;

		/// <summary>Fingers that began touching the screen on top of these UI layers will be ignored.</summary>
		public LayerMask GuiLayers { set { guiLayers = value; } get { return guiLayers; } } [SerializeField] private LayerMask guiLayers = 1 << 5;

		/// <summary>The target pitch angle in degrees.</summary>
		public float Pitch { set { pitch = value; } get { return pitch; } } [SerializeField] private float pitch;

		/// <summary>The speed the pitch changed relative to the mouse/finger drag distance.</summary>
		public float PitchSensitivity { set { pitchSensitivity = value; } get { return pitchSensitivity; } } [SerializeField] private float pitchSensitivity = 0.1f;

		/// <summary>The minimum value of the pitch value.</summary>
		public float PitchMin { set { pitchMin = value; } get { return pitchMin; } } [SerializeField] private float pitchMin = -90.0f;

		/// <summary>The maximum value of the pitch value.</summary>
		public float PitchMax { set { pitchMax = value; } get { return pitchMax; } } [SerializeField] private float pitchMax = 90.0f;

		/// <summary>The target yaw angle in degrees.</summary>
		public float Yaw { set { yaw = value; } get { return yaw; } } [SerializeField] private float yaw;

		/// <summary>The speed the yaw changed relative to the mouse/finger drag distance.</summary>
		public float YawSensitivity { set { yawSensitivity = value; } get { return yawSensitivity; } } [SerializeField] private float yawSensitivity = 0.1f;

		/// <summary>How quickly the rotation transitions from the current to the target value (-1 = instant).</summary>
		public float Dampening { set { dampening = value; } get { return dampening; } } [SerializeField] private float dampening = 10.0f;

		[SerializeField]
		private float currentPitch;

		[SerializeField]
		private float currentYaw;

		[System.NonSerialized]
		private List<CwInputManager.Finger> fingers = new List<CwInputManager.Finger>();

		protected virtual void OnEnable()
		{
			CwInputManager.EnsureThisComponentExists();

			CwInputManager.OnFingerDown += HandleFingerDown;
			CwInputManager.OnFingerUp   += HandleFingerUp;
		}

		protected virtual void OnDisable()
		{
			CwInputManager.OnFingerDown -= HandleFingerDown;
			CwInputManager.OnFingerUp   -= HandleFingerUp;
		}

		private void HandleFingerDown(CwInputManager.Finger finger)
		{
			if (finger.Index == CwInputManager.HOVER_FINGER_INDEX) return;

			if (CwInputManager.PointOverGui(finger.ScreenPosition, guiLayers) == true) return;

			if (key != KeyCode.None && CwInput.GetKeyIsHeld(key) == false) return;

			fingers.Add(finger);
		}

		private void HandleFingerUp(CwInputManager.Finger finger)
		{
			fingers.Remove(finger);
		}

		protected virtual void Update()
		{
			// Calculate delta
			if (CanRotate == true && Application.isPlaying == true)
			{
				var delta = CwInputManager.GetAverageDeltaScaled(fingers);

				pitch -= delta.y * pitchSensitivity;
				yaw   += delta.x *   yawSensitivity;
			}

			pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

			// Smoothly dampen values
			var factor = CwHelper.DampenFactor(dampening, Time.deltaTime);

			currentPitch = Mathf.Lerp(currentPitch, pitch, factor);
			currentYaw   = Mathf.Lerp(currentYaw  , yaw  , factor);

			// Apply new rotation
			transform.localRotation = Quaternion.Euler(currentPitch, currentYaw, 0.0f);
		}

		private bool CanRotate
		{
			get
			{
				if (CwInput.GetKeyIsHeld(key) == true)
				{
					return true;
				}

				if (tools != null)
				{
					foreach (Transform child in tools)
					{
						if (child.gameObject.activeSelf == true)
						{
							return false;
						}
					}

					return true;
				}

				return false;
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dDragPitchYaw;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dDragPitchYaw_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("tools", "Rotation will be active if all of these tools are deactivated.");
			Draw("key", "The key that must be held for this component to activate on desktop platforms.\n\nNone = Any mouse button.");
			Draw("guiLayers", "Fingers that began touching the screen on top of these UI layers will be ignored.");

			Separator();

			Draw("pitch", "The target pitch angle in degrees.");
			Draw("pitchSensitivity", "The speed the camera rotates relative to the mouse/finger drag distance.");
			Draw("pitchMin", "The minimum value of the pitch value.");
			Draw("pitchMax", "The maximum value of the pitch value.");

			Separator();

			Draw("yaw", "The target yaw angle in degrees.");
			Draw("yawSensitivity", "The speed the yaw changed relative to the mouse/finger drag distance.");

			Separator();

			Draw("dampening", "How quickly the rotation transitions from the current to the target value (-1 = instant).");
		}
	}
}
#endif