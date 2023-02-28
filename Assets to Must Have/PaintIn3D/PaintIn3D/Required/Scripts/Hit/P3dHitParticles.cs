using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component can be added to any ParticleSystem with collisions enabled, and it will fire hits when the particles collide with something.</summary>
	[RequireComponent(typeof(ParticleSystem))]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dHitParticles")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Hit Particles")]
	public class P3dHitParticles : MonoBehaviour
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

		public enum NormalType
		{
			ParticleVelocity,
			CollisionNormal
		}

		public enum PressureType
		{
			Constant,
			Distance,
			Speed
		}

		/// <summary>This allows you to control the hit data this component sends out.
		/// PointsIn3D = Point drawing in 3D.
		/// PointsOnUV = Point drawing on UV (requires non-convex <b>MeshCollider</b>).
		/// TrianglesIn3D = Triangle drawing in 3D.</summary>
		public EmitType Emit { set { emit = value; } get { return emit; } } [SerializeField] private EmitType emit;

		/// <summary>When emitting <b>PointsOnUV</b> or <b>TrianglesIn3D</b>, this setting allows you to specify the world space distance from the hit point a raycast will be fired. This is necessary because particles by themselves don't provide the necessary information.
		/// NOTE: Performing this raycast has a slight performance penalty.</summary>
		public float RaycastDistance { set { raycastDistance = value; } get { return raycastDistance; } } [SerializeField] private float raycastDistance = 0.0001f;

		/// <summary>This allows you to filter collisions to specific layers.</summary>
		public LayerMask Layers { set { layers = value; } get { return layers; } } [SerializeField] private LayerMask layers = -1;

		/// <summary>How should the hit point be oriented?
		/// WorldUp = It will be rotated to the normal, where the up vector is world up.
		/// CameraUp = It will be rotated to the normal, where the up vector is world up.</summary>
		public OrientationType Orientation { set { orientation = value; } get { return orientation; } } [SerializeField] private OrientationType orientation;

		/// <summary>Orient to a specific camera?
		/// None = MainCamera.</summary>
		public Camera Camera { set { _camera = value; } get { return _camera; } } [SerializeField] private Camera _camera;

		/// <summary>Which normal should the hit point rotation be based on?</summary>
		public NormalType Normal { set { normal = value; } get { return normal; } } [SerializeField] private NormalType normal;

		/// <summary>If you want the raycast hit point to be offset from the surface a bit, this allows you to set by how much in world space.</summary>
		public float Offset { set { offset = value; } get { return offset; } } [SerializeField] private float offset;

		/// <summary>If you have too many particles, then painting can slow down. This setting allows you to reduce the amount of particles that actually cause hits.
		/// 0 = Every particle will hit.
		/// 5 = Skip 5 particles, then hit using the 6th.</summary>
		public int Skip { set { skip = value; } get { return skip; } } [SerializeField] private int skip;

		/// <summary>Should the particles paint preview paint?</summary>
		public bool Preview { set { preview = value; } get { return preview; } } [SerializeField] private bool preview;

		/// <summary>This allows you to override the order this paint gets applied to the object during the current frame.</summary>
		public int Priority { set { priority = value; } get { return priority; } } [SerializeField] private int priority;

		/// <summary>This allows you to set how the pressure value will be calculated.
		/// Constant = The <b>PressureConstant</b> value will be directly used.
		/// Distance = A value will be calculated based on the distance between this emitter and the particle hit point.
		/// Speed = A value will be calculated based on the hit speed of the particle.</summary>
		public PressureType PressureMode { set { pressureMode = value; } get { return pressureMode; } } [SerializeField] private PressureType pressureMode = PressureType.Constant;

		/// <summary>This allows you to specify the distance/speed that gives 0.0 pressure.</summary>
		public float PressureMin { set { pressureMin = value; } get { return pressureMin; } } [SerializeField] private float pressureMin;

		/// <summary>This allows you to specify the distance/speed that gives 1.0 pressure.</summary>
		public float PressureMax { set { pressureMax = value; } get { return pressureMax; } } [SerializeField] private float pressureMax;

		/// <summary>The pressure value used when <b>PressureMode</b> is set to <b>Constant</b>.</summary>
		public float PressureConstant { set { pressureConstant = value; } get { return pressureConstant; } } [SerializeField] [Range(0.0f, 1.0f)] private float pressureConstant = 1.0f;

		/// <summary>The calculated pressure value will be multiplied by this.</summary>
		public float PressureMultiplier { set { pressureMultiplier = value; } get { return pressureMultiplier; } } [SerializeField] private float pressureMultiplier = 1.0f;

		/// <summary>Hit events are normally sent to all components attached to the current GameObject, but this setting allows you to override that. This is useful if you want to use multiple <b>P3dHitParticles</b> components with different settings and results.</summary>
		public GameObject Root { set { ClearHitCache(); root = value; } get { return root; } } [SerializeField] private GameObject root;

		[System.NonSerialized]
		private ParticleSystem cachedParticleSystem;

		[System.NonSerialized]
		private bool cachedParticleSystemSet;

		[System.NonSerialized]
		private static List<ParticleCollisionEvent> particleCollisionEvents = new List<ParticleCollisionEvent>();

		[System.NonSerialized]
		private P3dHitCache hitCache = new P3dHitCache();

		[System.NonSerialized]
		private int skipCounter;

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

		private bool TryGetRaycastHit(ParticleCollisionEvent collision, ref RaycastHit hit)
		{
			if (raycastDistance > 0.0f)
			{
				var collider = collision.colliderComponent as Collider;

				if (collider != null)
				{
					var ray = new Ray(collision.intersection + collision.normal * raycastDistance, -collision.normal);

					if (collider.Raycast(ray, out hit, raycastDistance * 2.0f) == true)
					{
						return true;
					}
				}
			}

			return false;
		}

		protected virtual void OnParticleCollision(GameObject hitGameObject)
		{
			if (cachedParticleSystemSet == false)
			{
				cachedParticleSystem    = GetComponent<ParticleSystem>();
				cachedParticleSystemSet = true;
			}

			// Get the collision events array
			var count = cachedParticleSystem.GetSafeCollisionEventSize();

			// Expand collisionEvents list to fit all particles
			for (var i = particleCollisionEvents.Count; i < count; i++)
			{
				particleCollisionEvents.Add(new ParticleCollisionEvent());
			}

			count = cachedParticleSystem.GetCollisionEvents(hitGameObject, particleCollisionEvents);

			// Calculate up vector ahead of time
			var finalUp   = orientation == OrientationType.CameraUp ? P3dCommon.GetCameraUp(_camera) : Vector3.up;
			var finalRoot = root != null ? root : gameObject;

			// Paint all locations
			for (var i = 0; i < count; i++)
			{
				var collision = particleCollisionEvents[i];

				if (CwHelper.IndexInMask(collision.colliderComponent.gameObject.layer, layers) == false)
				{
					continue;
				}

				if (skip > 0)
				{
					if (skipCounter++ > skip)
					{
						skipCounter = 0;
					}
					else
					{
						continue;
					}
				}

				var finalPosition = collision.intersection + collision.normal * offset;
				var finalNormal   = normal == NormalType.CollisionNormal ? collision.normal : -collision.velocity;
				var finalRotation = finalNormal != Vector3.zero ? Quaternion.LookRotation(-finalNormal, finalUp) : Quaternion.identity;
				var finalPressure = pressureMultiplier;

				switch (pressureMode)
				{
					case PressureType.Constant:
					{
						finalPressure *= pressureConstant;
					}
					break;

					case PressureType.Distance:
					{
						var distance = Vector3.Distance(transform.position, collision.intersection);

						finalPressure *= Mathf.InverseLerp(pressureMin, pressureMax, distance);
					}
					break;

					case PressureType.Speed:
					{
						var speed = Vector3.SqrMagnitude(collision.velocity);

						if (speed > 0.0f)
						{
							speed = Mathf.Sqrt(speed);
						}

						finalPressure *= Mathf.InverseLerp(pressureMin, pressureMax, speed);
					}
					break;
				}

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

						if (TryGetRaycastHit(collision, ref hit) == true)
						{
							hitCache.InvokeCoord(finalRoot, preview, priority, finalPressure, new P3dHit(hit), finalRotation);
						}
					}
					break;

					case EmitType.TrianglesIn3D:
					{
						var hit = default(RaycastHit);

						if (TryGetRaycastHit(collision, ref hit) == true)
						{
							hitCache.InvokeTriangle(gameObject, preview, priority, finalPressure, hit, finalRotation);
						}
					}
					break;
				}
			}
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(transform.position, pressureMin);
			Gizmos.DrawWireSphere(transform.position, pressureMax);
		}
