using System.Collections.Generic;
using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This allows you to spawn a prefab at a hit point. Hit points will automatically be sent by any <b>P3dHit___</b> component on this GameObject, or its ancestors.</summary>
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dSpawner")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Spawner")]
	public class P3dSpawner : MonoBehaviour, IHitPoint
	{
		/// <summary>A random prefab from this list will be spawned.</summary>
		public List<GameObject> Prefabs { get { if (prefabs == null) prefabs = new List<GameObject>(); return prefabs; } } [SerializeField] private List<GameObject> prefabs;

		/// <summary>The spawned prefab will be randomly offset by a random point within this radius in world space.</summary>
		public float Radius { set { radius = value; } get { return radius; } } [SerializeField] private float radius;

		/// <summary>If the prefab contains a <b>Rigidbody</b>, it will be given this velocity in local space.</summary>
		public Vector3 Velocity { set { velocity = value; } get { return velocity; } } [SerializeField] private Vector3 velocity;

		/// <summary>The spawned prefab will be offset from the hit point based on the hit normal by this value in world space.</summary>
		public float OffsetNormal { set { offsetNormal = value; } get { return offsetNormal; } } [SerializeField] private float offsetNormal;

		/// <summary>The spawned prefab will be offset from the hit point based on this value in world space.</summary>
		public Vector3 OffsetWorld { set { offsetWorld = value; } get { return offsetWorld; } } [SerializeField] private Vector3 offsetWorld;

		// Legacy
		[SerializeField] private GameObject prefab;

		/// <summary>Call this if you want to manually spawn the specified prefab.</summary>
		public void Spawn()
		{
			Spawn(transform.position, transform.rotation);
		}

		public void Spawn(Vector3 position, Vector3 normal)
		{
			Spawn(position, Quaternion.LookRotation(normal));
		}

		public void Spawn(Vector3 position, Quaternion rotation)
		{
			UpgradeLegacy();

			if (prefabs != null && prefabs.Count > 0)
			{
				var finalPrefab = prefabs[Random.Range(0, prefabs.Count)];

				if (finalPrefab != null)
				{
					position += Random.insideUnitSphere * radius;

					var clone     = Instantiate(finalPrefab, position, rotation, default(Transform));
					var rigidbody = clone.GetComponent<Rigidbody>();

					if (rigidbody != null)
					{
						rigidbody.velocity = rotation * velocity;
					}

					clone.SetActive(true);
				}
			}
		}

		public void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			Spawn(position + rotation * Vector3.forward * offsetNormal + offsetWorld, rotation);
		}

		public void UpgradeLegacy()
		{
			if (prefab != null && Prefabs.Count == 0)
			{
				prefabs.Add(prefab);

				prefab = null;
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dSpawner;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dSpawner_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Each(tgts, t => t.UpgradeLegacy());

			BeginError(Any(tgts, t => t.Prefabs.Count == 0));
				Draw("prefabs", "A random prefab from this list will be spawned.");
			EndError();
			Draw("radius", "The spawned prefab will be randomly offset by a random point within this radius in world space.");
			Draw("velocity", "If the prefab contains a Rigidbody, it will be given this velocity in local space.");

			Separator();

			Draw("offsetNormal", "The spawned prefab will be offset from the hit point based on the hit normal by this value in world space.");
			Draw("offsetWorld", "The spawned prefab will be offset from the hit point based on this value in world space.");
		}
	}
}
#endif