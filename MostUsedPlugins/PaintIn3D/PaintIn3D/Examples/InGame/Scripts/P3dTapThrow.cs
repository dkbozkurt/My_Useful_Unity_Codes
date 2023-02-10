using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component will spawn and throw Rigidbody prefabs from the camera when you tap the mouse or a finger.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dTapThrow")]
	[AddComponentMenu(P3dCommon.ComponentMenuPrefix + "Tap Throw")]
	public class P3dTapThrow : MonoBehaviour
	{
		/// <summary>The key that must be held for this component to activate on desktop platforms.
		/// None = Any mouse button.</summary>
		public KeyCode Key { set { key = value; } get { return key; } } [SerializeField] private KeyCode key = KeyCode.Mouse0;

		/// <summary>Fingers that began touching the screen on top of these UI layers will be ignored.</summary>
		public LayerMask GuiLayers { set { guiLayers = value; } get { return guiLayers; } } [SerializeField] private LayerMask guiLayers = 1 << 5;

		/// <summary>The prefab that will be thrown.</summary>
		public GameObject Prefab { set { prefab = value; } get { return prefab; } } [SerializeField] private GameObject prefab;

		/// <summary>The speed that the object will be thrown at.</summary>
		public float Speed { set { speed = value; } get { return speed; } } [SerializeField] private float speed = 10.0f;

		/// <summary>Should painting triggered from this component be eligible for being undone?</summary>
		public bool StoreStates { set { storeStates = value; } get { return storeStates; } } [SerializeField] protected bool storeStates;

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
			if (fingers.Remove(finger) == true)
			{
				//var delta = Vector2.Distance(finger.StartScreenPosition, finger.ScreenPosition) * CwInputManager.ScaleFactor;

				if (finger.Age < 0.5f)// && delta < 20.0f)
				{
					DoThrow(finger.ScreenPosition);
				}
			}
		}

		private void DoThrow(Vector2 screenPosition)
		{
			if (prefab != null)
			{
				var camera = CwHelper.GetCamera(null);

				if (camera != null)
				{
					if (storeStates == true)
					{
						P3dStateManager.StoreAllStates();
					}

					// Find the ray for this screen position
					var ray      = camera.ScreenPointToRay(screenPosition);
					var rotation = Quaternion.LookRotation(ray.direction);

					// Loop through all prefabs and spawn them
					var clone = Instantiate(prefab, ray.origin, rotation);

					clone.SetActive(true);

					// Throw with velocity?
					var cloneRigidbody = clone.GetComponent<Rigidbody>();

					if (cloneRigidbody != null)
					{
						cloneRigidbody.velocity = clone.transform.forward * Speed;
					}
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dTapThrow;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dTapThrow_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("key", "The key that must be held for this component to activate on desktop platforms.\n\nNone = Any mouse button.");
			Draw("guiLayers", "Fingers that began touching the screen on top of these UI layers will be ignored.");

			Separator();

			BeginError(Any(tgts, t => t.Prefab == null));
				Draw("prefab", "The prefab that will be thrown.");
			EndError();
			Draw("speed", "Rotate the decal to the hit normal?");
			Draw("storeStates", "Should painting triggered from this component be eligible for being undone?");
		}
	}
}
#endif