#endif
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dHitParticles;

	[CustomEditor(typeof(TARGET))]
	public class P3dHitParticles_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("emit", "This allows you to control the hit data this component sends out.\n\nPoints = Point drawing in 3D.\n\nPointsOnUV = Point drawing on UV (requires non-convex MeshCollider).\n\nTrianglesIn3D = Triangle drawing in 3D.");
			if (Any(tgts, t => t.Emit != P3dHitParticles.EmitType.PointsIn3D))
			{
				BeginIndent();
					BeginError(Any(tgts, t => t.RaycastDistance <= 0.0f));
						Draw("raycastDistance", "When emitting PointsOnUV or TrianglesIn3D, this setting allows you to specify the world space distance from the hit point a raycast will be fired. This is necessary because particles by themselves don't provide the necessary information.\n\nNOTE: Performing this raycast has a slight performance penalty.");
					EndError();
				EndIndent();
			}
			Draw("layers", "This allows you to filter collisions to specific layers.");

			Separator();

			Draw("orientation", "How should the hit point be oriented?\n\nNone = It will be treated as a point with no rotation.\n\nWorldUp = It will be rotated to the normal, where the up vector is world up.\n\nCameraUp = It will be rotated to the normal, where the up vector is world up.");
			BeginIndent();
				if (Any(tgts, t => t.Orientation == P3dHitParticles.OrientationType.CameraUp))
				{
					Draw("_camera", "Orient to a specific camera?\n\nNone = MainCamera.");
				}
			EndIndent();
			Draw("normal", "Which normal should the hit point rotation be based on?");

			Separator();
			
			Draw("preview", "Should the particles paint preview paint?");
			Draw("pressureMode", "This allows you to set how the pressure value will be calculated.\n\nConstant = The PressureConstant value will be directly used.\n\nDistance = A value will be calculated based on the distance between this emitter and the particle hit point.\n\nVelocity = A value will be calculated based on the hit velocity of the particle.");
			BeginIndent();
				if (Any(tgts, t => t.PressureMode == P3dHitParticles.PressureType.Constant))
				{
					Draw("pressureConstant", "The pressure value used when PressureMode is set to Constant.", "Constant");
				}
				if (Any(tgts, t => t.PressureMode == P3dHitParticles.PressureType.Distance))
				{
					Draw("pressureMin", "This allows you to set the world space distance from this emitter where the particle hit point will register as having 0.0 pressure.", "Min");
					Draw("pressureMax", "This allows you to set the world space distance from this emitter where the particle hit point will register as having 1.0 pressure.", "Max");
				}
				else if (Any(tgts, t => t.PressureMode == P3dHitParticles.PressureType.Speed))
				{
					Draw("pressureMin", "This allows you to set the particle speed where the hit will register as having 0.0 pressure.", "Min");
					Draw("pressureMax", "This allows you to set the particle speed where the hit will register as having 1.0 pressure.", "Max");
				}
				Draw("pressureMultiplier", "The calculated pressure value will be multiplied by this.", "Multiplier");
			EndIndent();

			Separator();

			if (DrawFoldout("Advanced", "Show advanced settings?") == true)
			{
				BeginIndent();
					Draw("skip", "If you have too many particles, then painting can slow down. This setting allows you to reduce the amount of particles that actually cause hits.\n\n0 = Every particle will hit.\n\n5 = Skip 5 particles, then hit using the 6th.");
					Draw("offset", "If you want the raycast hit point to be offset from the surface a bit, this allows you to set by how much in world space.");
					Draw("priority", "This allows you to override the order this paint gets applied to the object during the current frame.");
					Draw("root", "Hit events are normally sent to all components attached to the current GameObject, but this setting allows you to override that. This is useful if you want to use multiple P3dHitCollisions components with different settings and results.");
				EndIndent();
			}

			Separator();

			var point    = tgt.Emit == P3dHitParticles.EmitType.PointsIn3D;
			var triangle = tgt.Emit == P3dHitParticles.EmitType.TrianglesIn3D;
			var coord    = tgt.Emit == P3dHitParticles.EmitType.PointsOnUV;

			tgt.HitCache.Inspector(tgt.Root != null ? tgt.Root : tgt.gameObject, point: point, triangle: triangle, coord: coord);
		}
	}
}
#endif