using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This class contains common code for screen based mouse/finger hit components.</summary>
	public abstract class P3dHitScreenBase : MonoBehaviour
	{
		public enum RotationType
		{
			Normal,
			World,
			ThisRotation,
			ThisLocalRotation,
			CustomRotation,
			CustomLocalRotation
		}

		public enum RelativeType
		{
			WorldUp,
			CameraUp,
			DrawAngle
		}

		public enum DirectionType
		{
			HitNormal,
			RayDirection,
			CameraDirection
		}

		public enum EmitType
		{
			PointsIn3D    = 0,
			PointsOnUV    = 20,
			TrianglesIn3D = 30
		}

		[System.Flags]
		public enum ButtonTypes
		{
			LeftMouse   = 1 << 0,
			RightMouse  = 1 << 1,
			MiddleMouse = 1 << 2,
			Touch       = 1 << 5
		}

		/// <summary>Orient to a specific camera?
		/// None = MainCamera.</summary>
		public Camera Camera { set { _camera = value; } get { return _camera; } } [SerializeField] private Camera _camera;

		/// <summary>The layers you want the raycast to hit.</summary>
		public LayerMask Layers { set { layers = value; } get { return layers; } } [SerializeField] private LayerMask layers = Physics.DefaultRaycastLayers;

		/// <summary>Which inputs should this component react to?</summary>
		public ButtonTypes RequiredButtons { set { requiredButtons = value; } get { return requiredButtons; } } [SerializeField] private ButtonTypes requiredButtons = (ButtonTypes)~0;

		/// <summary>Which key should this component react to?</summary>
		public KeyCode RequiredKey { set { requiredKey = value; } get { return requiredKey; } } [SerializeField] private KeyCode requiredKey;

		/// <summary>Fingers that began touching the screen on top of these UI layers will be ignored.</summary>
		public LayerMask GuiLayers { set { guiLayers = value; } get { return guiLayers; } } [SerializeField] private LayerMask guiLayers = 1 << 5;

		/// <summary>This allows you to control the hit data this component sends out.
		/// PointsIn3D = Point drawing in 3D.
		/// PointsOnUV = Point drawing on UV (requires non-convex <b>MeshCollider</b>).
		/// TrianglesIn3D = Triangle drawing in 3D (requires non-convex <b>MeshCollider</b>).</summary>
		public EmitType Emit { set { emit = value; } get { return emit; } } [UnityEngine.Serialization.FormerlySerializedAs("draw")] [SerializeField] private EmitType emit;

		/// <summary>This allows you to control how the paint is rotated.
		/// Normal = The rotation will be based on a normal direction, and rolled relative to an up axis.
		/// World = The rotation will be aligned to the world, or given no rotation.
		/// ThisRotation = The current <b>Transform.rotation</b> will be used.
		/// ThisLocalRotation = The current <b>Transform.localRotation</b> will be used.
		/// CustomRotation = The specified <b>CustomTransform.rotation</b> will be used.
		/// CustomLocalRotation = The specified <b>CustomTransform.localRotation</b> will be used.</summary>
		public RotationType RotateTo { set { rotateTo = value; } get { return rotateTo; } } [SerializeField] private RotationType rotateTo;

		/// <summary>Which direction should the hit point rotation be based on?</summary>
		public DirectionType NormalDirection { set { normalDirection = value; } get { return normalDirection; } } [UnityEngine.Serialization.FormerlySerializedAs("normal")] [SerializeField] private DirectionType normalDirection = DirectionType.CameraDirection;

		/// <summary>Based on the normal direction, what should the rotation be rolled relative to?
		/// WorldUp = It will be rolled so the up vector is world up.
		/// CameraUp = It will be rolled so the up vector is camera up.
		/// DrawAngle = It will be rolled according to the mouse/finger movement on screen.</summary>
		public RelativeType NormalRelativeTo { set { normalRelativeTo = value; } get { return normalRelativeTo; } } [UnityEngine.Serialization.FormerlySerializedAs("orientation")] [SerializeField] private RelativeType normalRelativeTo = RelativeType.CameraUp;

		/// <summary>This allows you to specify the <b>Transform</b> when using <b>RotateTo = CustomRotation/CustomLocalRotation</b>.</summary>
		public Transform CustomTransform { set { customTransform = value; } get { return customTransform; } } [SerializeField] private Transform customTransform;

		/// <summary>If you want the raycast hit point to be offset from the surface a bit, this allows you to set by how much in world space.</summary>
		public float NormalOffset { set { normalOffset = value; } get { return normalOffset; } } [UnityEngine.Serialization.FormerlySerializedAs("offset")] [SerializeField] private float normalOffset;

		/// <summary>If you want the hit point to be offset upwards when using mouse input, this allows you to specify the physical distance the hit will be offset by on the screen. This is useful if you find paint hard to see because it's underneath your mouse.</summary>
		public float MouseOffset { set { mouseOffset = value; } get { return mouseOffset; } } [SerializeField] private float mouseOffset;

		/// <summary>If you want the hit point to be offset upwards when using touch input, this allows you to specify the physical distance the hit will be offset by on the screen. This is useful if you find paint hard to see because it's underneath your finger.</summary>
		public float TouchOffset { set { touchOffset = value; } get { return touchOffset; } } [SerializeField] private float touchOffset;

		/// <summary>Should painting triggered from this component be eligible for being undone?</summary>
		public bool StoreStates { set { storeStates = value; } get { return storeStates; } } [SerializeField] protected bool storeStates = true;

		/// <summary>Show a painting preview under the mouse?</summary>
		public bool ShowPreview { set { showPreview = value; } get { return showPreview; } } [SerializeField] protected bool showPreview = true;

		/// <summary>This allows you to override the order this paint gets applied to the object during the current frame.</summary>
		public int Priority { set { priority = value; } get { return priority; } } [SerializeField] private int priority;

		[System.NonSerialized]
		protected List<CwInputManager.Finger> fingers = new List<CwInputManager.Finger>();

		protected bool keyPressed;

		public bool NeedsDrawAngle
		{
			get
			{
				return rotateTo == RotationType.Normal && normalRelativeTo == RelativeType.DrawAngle;
			}
		}

		protected virtual void OnEnable()
		{
			CwInputManager.EnsureThisComponentExists();

			CwInputManager.OnFingerDown += HandleFingerDown;
			
			foreach (var finger in CwInputManager.Fingers)
			{
				HandleFingerDown(finger);
			}
		}

		protected virtual void LateUpdate()
		{
			for (var i = fingers.Count - 1; i >= 0; i--)
			{
				var finger = fingers[i];
				var down   = finger.Down;
				var up     = finger.Up;

				if (finger.Index == CwInputManager.HOVER_FINGER_INDEX)
				{
					if (keyPressed == true)
					{
						if (requiredKey == KeyCode.None || CwInput.GetKeyIsHeld(requiredKey) == false)
						{
							up = true;
						}
					}
					else
					{
						if (requiredKey != KeyCode.None && CwInput.GetKeyWentDown(requiredKey) == true)
						{
							if (CwInputManager.PointOverGui(finger.ScreenPosition, guiLayers) == false)
							{
								keyPressed = true;
								down       = true;
							}
						}
					}
				}

				HandleFingerUpdate(finger, down, up);

				if (up == true)
				{
					if (finger.Index != CwInputManager.HOVER_FINGER_INDEX)
					{
						fingers.Remove(finger);
					}

					HandleFingerUp(finger);
				}
			}

			if (keyPressed == true)
			{
				if (requiredKey == KeyCode.None || CwInput.GetKeyIsHeld(requiredKey) == false)
				{
					keyPressed = false;
				}
			}
		}

		protected virtual void HandleFingerUpdate(CwInputManager.Finger finger, bool down, bool up)
		{
		}

		protected virtual void HandleFingerUp(CwInputManager.Finger finger)
		{
		}

		protected virtual void OnDisable()
		{
			CwInputManager.OnFingerDown -= HandleFingerDown;

			fingers.Clear();
		}

		private bool FingerWentDown(CwInputManager.Finger finger)
		{
			if (finger.Index < 0)
			{
				if (CwInput.GetMouseExists() == true)
				{
					if ((requiredButtons & ButtonTypes.LeftMouse) != 0 && CwInput.GetKeyIsHeld(KeyCode.Mouse0) == true)
					{
						return true;
					}

					if ((requiredButtons & ButtonTypes.RightMouse) != 0 && CwInput.GetKeyIsHeld(KeyCode.Mouse1) == true)
					{
						return true;
					}

					if ((requiredButtons & ButtonTypes.MiddleMouse) != 0 && CwInput.GetKeyIsHeld(KeyCode.Mouse2) == true)
					{
						return true;
					}
				}
			}
			else if ((requiredButtons & ButtonTypes.Touch) != 0)
			{
				return true;
			}

			return false;
		}

		private void HandleFingerDown(CwInputManager.Finger finger)
		{
			if (finger.Index != CwInputManager.HOVER_FINGER_INDEX)
			{
				if (FingerWentDown(finger) == false) return;

				if (CwInputManager.PointOverGui(finger.ScreenPosition, guiLayers) == true) return;
			}

			fingers.Add(finger);
		}

		protected virtual void DoQuery(Vector2 screenPosition, ref Camera camera, ref Ray ray, ref RaycastHit hit3D, ref RaycastHit2D hit2D)
		{
			camera = CwHelper.GetCamera(_camera);
			ray    = camera.ScreenPointToRay(screenPosition);
			hit2D  = Physics2D.GetRayIntersection(ray, float.PositiveInfinity, layers);

			Physics.Raycast(ray, out hit3D, float.PositiveInfinity, layers);
		}

		protected void PaintAt(P3dPointConnector connector, P3dHitCache hitCache, Vector2 screenPosition, Vector2 screenPositionOld, bool preview, float pressure, object owner)
		{
			var offset = CwInput.GetTouchCount() > 0 ? touchOffset : mouseOffset;

			if (offset != 0.0f)
			{
				screenPosition.y    += offset / CwInputManager.ScaleFactor;
				screenPositionOld.y += offset / CwInputManager.ScaleFactor;
			}

			var camera        = default(Camera);
			var ray           = default(Ray);
			var hit2D         = default(RaycastHit2D);
			var hit3D         = default(RaycastHit);
			var finalPosition = default(Vector3);
			var finalRotation = default(Quaternion);

			DoQuery(screenPosition, ref camera, ref ray, ref hit3D, ref hit2D);

			var valid2D = hit2D.distance > 0.0f;
			var valid3D = hit3D.distance > 0.0f;

			// Hit 3D?
			if (valid3D == true && (valid2D == false || hit3D.distance < hit2D.distance))
			{
				CalcHitData(hit3D.point, hit3D.normal, ray, camera, screenPositionOld, ref finalPosition, ref finalRotation);

				if (emit == EmitType.PointsIn3D)
				{
					if (connector != null)
					{
						connector.SubmitPoint(gameObject, preview, priority, pressure, finalPosition, finalRotation, owner);
					}
					else
					{
						hitCache.InvokePoint(gameObject, preview, priority, pressure, finalPosition, finalRotation);
					}

					return;
				}
				else if (emit == EmitType.PointsOnUV)
				{
					hitCache.InvokeCoord(gameObject, preview, priority, pressure, new P3dHit(hit3D), finalRotation);

					return;
				}
				else if (emit == EmitType.TrianglesIn3D)
				{
					hitCache.InvokeTriangle(gameObject, preview, priority, pressure, hit3D, finalRotation);

					return;
				}
			}
			// Hit 2D?
			else if (valid2D == true)
			{
				CalcHitData(hit2D.point, hit2D.normal, ray, camera, screenPositionOld, ref finalPosition, ref finalRotation);

				if (emit == EmitType.PointsIn3D)
				{
					if (connector != null)
					{
						connector.SubmitPoint(gameObject, preview, priority, pressure, finalPosition, finalRotation, owner);
					}
					else
					{
						hitCache.InvokePoint(gameObject, preview, priority, pressure, finalPosition, finalRotation);
					}

					return;
				}
			}

			if (connector != null)
			{
				connector.BreakHits(owner);
			}
		}

		private void CalcHitData(Vector3 hitPoint, Vector3 hitNormal, Ray ray, Camera camera, Vector2 screenPositionOld, ref Vector3 finalPosition, ref Quaternion finalRotation)
		{
			finalPosition = hitPoint + hitNormal * normalOffset;
			finalRotation = Quaternion.identity;

			switch (rotateTo)
			{
				case RotationType.Normal:
				{
					var finalNormal = default(Vector3);

					switch (normalDirection)
					{
						case DirectionType.HitNormal: finalNormal = hitNormal; break;
						case DirectionType.RayDirection: finalNormal = -ray.direction; break;
						case DirectionType.CameraDirection: finalNormal = -camera.transform.forward; break;
					}

					var finalUp = Vector3.up;

					switch (normalRelativeTo)
					{
						case RelativeType.CameraUp: finalUp = camera.transform.up; break;
						case RelativeType.DrawAngle:
						{
							var rayOld = camera.ScreenPointToRay(screenPositionOld);

							if (camera.orthographic == true)
							{
								finalUp = Vector3.Cross(rayOld.GetPoint(1.0f) - ray.origin, ray.GetPoint(1.0f) - ray.origin);
							}
							else
							{
								finalUp = Vector3.Cross(rayOld.direction, ray.direction);
							}
						}

						break;
					}

					finalRotation = Quaternion.LookRotation(-finalNormal, finalUp);
				}
				break;
				case RotationType.World: finalRotation = Quaternion.identity; break;
				case RotationType.ThisRotation: finalRotation = transform.rotation; break;
				case RotationType.ThisLocalRotation: finalRotation = transform.localRotation; break;
				case RotationType.CustomRotation: if (customTransform != null) finalRotation = customTransform.rotation; break;
				case RotationType.CustomLocalRotation: if (customTransform != null) finalRotation = customTransform.localRotation; break;
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dHitScreenBase;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dHitScreenBase_Editor : CwEditor
	{
		protected virtual void DrawBasic()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("emit", "This allows you to control the hit data this component sends out.\n\nPointsIn3D = Point drawing in 3D.\n\nPointsOnUV = Point drawing on UV (requires non-convex MeshCollider).\n\nTrianglesIn3D = Triangle drawing in 3D (requires non-convex MeshCollider).");
			BeginError(Any(tgts, t => t.Layers == 0));
				Draw("layers", "The layers you want the raycast to hit.");
			EndError();
			Draw("guiLayers", "Fingers that began touching the screen on top of these UI layers will be ignored.");
			BeginError(Any(tgts, t => CwHelper.GetCamera(t.Camera) == null));
				Draw("_camera", "Orient to a specific camera?\n\nNone = MainCamera.");
			EndError();

			Separator();

			Draw("rotateTo", "This allows you to control how the paint is rotated.\n\nNormal = The rotation will be based on a normal direction, and rolled relative to an up axis.\n\nWorld = The rotation will be aligned to the world, or given no rotation.\n\nThisRotation = The current Transform.rotation will be used.\n\nThisLocalRotation = The current Transform.localRotation will be used.\n\nCustomRotation = The specified Transform.rotation will be used.\n\nCustomLocalRotation = The specified Transform.localRotation will be used.");
			if (Any(tgts, t => t.RotateTo == P3dHitScreenBase.RotationType.Normal))
			{
				BeginIndent();
					Draw("normalDirection", "Which direction should the hit point rotation be based on?", "Direction");
					Draw("normalRelativeTo", "Based on the normal direction, what should the rotation be rolled relative to?\n\nWorldUp = It will be rolled so the up vector is world up.\n\nCameraUp = It will be rolled so the up vector is camera up.\n\nDrawAngle = It will be rolled according to the mouse/finger movement on screen.", "Relative To");
				EndIndent();
			}
			if (Any(tgts, t => t.RotateTo == P3dHitScreenBase.RotationType.CustomRotation || t.RotateTo == P3dHitScreenBase.RotationType.CustomLocalRotation))
			{
				BeginIndent();
					Draw("customTransform", "This allows you to specify the Transform when using RotateTo = CustomRotation/CustomLocalRotation.");
				EndIndent();
			}
		}

		protected void DrawAdvancedFoldout()
		{
			if (DrawFoldout("Advanced", "Show advanced settings?") == true)
			{
				BeginIndent();
					DrawAdvanced();
				EndIndent();
			}
		}

		protected virtual void DrawAdvanced()
		{
			Draw("requiredButtons", "Which inputs should this component react to?");
			Draw("requiredKey", "Which key should this component react to?");
			Draw("storeStates", "Should painting triggered from this component be eligible for being undone?");
			Draw("showPreview", "Show a painting preview under the mouse?");
			Draw("priority", "This allows you to override the order this paint gets applied to the object during the current frame.");
			Draw("normalOffset", "If you want the raycast hit point to be offset from the surface a bit, this allows you to set by how much in world space.");
			Draw("mouseOffset", "If you want the hit point to be offset upwards when using mouse input, this allows you to specify the physical distance the hit will be offset by on the screen. This is useful if you find paint hard to see because it's underneath your mouse.");
			Draw("touchOffset", "If you want the hit point to be offset upwards when using touch input, this allows you to specify the physical distance the hit will be offset by on the screen. This is useful if you find paint hard to see because it's underneath your finger.");
		}
	}
}
#endif