using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component raycasts between two points, and fires hit events when the ray hits something.</summary>
	[ExecuteInEditMode]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dHitBetween")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Hit Between")]
	public class P3dHitBetween : MonoBehaviour
	{
		public enum PhaseType
		{
			Update,
			FixedUpdate
		}

		public enum OrientationType
		{
			WorldUp,
			CameraUp,
			ThisRotation,
			ThisLocalRotation,
			CustomRotation,
			CustomLocalRotation
		}

		public enum NormalType
		{
			HitNormal,
			RayDirection
		}

		public enum EmitType
		{
			PointsIn3D    = 0,
			PointsOnUV    = 20,
			TrianglesIn3D = 30
		}

		/// <summary>Where in the game loop should this component hit?</summary>
		public PhaseType PaintIn { set { paintIn = value; } get { return paintIn; } } [SerializeField] private PhaseType paintIn;

		/// <summary>The time in seconds between each raycast.
		/// 0 = Every frame.
		/// -1 = Manual only.</summary>
		public float Interval { set { interval = value; } get { return interval; } } [SerializeField] private float interval = 0.05f;

		/// <summary>The start point of the raycast.</summary>
		public Transform PointA { set { pointA = value; } get { return pointA; } } [SerializeField] private Transform pointA;

		/// <summary>The end point of the raycast.</summary>
		public Transform PointB { set { pointB = value; } get { return pointB; } } [SerializeField] private Transform pointB;

		/// <summary>The end point of the raycast.</summary>
		public float Fraction { get { return fraction; } } [SerializeField] private float fraction = 1.0f;

		/// <summary>The layers you want the raycast to hit.</summary>
		public LayerMask Layers { set { layers = value; } get { return layers; } } [SerializeField] private LayerMask layers = Physics.DefaultRaycastLayers;

		/// <summary>How should the hit point be oriented?
		/// WorldUp = It will be rotated to the normal, where the up vector is world up.
		/// CameraUp = It will be rotated to the normal, where the up vector is world up.
		/// ThisRotation = The current <b>Transform.rotation</b> will be used.
		/// ThisLocalRotation = The current <b>Transform.localRotation</b> will be used.
		/// CustomRotation = The specified <b>CustomTransform.rotation</b> will be used.
		/// CustomLocalRotation = The specified <b>CustomTransform.localRotation</b> will be used.</summary>
		public OrientationType Orientation { set { orientation = value; } get { return orientation; } } [SerializeField] private OrientationType orientation;

		/// <summary>Orient to a specific camera?
		/// None = MainCamera.</summary>
		public Camera Camera { set { _camera = value; } get { return _camera; } } [SerializeField] private Camera _camera;

		/// <summary>If you use <b>Orientation = CustomRotation/CustomLocalRotation</b>, this allows you to set the transform.</summary>
		public Transform CustomTransform { set { customTransform = value; } get { return customTransform; } } [SerializeField] private Transform customTransform;

		/// <summary>Which normal should the hit point rotation be based on?</summary>
		public NormalType Normal { set { normal = value; } get { return normal; } } [SerializeField] private NormalType normal;

		/// <summary>If you want the raycast hit point to be offset from the surface a bit, this allows you to set by how much in world space.</summary>
		public float Offset { set { offset = value; } get { return offset; } } [SerializeField] private float offset;

		/// <summary>Should the applied paint be applied as a preview?</summary>
		public bool Preview { set { preview = value; } get { return preview; } } [SerializeField] private bool preview;

		/// <summary>This allows you to override the order this paint gets applied to the object during the current frame.</summary>
		public int Priority { set { priority = value; } get { return priority; } } [SerializeField] private int priority;

		/// <summary>This allows you to control the pressure of the painting. This could be controlled by a VR trigger or similar for more advanced effects.</summary>
		public float Pressure { set { pressure = value; } get { return pressure; } } [Range(0.0f, 1.0f)] [SerializeField] private float pressure = 1.0f;

		/// <summary>This allows you to control the hit data this component sends out.
		/// PointsIn3D = Point drawing in 3D.
		/// PointsOnUV = Point drawing on UV (requires non-convex <b>MeshCollider</b>).
		/// TrianglesIn3D = Triangle drawing in 3D.</summary>
		public EmitType Draw { set { emit = value; } get { return emit; } } [SerializeField] private EmitType emit;

		/// <summary>If you want to display something at the hit point (e.g. particles), you can specify the Transform here.</summary>
		public Transform Point { set { point = value; } get { return point; } } [SerializeField] private Transform point;

		/// <summary>If you want to draw a line between the start point and the his point then you can set the line here.</summary>
		public LineRenderer Line { set { line = value; } get { return line; } } [SerializeField] private LineRenderer line;

		/// <summary>This allows you to connect the hit points together to form lines.</summary>
		public P3dPointConnector Connector { get { if (connector == null) connector = new P3dPointConnector(); return connector; } } [SerializeField] private P3dPointConnector connector;

		[System.NonSerialized]
		private float current;

		/// <summary>This method will immediately submit a non-preview hit. This can be used to apply real paint to your objects.</summary>
		[ContextMenu("Manually Hit Now")]
		public void ManuallyHitNow()
		{
			SubmitHit(false);
		}

		/// <summary>This component sends hit events to a cached list of components that can receive them. If this list changes then you must manually call this method.</summary>
		[ContextMenu("Clear Hit Cache")]
		public void ClearHitCache()
		{
			Connector.ClearHitCache();
		}

		/// <summary>If this GameObject has teleported and you have <b>ConnectHits</b> or <b>HitSpacing</b> enabled, then you can call this to prevent a line being drawn between the previous and current points.</summary>
		[ContextMenu("Reset Connections")]
		public void ResetConnections()
		{
			connector.ResetConnections();
		}

		protected virtual void OnEnable()
		{
			Connector.ResetConnections();
		}

		protected virtual void OnDisable()
		{
			if (point != null && pointB != null)
			{
				point.position = pointB.position;
			}
		}

		protected virtual void Update()
		{
			connector.Update();

			if (preview == true)
			{
				SubmitHit(true);
			}
			else if (paintIn == PhaseType.Update)
			{
				UpdateHit();
			}
		}

		protected virtual void LateUpdate()
		{
			UpdatePointAndLine();
		}

		protected virtual void FixedUpdate()
		{
			if (preview == false && paintIn == PhaseType.FixedUpdate)
			{
				UpdateHit();
			}
		}

		private void SubmitHit(bool preview)
		{
			if (pointA != null && pointB != null)
			{
				var vector        = pointB.position - pointA.position;
				var maxDistance   = vector.magnitude;
				var ray           = new Ray(pointA.position, vector);
				var hit2D         = Physics2D.GetRayIntersection(ray, float.PositiveInfinity, layers);
				var hit3D         = default(RaycastHit);
				var finalPosition = default(Vector3);
				var finalRotation = default(Quaternion);

				// Hit 3D?
				if (Physics.Raycast(ray, out hit3D, maxDistance, layers) == true && (hit2D.collider == null || hit3D.distance < hit2D.distance))
				{
					CalcHitData(hit3D.point, hit3D.normal, ray, out finalPosition, out finalRotation);

					fraction = (hit3D.distance + offset) / maxDistance;

					if (emit == EmitType.PointsIn3D)
					{
						connector.SubmitPoint(gameObject, preview, priority, pressure, finalPosition, finalRotation, this);
					}
					else if (emit == EmitType.PointsOnUV)
					{
						connector.HitCache.InvokeCoord(gameObject, preview, priority, pressure, new P3dHit(hit3D), finalRotation);
					}
					else if (emit == EmitType.TrianglesIn3D)
					{
						connector.HitCache.InvokeTriangle(gameObject, preview, priority, pressure, hit3D, finalRotation);
					}
				}
				// Hit 2D?
				else if (hit2D.collider != null)
				{
					CalcHitData(hit2D.point, hit2D.normal, ray, out finalPosition, out finalRotation);

					fraction = (hit3D.distance + offset) / maxDistance;

					if (emit == EmitType.PointsIn3D)
					{
						connector.SubmitPoint(gameObject, preview, priority, pressure, finalPosition, finalRotation, this);
					}
				}
				else
				{
					connector.BreakHits(this);

					fraction = 1.0f;
				}
			}
		}

		private void CalcHitData(Vector3 hitPoint, Vector3 hitNormal, Ray ray, out Vector3 finalPosition, out Quaternion finalRotation)
		{
			finalPosition = hitPoint + hitNormal * offset;

			switch (orientation)
			{
				case OrientationType.WorldUp:
				{
					var finalUp     = Vector3.up;
					var finalNormal = normal == NormalType.HitNormal ? hitNormal : -ray.direction;

					finalRotation = Quaternion.LookRotation(-finalNormal, finalUp);
				}
				break;

				case OrientationType.CameraUp:
				{
					var finalUp     = P3dCommon.GetCameraUp(_camera);
					var finalNormal = normal == NormalType.HitNormal ? hitNormal : -ray.direction;

					finalRotation = Quaternion.LookRotation(-finalNormal, finalUp);
				}
				return;

				case OrientationType.ThisRotation:
				{
					finalRotation = transform.rotation;
				}
				return;

				case OrientationType.ThisLocalRotation:
				{
					finalRotation = transform.localRotation;
				}
				return;

				case OrientationType.CustomRotation:
				{
					if (customTransform != null)
					{
						finalRotation = customTransform.rotation;

						return;
					}
				}
				break;

				case OrientationType.CustomLocalRotation:
				{
					if (customTransform != null)
					{
						finalRotation = customTransform.localRotation;

						return;
					}
				}
				break;
			}
			
			finalRotation = Quaternion.identity;
		}

		private void UpdatePointAndLine()
		{
			if (pointA != null && pointB != null)
			{
				var a = pointA.position;
				var b = pointB.position;
				var m = Vector3.Lerp(a, b, fraction);

				if (point != null)
				{
					point.position = m;
				}

				if (line != null)
				{
					line.positionCount = 2;

					line.SetPosition(0, a);
					line.SetPosition(1, m);
				}
			}
		}

		private void UpdateHit()
		{
			current += Time.deltaTime;

			if (interval > 0.0f)
			{
				if (current >= interval)
				{
					current %= interval;

					SubmitHit(false);
				}
			}
			else if (interval == 0.0f)
			{
				SubmitHit(false);
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dHitBetween;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dHitBetween_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("emit", "This allows you to control the hit data this component sends out.\n\nPointsIn3D = Point drawing in 3D.\n\nPointsOnUV = Point drawing on UV (requires non-convex MeshCollider).\n\nTrianglesIn3D = Triangle drawing in 3D.");
			BeginError(Any(tgts, t => t.Layers == 0));
				Draw("layers", "The layers you want the raycast to hit.");
			EndError();
			Draw("paintIn", "Where in the game loop should this component hit?");
			Draw("interval", "The time in seconds between each raycast.\n\n0 = Every Frame\n\n-1 = Manual Only");

			Separator();

			BeginError(Any(tgts, t => t.PointA == null));
				Draw("pointA", "The start point of the raycast.");
			EndError();
			BeginError(Any(tgts, t => t.PointB == null));
				Draw("pointB", "The end point of the raycast.");
			EndError();

			Draw("orientation", "How should the hit point be oriented?\n\nWorldUp = It will be rotated to the normal, where the up vector is world up.\n\nCameraUp = It will be rotated to the normal, where the up vector is world up.\n\nThisRotation = The current <b>Transform.rotation</b> will be used.\n\nThisLocalRotation = The current <b>Transform.localRotation</b> will be used.\n\nCustomRotation = The specified <b>CustomTransform.rotation</b> will be used.\n\nCustomLocalRotation = The specified <b>CustomTransform.localRotation</b> will be used.");
			BeginIndent();
				if (Any(tgts, t => t.Orientation == P3dHitBetween.OrientationType.CameraUp))
				{
					Draw("_camera", "Orient to a specific camera?\nNone = MainCamera.");
				}
				if (Any(tgts, t => t.Orientation == P3dHitBetween.OrientationType.WorldUp || t.Orientation == P3dHitBetween.OrientationType.CameraUp))
				{
					Draw("normal", "Which normal should the hit point rotation be based on?");
				}
				if (Any(tgts, t => t.Orientation == P3dHitBetween.OrientationType.CustomRotation || t.Orientation == P3dHitBetween.OrientationType.CustomLocalRotation))
				{
					Draw("customTransform", "If you use <b>Orientation = CustomRotation/CustomLocalRotation</b>, this allows you to set the transform.");
				}
			EndIndent();

			Separator();

			Draw("preview", "Should the applied paint be applied as a preview?");
			Draw("pressure", "This allows you to control the pressure of the painting. This could be controlled by a VR trigger or similar for more advanced effects.");

			Separator();

			if (DrawFoldout("Advanced", "Show advanced settings?") == true)
			{
				BeginIndent();
					Draw("priority", "This allows you to override the order this paint gets applied to the object during the current frame.");
					Draw("offset", "If you want the raycast hit point to be offset from the surface a bit, this allows you to set by how much in world space.");

					Separator();

					P3dPointConnector_Editor.Draw();

					Separator();

					Draw("point", "If you want to display something at the hit point (e.g. particles), you can specify the Transform here.");
					Draw("line", "If you want to draw a line between the start point and the his point then you can set the line here");
				EndIndent();
			}

			Separator();

			var point    = tgt.Draw == P3dHitBetween.EmitType.PointsIn3D;
			var line     = tgt.Draw == P3dHitBetween.EmitType.PointsIn3D && tgt.Connector.ConnectHits == true;
			var triangle = tgt.Draw == P3dHitBetween.EmitType.TrianglesIn3D;
			var coord    = tgt.Draw == P3dHitBetween.EmitType.PointsOnUV;

			tgt.Connector.HitCache.Inspector(tgt.gameObject, point: point, line: line, triangle: triangle, coord: coord);
		}
	}
}
#endif