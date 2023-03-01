using System.Collections;
using UnityEngine;
using CW.Common;

namespace PaintIn3D
{
	/// <summary>This component listens for <b>point</b> and <b>line</b> painting events. It then simulates transmitting them over a network with a delay, and then painting the received data.</summary>
	[ExecuteInEditMode]
	[HelpURL(P3dCommon.HelpUrlPrefix + "P3dPaintMultiplayer")]
	[AddComponentMenu(P3dCommon.ComponentHitMenuPrefix + "Paint Multiplayer")]
	public class P3dPaintMultiplayer : MonoBehaviour, IHitPoint, IHitLine
	{
		/// <summary>This allows you to specify the simulated delay between painting across the network in seconds.</summary>
		public float Delay { set { delay = value; } get { return delay; } } [SerializeField] private float delay = 0.5f;

		public void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			// NOTE: You should remove this code when you implement actual networking
			{
				// If we painted on the left side, shift the hit to the right side
				if (position.x < 0.0f)
				{
					position.x += 100.0f;
				}
				// If we painted on the right side, shift the hit to the left side
				else
				{
					position.x -= 100.0f;
				}
			}

			// Send the hit data over the fake network
			StartCoroutine(SimulateNetworkTransmission(preview, priority, pressure, seed, position, rotation));
		}

		public void HandleHitLine(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip)
		{
			// NOTE: You should remove this code when you implement actual networking
			{
				// If we painted on the left side, shift the hit to the right side
				if (position.x < 0.0f)
				{
					position.x += 100.0f;
					endPosition.x += 100.0f;
				}
				// If we painted on the right side, shift the hit to the left side
				else
				{
					position.x -= 100.0f;
					endPosition.x -= 100.0f;
				}
			}

			// Send the hit data over the fake network
			StartCoroutine(SimulateNetworkTransmission(preview, priority, pressure, seed, position, endPosition, rotation, clip));
		}

		private IEnumerator SimulateNetworkTransmission(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			// Simulate network delay
			yield return new WaitForSecondsRealtime(delay);

			// Loop through all components that implement IHitPoint
			foreach (var hitPoint in GetComponentsInChildren<IHitPoint>())
			{
				// Ignore this one so we don't recursively paint
				if ((Object)hitPoint != this)
				{
					// Submit the hit point
					hitPoint.HandleHitPoint(preview, priority, pressure, seed, position, rotation);
				}
			}
		}

		private IEnumerator SimulateNetworkTransmission(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip)
		{
			// Simulate network delay
			yield return new WaitForSecondsRealtime(delay);

			// Loop through all components that implement IHitLine
			foreach (var hitLine in GetComponentsInChildren<IHitLine>())
			{
				// Ignore this one so we don't recursively paint
				if ((Object)hitLine != this)
				{
					// Submit the hit line
					hitLine.HandleHitLine(preview, priority, pressure, seed, position, endPosition, rotation, clip);
				}
			}
		}
	}
}

#if UNITY_EDITOR
namespace PaintIn3D
{
	using UnityEditor;
	using TARGET = P3dPaintMultiplayer;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class P3dPaintMultiplayer_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("delay", "This allows you to specify the simulated delay between painting across the network in seconds.");
		}
	}
}
#endif