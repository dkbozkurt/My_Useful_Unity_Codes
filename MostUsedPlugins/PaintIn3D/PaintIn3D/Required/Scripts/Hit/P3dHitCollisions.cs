using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component can be added to any Rigidbody, and it will fire hit events when it hits something.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dHitCollisions")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Hit Collisions")]
	public class P3dHitCollisions : MonoBehaviour
	{
		public enum EmitType
		{
			PointsIn3D    = 0,
			PointsOnUV    = 20,
			TrianglesIn3D = 30
		}

		public enum OrientationType
		{
			WorldUp,
			CameraUp
		}

		public enum PressureType
		{
			Constant,
			ImpactSpeed
		}

		/// <summary>This allows you to control the hit data this component sends out.
		/// PointsIn3D = Point drawing in 3D.
		/// PointsOnUV = Point drawing on UV (requires non-convex <b>MeshCollider</b>).
		/// TrianglesIn3D = Triangle drawing in 3D.</summary>
		public EmitType Emit { set { emit = value; } get { return emit; } } [SerializeField] private EmitType emit;

		/// <summary>When emitting <b>PointsOnUV</b> or <b>TrianglesIn3D</b>, this setting allows you to specify the world space distance from the hit point a raycast will be fired. This is necessary because collisions by themselves don't provide the necessary information.
		/// NOTE: Performing this raycast has a slight performance penalty.</summary>
		public float RaycastDistance { set { raycastDistance = value; } get { return raycastDistance; } } [SerializeField] private float raycastDistance = 0.0001f;

		/// <summary>This allows you to filter collisions to specific layers.</summary>
		public LayerMask Layers { set { layers = value; } get { return layers; } } [SerializeField] private LayerMask layers = -1;

		/// <summary>If there are multiple contact points, skip them?</summary>
		public bool OnlyUseFirstContact { set { onlyUseFirstContact = value; } get { return onlyUseFirstContact; } } [SerializeField] private bool onlyUseFirstContact = true;

		/// <summary>If this component is generating too many hits, then you can use this setting to ignore hits for the specified amount of seconds.
		/// 0 = Unlimited.</summary>
		public float Delay { set { delay = value; } get { return delay; } } [SerializeField] private float delay;

		/// <summary>How should the hit point be oriented?
		/// WorldUp = It will be rotated to the normal, where the up vector is world up.
		/// CameraUp = It will be rotated to the normal, where the up vector is world up.</summary>
		public OrientationType Orientation { set { orientation = value; } get { return orientation; } } [SerializeField] private OrientationType orientation;

		/// <summary>Orient to a specific camera?
		/// None = MainCamera.</summary>
		public Camera Camera { set { _camera = value; } get { return _camera; } } [SerializeField] private Camera _camera;

		/// <summary>Should the applied paint be applied as a preview?</summary>
		public bool Preview { set { preview = value; } get { return preview; } } [SerializeField] private bool preview;

		/// <summary>If the collision impact speed is below this value, then the collision will be ignored.</summary>
		public float Threshold { set { threshold = value; } get { return threshold; } } [SerializeField] private float threshold = 50.0f;

		/// <summary>This allows you to set how the pressure value will be calculated.
		/// Constant = The <b>PressureConstant</b> value will be directly used.
		/// ImpactSpeed = The pressure will be 0 when the collision impact speed is <b>PressureMin</b>, and 1 when the impact speed is or exceeds <b>PressureMax</b>.</summary>
		public PressureType PressureMode { set { pressureMode = value; } get { return pressureMode; } } [SerializeField] private PressureType pressureMode = PressureType.ImpactSpeed;

		/// <summary>The impact strength required for a hit to occur with a pressure of 0.</summary>
		public float PressureMin { set { pressureMin = value; } get { return pressureMin; } } [SerializeField] private float pressureMin = 50.0f;

		/// <summary>The impact strength required for a hit to occur with a pressure of 1.</summary>
		public float PressureMax { set { pressureMax = value; } get { return pressureMax; } } [SerializeField] private float pressureMax = 100.0f;

		/// <summary>The pressure value used when <b>PressureMode</b> is set to <b>Constant</b>.</summary>
		public float PressureConstant { set { pressureConstant = value; } get { return pressureConstant; } } [SerializeField] [Range(0.0f, 1.0f)] private float pressureConstant = 1.0f;

		/// <summary>The calculated pressure value will be multiplied by this.</summary>
		public float PressureMultiplier { set { pressureMultiplier = value; } get { return pressureMultiplier; } } [SerializeField] private float pressureMultiplier = 1.0f;

		/// <summary>If you want the raycast hit point to be offset from the surface a bit, this allows you to set by how much in world space.</summary>
		public float Offset { set { offset = value; } get { return offset; } } [SerializeField] private float offset;

		/// <summary>This allows you to override the order this paint gets applied to the object during the current frame.</summary>
		public int Priority { set { priority = value; } get { return priority; } } [SerializeField] private int priority;

		/// <summary>Hit events are normally sent to all components attached to the current GameObject, but this setting allows you to override that. This is useful if you want to use multiple <b>P3dHitCollisions</b> components with different settings and results.</summary>
		public GameObject Root { set { ClearHitCache(); root = value; } get { return root; } } [SerializeField] private GameObject root;

		[SerializeField]
		private float cooldown;

		[System.NonSerialized]
		private P3dHitCache hitCache = new P3dHitCache();

		public P3dHitCache HitCache
		{
			get
			{
				return hitCache;
			}
		}

		/// <summary>This component sends hit events to a cached list of components that can receive them. If this list changes then you must manually call this method.</summary>
		[ContextMenu("Clear Hit Cache")]
		public void ClearHitCache()
		{
			hitCache.Clear();
		}

		protected virtual void OnCollisionEnter(Collision collision)
		{
			CheckCollision(collision);
		}

		protected virtual void OnCollisionStay(Collision collision)
		{
			CheckCollision(collision);
		}

		protected virtual void Update()
		{
			cooldown -= Time.deltaTime;
		}

		private bool TryGetRaycastHit(ContactPoint contact, ref RaycastHit hit)
		{
			if (raycastDistance > 0.0f)
			{
				var ray = new Ray(contact.point + contact.normal * raycastDistance, -contact.normal);

				if (contact.otherCollider.Raycast(ray, out hit, raycastDistance * 2.0f) == true)
				{
					return true;
				}
			}

			return false;
		}

		private void CheckCollision(Collision collision)
		{
			if (cooldown > 0.0f)
			{
				return;
			}

			var impulse = collision.impulse.magnitude / Time.fixedDeltaTime;

			// Only handle the collision if the impact was strong enough
			if (impulse >= pressureMin)
			{
				cooldown = delay;

				// Calculate up vector ahead of time
				var finalUp       = orientation == OrientationType.CameraUp ? P3dCommon.GetCameraUp(_camera) : Vector3.up;
				var contacts      = collision.contacts;
				var finalPressure = pressureMultiplier;
				var finalRoot     = root != null ? root : gameObject;

				switch (pressureMode)
				{
					case PressureType.Constant:
					{
						finalPressure *= pressureConstant;
					}
					break;

					case PressureType.ImpactSpeed:
					{
						finalPressure *= Mathf.InverseLerp(pressureMin, pressureMax, impulse);
					}
					break;
				}

				for (var i = contacts.Length - 1; i >= 0; i--)
				{
					var contact = contacts[i];

					if (CwHelper.IndexInMask(contact.otherCollider.gameObject.layer, layers) == true)
					{
						var finalPosition = contact.point + contact.normal * offset;
						var finalRotation = Quaternion.LookRotation(-contact.normal, finalUp);

						switch (emit)
						{
							case EmitType.PointsIn3D:
							{
								hitCache.InvokePoint(finalRoot, preview, priority, finalPressure, finalPosition, finalRotation);
							}
							break;

							case EmitType.PointsOnUV:
							{
								var hit = default(RaycastHit);

								if (TryGetRaycastHit(contact, ref hit) == true)
								{
									hitCache.InvokeCoord(finalRoot, preview, priority, finalPressure, new P3dHit(hit), finalRotation);
								}
							}
							break;

							case EmitType.TrianglesIn3D:
							{
								var hit = default(RaycastHit);

								if (TryGetRaycastHit(contact, ref hit) == true)
								{
									hitCache.InvokeTriangle(gameObject, preview, priority, finalPressure, hit, finalRotation);
								}
							}
							break;
						}

						if (onlyUseFirstContact == true)
						{
							break;
						}
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
	using TARGET = P3dHitCollisions;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dHitCollisions_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("emit", "This allows you to control the hit data this component sends out.\n\nPoints = Point drawing in 3D.\n\nPointsOnUV = Point drawing on UV (requires non-convex MeshCollider).\n\nTrianglesIn3D = Triangle drawing in 3D.");
			if (Any(tgts, t => t.Emit != P3dHitCollisions.EmitType.PointsIn3D))
			{
				BeginIndent();
					BeginError(Any(tgts, t => t.RaycastDistance <= 0.0f));
						Draw("raycastDistance", "When emitting PointsOnUV or TrianglesIn3D, this setting allows you to specify the world space distance from the hit point a raycast will be fired. This is necessary because collisions by themselves don't provide the necessary information.\n\nNOTE: Performing this raycast has a slight performance penalty.");
					EndError();
				EndIndent();
			}
			Draw("layers", "This allows you to filter collisions to specific layers.");

			Separator();

			Draw("onlyUseFirstContact", "If there are multiple contact points, skip them?");
			BeginError(Any(tgts, t => t.Delay < 0.0f));
				Draw("delay", "If this component is generating too many hits, then you can use this setting to ignore hits for the specified amount of seconds.\n\n0 = Unlimited.");
			EndError();
			
			Draw("orientation", "How should the hit point be oriented?\nNone = It will be treated as a point with no rotation.\n\nWorldUp = It will be rotated to the normal, where the up vector is world up.\n\nCameraUp = It will be rotated to the normal, where the up vector is world up.");
			BeginIndent();
				if (Any(tgts, t => t.Orientation == P3dHitCollisions.OrientationType.CameraUp))
				{
					Draw("_camera", "Orient to a specific camera?\nNone = MainCamera.");
				}
			EndIndent();

			Separator();

			Draw("preview", "Should the applied paint be applied as a preview?");
			Draw("threshold", "If the collision impact speed is below this value, then the collision will be ignored.");
			Draw("pressureMode", "This allows you to set how the pressure value will be calculated.\n\nConstant = The <b>PressureConstant</b> value will be directly used.\n\nImpactSpeed = The pressure will be 0 when the collision impact speed is <b>PressureMin</b>, and 1 when the impact speed is or exceeds <b>PressureMax</b>.");
			BeginIndent();
				if (Any(tgts, t => t.PressureMode == P3dHitCollisions.PressureType.Constant))
				{
					Draw("pressureConstant", "The pressure value used when PressureMode is set to Constant.", "Constant");
				}
				if (Any(tgts, t => t.PressureMode == P3dHitCollisions.PressureType.ImpactSpeed))
				{
					Draw("pressureMin", "The impact strength required for a hit to occur with a pressure of 0.", "Min");
					Draw("pressureMax", "The impact strength required for a hit to occur with a pressure of 1.", "Max");
				}
				Draw("pressureMultiplier", "The calculated pressure value will be multiplied by this.", "Multiplier");
			EndIndent();

			Separator();

			if (DrawFoldout("Advanced", "Show advanced settings?") == true)
			{
				BeginIndent();
					Draw("offset", "If you want the raycast hit point to be offset from the surface a bit, this allows you to set by how much in world space.");
					Draw("priority", "This allows you to override the order this paint gets applied to the object during the current frame.");
					Draw("root", "Hit events are normally sent to all components attached to the current GameObject, but this setting allows you to override that. This is useful if you want to use multiple P3dHitCollisions components with different settings and results.");
				EndIndent();
			}

			Separator();

			var point    = tgt.Emit == P3dHitCollisions.EmitType.PointsIn3D;
			var triangle = tgt.Emit == P3dHitCollisions.EmitType.TrianglesIn3D;
			var coord    = tgt.Emit == P3dHitCollisions.EmitType.PointsOnUV;

			tgt.HitCache.Inspector(tgt.Root != null ? tgt.Root : tgt.gameObject, point: point, triangle: triangle, coord: coord);
		}
	}
}
#endif